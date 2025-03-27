using Dip.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Dip.Pages
{
    public class CartModel : PageModel
    {
        private readonly DiplomaDbContext _context;

        public CartModel(DiplomaDbContext context)
        {
            _context = context;
        }
        public bool CanOrder { get; set; } = true;
        public Корзины Cart { get; set; }
        public List<СоставКорзины> CartItems { get; set; } = new List<СоставКорзины>();
        public decimal TotalPrice { get; set; }

        public async Task<IActionResult> OnPostRemoveItemAsync(int itemId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToPage("/Account/Login");
            }

            var cartItem = await _context.СоставКорзиныs
                .Include(ci => ci.Корзина)
                .FirstOrDefaultAsync(ci => ci.Id == itemId && ci.Корзина.ПользовательId == int.Parse(userId) && ci.Корзина.Статус == "активная");

            if (cartItem == null)
            {
                TempData["ErrorMessage"] = "Товар не найден в корзине.";
                return RedirectToPage();
            }

            var cart = cartItem.Корзина;
            cart.ИтоговаяСтоимость = Math.Max(0m, (cart.ИтоговаяСтоимость ?? 0m) -
                (cartItem.Количество * cartItem.Цена * 1.5m));

            _context.СоставКорзиныs.Remove(cartItem);

            if (!await _context.СоставКорзиныs.AnyAsync(ci => ci.КорзинаId == cart.Id))
            {
                cart.ИтоговаяСтоимость = 0;
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Товар удалён из корзины.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnGet()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToPage("/Account/Login");
            }

            Cart = await _context.Корзиныs
                .Where(c => c.ПользовательId == int.Parse(userId) && c.Статус == "активная")
                .FirstOrDefaultAsync();

            if (Cart != null)
            {
                CartItems = await _context.СоставКорзиныs
                    .Include(ci => ci.Блюдо)
                    .ThenInclude(b => b.Ресторан)
                    .Where(ci => ci.КорзинаId == Cart.Id)
                    .ToListAsync();

                TotalPrice = CartItems.Sum(item => item.Цена * 1.5m);

                if (CartItems.Any() && CartItems.Select(ci => ci.Блюдо.РесторанId).Distinct().Count() > 1)
                {
                    CanOrder = false;
                }
            }

            return Page();
        }


        public async Task<IActionResult> OnPostOrder()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToPage("/Account/Login");
            }

            var cart = await _context.Корзиныs
                .Where(c => c.ПользовательId == int.Parse(userId) && c.Статус == "активная")
                .FirstOrDefaultAsync();

            if (cart == null)
            {
                TempData["ErrorMessage"] = "Ваша корзина пуста!";
                return RedirectToPage();
            }

            var cartItems = await _context.СоставКорзиныs
                .Include(ci => ci.Блюдо)
                .ThenInclude(b => b.Ресторан)
                .Where(ci => ci.КорзинаId == cart.Id)
                .ToListAsync();

            if (!cartItems.Any())
            {
                TempData["ErrorMessage"] = "Ваша корзина пуста!";
                return RedirectToPage();
            }

            int? ресторанId = cartItems.First().Блюдо.РесторанId;
            if (cartItems.Any(ci => ci.Блюдо.РесторанId != ресторанId))
            {
                TempData["ErrorMessage"] = "Вы не можете оформить заказ, так как в корзине есть блюда из разных ресторанов.";
                return RedirectToPage();
            }

            cart.ИтоговаяСтоимость = cartItems.Sum(item => item.Количество * item.Цена * 1.5m);

            cart.Статус = "оформлена";
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Заказ успешно оформлен!";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostClearCartAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToPage("/Account/Login");
            }

            var cart = await _context.Корзиныs
                .Where(c => c.ПользовательId == int.Parse(userId) && c.Статус == "активная")
                .FirstOrDefaultAsync();

            if (cart != null)
            {
                var cartItems = _context.СоставКорзиныs.Where(ci => ci.КорзинаId == cart.Id);
                _context.СоставКорзиныs.RemoveRange(cartItems);
                cart.ИтоговаяСтоимость = 0;

                await _context.SaveChangesAsync();
            }

            TempData["SuccessMessage"] = "Корзина очищена.";
            return RedirectToPage();
        }

    }
}