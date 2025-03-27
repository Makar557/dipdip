using Dip.Models;
using Dip.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Dip.Pages.Restaurant
{
    public class EditModel : PageModel
    {
        private readonly РесторанRepository _restaurantRepository;
        private readonly CloudinaryService _cloudinaryService;

        [BindProperty]
        public Рестораны Ресторан { get; set; }

        [BindProperty]
        [Display(Name = "Логотип ресторана")]
        public IFormFile? ЗагруженныйЛоготип { get; set; }

        public EditModel(РесторанRepository restaurantRepository, CloudinaryService cloudinaryService)
        {
            _restaurantRepository = restaurantRepository;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return RedirectToPage("/Account/Login");
            }

            int userId = int.Parse(userIdClaim.Value);
            Ресторан = await _restaurantRepository.GetByUserIdAsync(userId);

            if (Ресторан == null)
            {
                return NotFound("Ресторан не найден.");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Ресторан.Пользователь"); 

            if (!ModelState.IsValid)
            {
                foreach (var modelError in ModelState)
                {
                    foreach (var error in modelError.Value.Errors)
                    {
                        Console.WriteLine($"Ошибка в поле {modelError.Key}: {error.ErrorMessage}");
                    }
                }
                return Page();
            }

            try
            {
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return RedirectToPage("/Account/Login");
                }

                int userId = int.Parse(userIdClaim.Value);
                var existingRestaurant = await _restaurantRepository.GetByUserIdAsync(userId);
                if (existingRestaurant == null)
                {
                    return NotFound("Ресторан не найден.");
                }

                var пользователь = await _restaurantRepository.GetUserByIdAsync(userId);
                if (пользователь == null)
                {
                    return NotFound("Пользователь не найден.");
                }

                existingRestaurant.Название = Ресторан.Название;
                existingRestaurant.Описание = Ресторан.Описание;
                existingRestaurant.Адрес = Ресторан.Адрес;
                existingRestaurant.Телефон = Ресторан.Телефон;
                existingRestaurant.НачалоРаботы = Ресторан.НачалоРаботы;
                existingRestaurant.КоличествоЧасовВДень = Ресторан.КоличествоЧасовВДень;
                existingRestaurant.Пользователь = пользователь; 

                if (ЗагруженныйЛоготип != null)
                {
                    existingRestaurant.Логотип = await _cloudinaryService.UploadImageAsync(ЗагруженныйЛоготип);
                }

                await _restaurantRepository.UpdateAsync(existingRestaurant);
                return Page();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка обновления ресторана: {ex.Message}");
                ModelState.AddModelError("", "Произошла ошибка при сохранении данных.");
                return Page();
            }
        }


    }
}
