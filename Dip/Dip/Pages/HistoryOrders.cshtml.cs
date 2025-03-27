using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Dip.Models;
using System.Security.Claims;

namespace Dip.Pages
{
    public class HistoryOrdersModel : PageModel
    {
        private readonly DiplomaDbContext _context;

        public HistoryOrdersModel(DiplomaDbContext context)
        {
            _context = context;
        }

        public Dictionary<DateTime, List<Заказы>> OrdersByMonth { get; set; } = new();

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
                .Where(o => o.КлиентId == userId && o.Статус == "получен заказчиком")
                .OrderByDescending(o => o.ДатаСоздания)
                .AsNoTracking()
                .ToListAsync();

            OrdersByMonth = orders
                .Where(o => o.ДатаСоздания.HasValue)
                .GroupBy(o => new DateTime(o.ДатаСоздания.Value.Year, o.ДатаСоздания.Value.Month, 1))
                .ToDictionary(g => g.Key, g => g.ToList());

            return Page();
        }

        public async Task<IActionResult> OnPostRateRestaurantAsync(int ресторанId, int заказId, int оценка)
        {
            if (оценка < 1 || оценка > 5)
            {
                ModelState.AddModelError("", "Оценка должна быть от 1 до 5.");
                return Page();
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return RedirectToPage("/Account/Login");
            }

            var existingRating = await _context.Оценкиs
                .FirstOrDefaultAsync(r => r.ПользовательId == userId && r.РесторанId == ресторанId);

            if (existingRating != null)
            {
                existingRating.Оценка = оценка;
            }
            else
            {
                _context.Оценкиs.Add(new Оценки
                {
                    ПользовательId = userId,
                    РесторанId = ресторанId,
                    Оценка = оценка
                });
            }

            await _context.SaveChangesAsync();

            var среднийРейтинг = await _context.Оценкиs
                .Where(r => r.РесторанId == ресторанId)
                .AverageAsync(r => r.Оценка);

            var ресторан = await _context.Рестораныs.FindAsync(ресторанId);
            ресторан.Рейтинг = (decimal)среднийРейтинг;

            await _context.SaveChangesAsync();

            return RedirectToPage();
        }


    }
}
