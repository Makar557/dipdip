using Dip.Models;
using Dip.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Dip.Pages.Restaurant
{
    public class RegisterModel : PageModel
    {
        private readonly РесторанRepository _restaurantRepository;

        [BindProperty]
        public string Name { get; set; }

        [BindProperty]
        public string Description { get; set; }

        [BindProperty]
        public string Address { get; set; }

        [BindProperty]
        public string Phone { get; set; }

        [BindProperty]
        public TimeOnly StartTime { get; set; }

        [BindProperty]
        public int HoursPerDay { get; set; }

        public string ErrorMessage { get; set; }

        public RegisterModel(РесторанRepository restaurantRepository)
        {
            _restaurantRepository = restaurantRepository;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return RedirectToPage("/Account/Login");
                }

                var userId = int.Parse(userIdClaim.Value);

                var restaurant = new Рестораны
                {
                    ПользовательId = userId,
                    Название = Name,
                    Описание = Description,
                    Адрес = Address,
                    Телефон = Phone,
                    НачалоРаботы = StartTime,
                    КоличествоЧасовВДень = HoursPerDay
                };

                await _restaurantRepository.AddAsync(restaurant);

                return RedirectToPage("/Restaurant/Dashboard");
            }
            catch (Exception ex)
            {
                ErrorMessage = "Ошибка при регистрации ресторана: " + ex.Message;
                return Page();
            }
        }
    }
}
