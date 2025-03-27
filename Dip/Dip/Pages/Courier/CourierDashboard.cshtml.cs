using Dip.Models;
using Dip.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Pages.Courier
{
    public class CourierDashboardModel : PageModel
    {
        private readonly ЗаказыRepository _orderRepository;

        public List<Заказы> DostupnyeZakazy { get; set; }
        public List<Заказы> MoiZakazy { get; set; }

        public CourierDashboardModel(ЗаказыRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task OnGetAsync()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var allOrders = await _orderRepository.GetZakazyByCourierIdAsync(userId);

            MoiZakazy = allOrders
                .Where(o => o.Статус != "получен заказчиком" && o.Статус != "отменен")
                .ToList();

            DostupnyeZakazy = await _orderRepository.GetAktualnyeZakazyAsync();
        }

        public async Task<IActionResult> OnPostPrinyatZakazAsync(int orderId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var allOrders = await _orderRepository.GetZakazyByCourierIdAsync(userId);
            var activeOrders = allOrders
                .Where(o => o.Статус != "получен заказчиком" && o.Статус != "отменен")
                .ToList();

            if (activeOrders.Count == 0) 
            {
                await _orderRepository.AssignCourierToOrderAsync(orderId, userId);
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostStartDeliveryAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);

            if (order != null && order.Статус == "приготовлен")
            {
                order.Статус = "получен курьером";
                await _orderRepository.UpdateAsync(order);
            }

            return RedirectToPage();
        }
    }
}
