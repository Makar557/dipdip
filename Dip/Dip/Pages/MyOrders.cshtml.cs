using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Dip.Models;
using System.Security.Claims;

namespace Dip.Pages
{
    public class MyOrdersModel : PageModel
    {
        private readonly DiplomaDbContext _context;

        public MyOrdersModel(DiplomaDbContext context)
        {
            _context = context;
        }

        public Dictionary<DateTime, List<OrderWithItems>> OrdersByMonth { get; set; } = new();

        public class OrderWithItems
        {
            public Заказы Order { get; set; } = null!;
            public List<СоставЗаказа> Items { get; set; } = new();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login");
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return RedirectToPage("/Account/Login");
            }

            var orders = await _context.Заказыs
                .Where(o => o.КлиентId == userId && o.Статус != "получен заказчиком")
                .OrderByDescending(o => o.ДатаСоздания)
                .AsNoTracking()
                .Select(o => new OrderWithItems
                {
                    Order = o,
                    Items = _context.СоставЗаказаs
                        .Where(s => s.ЗаказId == o.Id)
                        .Include(s => s.Блюдо)
                        .ToList()
                })
                .ToListAsync();

            OrdersByMonth = orders
                .Where(o => o.Order.ДатаСоздания.HasValue)
                .GroupBy(o => new DateTime(o.Order.ДатаСоздания.Value.Year, o.Order.ДатаСоздания.Value.Month, 1))
                .ToDictionary(g => g.Key, g => g.ToList());

            return Page();
        }

        public async Task<IActionResult> OnPostUpdateStatusAsync(int orderId)
        {
            var order = await _context.Заказыs.FindAsync(orderId);
            if (order == null || order.Статус != "доставлен")
            {
                return NotFound();
            }

            order.Статус = "получен заказчиком";
            order.ДатаДоставки = DateTime.Now;

            var orderItems = _context.СоставЗаказаs.Where(s => s.ЗаказId == orderId);
            _context.СоставЗаказаs.RemoveRange(orderItems);

            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}