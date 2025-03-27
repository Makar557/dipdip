using Dip.Models;
using Dip.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dip.Pages.Restaurant
{
    public class OrdersModel : PageModel
    {
        private readonly ЗаказыRepository _ordersRepository;
        private readonly РесторанRepository _restaurantRepository;
        private readonly DiplomaDbContext _context;


        public Dictionary<string, List<Заказы>> OrdersGroupedByMonth { get; set; }
        public string RestaurantName { get; set; }

        public OrdersModel(DiplomaDbContext context, ЗаказыRepository ordersRepository, РесторанRepository restaurantRepository)
        {
            _context = context;
            _ordersRepository = ordersRepository;
            _restaurantRepository = restaurantRepository;
        }
        private async Task LoadOrderItemsAsync(List<Заказы> orders)
        {
            var orderIds = orders.Select(o => o.Id).ToList();

            var orderItems = await _context.СоставЗаказаs
                .Where(s => orderIds.Contains(s.ЗаказId))
                .Include(s => s.Блюдо)
                .ToListAsync();

            var orderItemsDict = orderItems.GroupBy(s => s.ЗаказId)
                .ToDictionary(g => g.Key, g => g.ToList());

            ViewData["OrderItems"] = orderItemsDict;
        }


        public async Task OnGetAsync()
        {
            

            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                RedirectToPage("/Account/Login");
                return;
            }

            if (!int.TryParse(userIdClaim.Value, out int userId))
            {
                RedirectToPage("/Account/Login");
                return;
            }

            var restaurant = await _restaurantRepository.GetByUserIdAsync(userId);
            if (restaurant == null)
            {
                RedirectToPage("/Error");
                return;
            }

            RestaurantName = restaurant.Название;

            var orders = await _ordersRepository.GetOrdersByRestaurantAsync(restaurant.Id);
            OrdersGroupedByMonth = orders
                .Where(o => o.Статус == "ожидает" || o.Статус == "приготовлен" || o.Статус == "получен курьером" || o.Статус == "ГОТОВ" || o.Статус == "оплачено")
                .GroupBy(o => new { Year = o.ДатаСоздания?.Year, Month = o.ДатаСоздания?.Month }) 
                .Where(g => g.Key.Year.HasValue && g.Key.Month.HasValue) 
                .OrderByDescending(g => g.Key.Year)
                .ThenByDescending(g => g.Key.Month)
                .ToDictionary(
                    g => $"{g.Key.Year.Value}-{g.Key.Month.Value:D2}", 
                    g => g.OrderBy(o => o.Статус == "ожидает" ? 1 :
                                       o.Статус == "оплачено" ? 2 :
                                       o.Статус == "приготовлен" ? 3 :
                                       o.Статус == "получен курьером" ? 4 : 5) 
                        .ToList());
            await LoadOrderItemsAsync(orders);
        }

        public async Task<IActionResult> OnPostChangeStatusAsync(int orderId)
        {
            var order = await _ordersRepository.GetByIdAsync(orderId);
            if (order == null || order.Статус != "ожидает" && order.Статус != "оплачено")
            {
                return RedirectToPage();
            }

            order.Статус = "приготовлен";
            await _ordersRepository.UpdateAsync(order);

            return RedirectToPage();
        }

    }
}
