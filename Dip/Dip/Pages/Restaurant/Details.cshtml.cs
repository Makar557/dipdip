using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Dip.Models;
using Dip.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Dip.Pages.Restaurant
{
    public class DetailsModel : PageModel
    {
        private readonly DiplomaDbContext _context;
        private readonly КорзинаRepository _cartRepository;

        public Рестораны Restaurant { get; set; }
        public List<Меню> MenuItems { get; set; } = new List<Меню>();

        public string WorkingHours { get; set; }

        public DetailsModel(DiplomaDbContext context, КорзинаRepository cartRepository)
        {
            _context = context;
            _cartRepository = cartRepository;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Restaurant = await _context.Рестораныs
                .Include(r => r.Менюs)
                .ThenInclude(m => m.Скидкиs) 
                .FirstOrDefaultAsync(r => r.Id == id);

            if (Restaurant == null)
            {
                return NotFound();
            }

            MenuItems = Restaurant.Менюs
                .Where(m => m.Доступность)
                .OrderByDescending(m => m.АктуальнаяСкидка != null) 
                .ToList();
            DateTime startTime = DateTime.Today.Add(Restaurant.НачалоРаботы.ToTimeSpan());
            DateTime endTime = startTime.AddHours(Restaurant.КоличествоЧасовВДень);
            WorkingHours = $"{startTime:HH\\:mm} - {endTime:HH\\:mm}";

            return Page();
        }


        public async Task<IActionResult> OnPost(int dishId)
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

            var dish = await _context.Менюs.FindAsync(dishId);
            if (dish == null) return NotFound();

            _cartRepository.AddToCart(userId, dishId);

            Restaurant = await _context.Рестораныs.FirstOrDefaultAsync(r => r.Id == dish.РесторанId);

            if (Restaurant == null) return NotFound();

            return RedirectToPage(new { id = Restaurant.Id });
        }
    }
}
