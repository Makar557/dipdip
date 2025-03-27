using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Dip.Models;

namespace Dip.Pages.Order
{
    public class SuccessModel : PageModel
    {
        private readonly DiplomaDbContext _context;

        public SuccessModel(DiplomaDbContext context)
        {
            _context = context;
        }

        public int OrderId { get; set; }
        public bool IsPaid { get; set; }

        public async Task<IActionResult> OnGetAsync(int orderId)
        {
            var order = await _context.Заказыs.FindAsync(orderId);
            if (order == null)
            {
                return NotFound();
            }

            OrderId = orderId;
            IsPaid = order.Статус == "оплачено";

            return Page();
        }
    }
}
