using Dip.Models;
using Dip.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Dip.Pages.Courier
{
    public class OrderHistoryModel : PageModel
    {
        private readonly ЗаказыRepository _ordersRepository;

        public Dictionary<string, List<Заказы>> OrdersGroupedByMonth { get; set; } 
        public Dictionary<string, decimal> TotalEarningsByMonth { get; set; } 

        public OrderHistoryModel(ЗаказыRepository ordersRepository)
        {
            _ordersRepository = ordersRepository;
        }

        public async Task OnGetAsync()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                RedirectToPage("/Account/Login");
                return;
            }

            var orders = await _ordersRepository.GetZakazyByCourierIdAsync(userId);

            var filteredOrders = orders
                .Where(o => o.Статус == "получен заказчиком" || o.Статус == "отменен")
                .ToList();

            OrdersGroupedByMonth = filteredOrders
                .GroupBy(o => new { Year = o.ДатаДоставки?.Year, Month = o.ДатаДоставки?.Month })
                .Where(g => g.Key.Year.HasValue && g.Key.Month.HasValue)
                .OrderByDescending(g => g.Key.Year)
                .ThenByDescending(g => g.Key.Month)
                .ToDictionary(
                    g => $"{g.Key.Year.Value}-{g.Key.Month.Value:D2}", 
                    g => g.ToList());

            TotalEarningsByMonth = OrdersGroupedByMonth
                .ToDictionary(
                    g => g.Key,
                    g => g.Value
                        .Where(o => o.Статус == "получен заказчиком")  
                        .Sum(o => o.ИтоговаяСтоимость));
        }
    }
}
