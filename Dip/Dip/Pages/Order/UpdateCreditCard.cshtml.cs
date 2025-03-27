using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Dip.Models;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Dip.Pages.Order
{
    public class UpdateCreditCardModel : PageModel
    {
        private readonly DiplomaDbContext _context;

        public UpdateCreditCardModel(DiplomaDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string CreditCardNumber { get; set; }

        public async Task<IActionResult> OnPostAsync()
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
            if (user == null)
            {
                return NotFound();
            }
            user.КредитнаяКарта = CreditCardNumber;
            await _context.SaveChangesAsync();

            return RedirectToPage("/Order/Checkout");
        }
    }
}
