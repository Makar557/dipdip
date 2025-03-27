using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dip.Models;

namespace Dip.Pages.Cart
{
    public class CheckoutModel : PageModel
    {
        private readonly DiplomaDbContext _context;

        public CheckoutModel(DiplomaDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string АдресДоставки { get; set; }

        [BindProperty]
        public string СпособОплаты { get; set; }

        [BindProperty]
        public string Квартира { get; set; }

        public async Task<IActionResult> OnPostAsync(bool? payNow)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return RedirectToPage("/Account/Login");
            }

            if (!int.TryParse(userIdClaim.Value, out int userId))
            {
                return BadRequest("Некорректный пользовательский идентификатор.");
            }

            var user = await _context.Пользователиs.FindAsync(userId);

            if (payNow == true && string.IsNullOrEmpty(user.КредитнаяКарта))
            {
                return RedirectToPage("/Order/UpdateCreditCard");
            }

            var cart = await _context.Корзиныs
                .Include(c => c.СоставКорзиныs)
                .ThenInclude(ci => ci.Блюдо)
                .FirstOrDefaultAsync(c => c.ПользовательId == userId && c.Статус == "активная");

            if (cart == null || !cart.СоставКорзиныs.Any())
            {
                return BadRequest("Корзина пуста.");
            }

            var order = new Заказы
            {
                КлиентId = userId,
                РесторанId = cart.СоставКорзиныs.First().Блюдо.РесторанId,
                АдресДоставки = АдресДоставки,
                Квартира = Квартира,
                ИтоговаяСтоимость = (cart.ИтоговаяСтоимость ?? 0) * 1.5m,
                Статус = payNow == true ? "оплачено" : "ожидает",
                СпособОплаты = СпособОплаты,
                ДатаСоздания = DateTime.Now
            };

            _context.Заказыs.Add(order);
            await _context.SaveChangesAsync();

            var orderItems = cart.СоставКорзиныs.Select(item => new СоставЗаказа
            {
                ЗаказId = order.Id,
                БлюдоId = item.БлюдоId,
                Количество = item.Количество,
                Цена = item.Цена
            }).ToList();

            _context.СоставЗаказаs.AddRange(orderItems);

            _context.СоставКорзиныs.RemoveRange(cart.СоставКорзиныs);
            cart.Статус = "оформлена";
            cart.ИтоговаяСтоимость = 0;
            await _context.SaveChangesAsync();

            return RedirectToPage("/Order/Success", new { orderId = order.Id });
        }
    }
}