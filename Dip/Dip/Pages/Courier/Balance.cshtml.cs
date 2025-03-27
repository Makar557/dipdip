using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Threading.Tasks;
using Dip.Models;

namespace Dip.Pages.Courier
{
    public class BalanceModel : PageModel
    {
        private readonly DiplomaDbContext _context;

        public BalanceModel(DiplomaDbContext context)
        {
            _context = context;
        }

        public decimal Баланс { get; set; }

        public async Task OnGetAsync()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                Баланс = 0;
                return;
            }

            var курьер = await _context.Пользователиs.FindAsync(userId);
            Баланс = курьер?.Баланс ?? 0;
        }
    }
}
