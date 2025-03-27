using Dip.Models;
using Dip.Repository;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dip.Pages.Restaurant
{
    public class DashboardModel : PageModel
    {
        private readonly РесторанRepository _restaurantRepository;

        public int UserId { get; private set; }

        public string RestaurantName { get; set; }

        public DashboardModel(РесторанRepository restaurantRepository)
        {
            _restaurantRepository = restaurantRepository;
        }

        public async Task OnGetAsync()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                RedirectToPage("/Account/Login");
            }
            else
            {
                UserId = int.Parse(userIdClaim.Value);
            }
            var restaurant = await _restaurantRepository.GetByUserIdAsync(UserId);
            RestaurantName = restaurant?.Название ?? "Ваш ресторан";
        }
    }
}
