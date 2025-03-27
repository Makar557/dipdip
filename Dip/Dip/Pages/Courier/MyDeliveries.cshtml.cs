using Dip.Models;
using Dip.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Pages.Courier
{
    public class MyDeliveriesModel : PageModel
    {
        private readonly ЗаказыRepository _orderRepository;
        private readonly ПользователиRepository _userRepository;

        public List<Заказы> MoiZakazy { get; set; }

        public MyDeliveriesModel(ЗаказыRepository orderRepository, ПользователиRepository userRepository)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
        }

        public async Task OnGetAsync()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            MoiZakazy = await _orderRepository.GetZakazyByCourierIdAsync(userId) ?? new List<Заказы>();
        }

        public async Task<IActionResult> OnPostDeliverdAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null || order.Статус != "получен курьером")
            {
                return BadRequest("Невозможно завершить доставку.");
            }

            if (order.КурьерId == null)
            {
                return BadRequest("У заказа нет назначенного курьера.");
            }

            var курьер = await _userRepository.GetUserByIdAsync(order.КурьерId.Value);

            if (курьер == null)
            {
                return BadRequest("Курьер не найден.");
            }

            decimal комиссия = order.ИтоговаяСтоимость  * Заказы.КомиссияБизнеса; 
            decimal заработокКурьера = order.ИтоговаяСтоимость - комиссия; 

            if (order.СпособОплаты == "наличные")
            {
                курьер.Баланс -= комиссия; 
            }
            else 
            {
                курьер.Баланс += заработокКурьера;
            }

            order.Статус = "доставлен";

            await _orderRepository.UpdateAsync(order);
            await _userRepository.UpdateAsync(курьер);

            return RedirectToPage();
        }


    }

}
