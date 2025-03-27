using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dip.Pages.Admin
{
    [Authorize(Roles = "4")]
    public class AdminDashboardModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
