using Dip.Models;
using Dip.Repository;
using Google;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Dip.Pages.Restaurant
{
    public class MenuModel : PageModel
    {
        private readonly МенюRepository _menuRepository;
        private readonly РесторанRepository _restaurantRepository;
        private readonly CloudinaryService _cloudinaryService;

        public string RestaurantName { get; set; }
        public List<Меню> MenuItems { get; set; } = new List<Меню>();
        public Меню SelectedMenuItem { get; set; } 
        private readonly IWebHostEnvironment _environment;
        private readonly DiplomaDbContext _context;

        public MenuModel(МенюRepository menuRepository, РесторанRepository restaurantRepository, CloudinaryService cloudinaryService, DiplomaDbContext context, IWebHostEnvironment environment)
        {
            _menuRepository = menuRepository;
            _restaurantRepository = restaurantRepository;
            _cloudinaryService = cloudinaryService;
            _context = context;
            _environment = environment;
        }



        public async Task OnGetAsync()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                RedirectToPage("/Account/Login");
                return;
            }

            var userId = int.Parse(userIdClaim.Value);
            var restaurant = await _restaurantRepository.GetByUserIdAsync(userId);

            if (restaurant == null)
            {
                RedirectToPage("/Account/Login");
                return;
            }

            RestaurantName = restaurant.Название;
            MenuItems = await _menuRepository.GetMenuByRestaurantIdAsync(restaurant.Id);
        }

        public async Task<IActionResult> OnPostEditMenuItemAsync(int Id, string Название, string Описание, decimal Цена, IFormFile Фото)
        {
            var item = await _context.Менюs.FindAsync(Id);
            if (item == null)
            {
                return NotFound();
            }

            item.Название = Название;
            item.Описание = Описание;
            item.Цена = Цена;

            if (Фото != null && Фото.Length > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(Фото.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Фото.CopyToAsync(fileStream);
                }

                item.СсылкаНаИзображение = "/uploads/" + uniqueFileName;
            }

            await _context.SaveChangesAsync();
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSelectMenuItemAsync(int Id)
        {
            var menuItem = await _menuRepository.GetMenuItemByIdAsync(Id);
            if (menuItem == null)
            {
                return NotFound();
            }
            SelectedMenuItem = menuItem;
            return Page();
        }

        public async Task<IActionResult> OnPostAddMenuItemAsync(string Название, string Описание, decimal Цена, IFormFile Фото)
        {
            if (string.IsNullOrWhiteSpace(Название))
            {
                ModelState.AddModelError(string.Empty, "Название обязательно.");
                return Page();
            }

            if (Фото == null || Фото.Length == 0)
            {
                ModelState.AddModelError(string.Empty, "Необходимо загрузить фото.");
                return Page();
            }

            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return RedirectToPage("/Account/Login");
            }

            var userId = int.Parse(userIdClaim.Value);
            var restaurant = await _restaurantRepository.GetByUserIdAsync(userId);
            if (restaurant == null)
            {
                return RedirectToPage("/Account/Login");
            }

            string imageUrl = await _cloudinaryService.UploadImageAsync(Фото);

            var newMenuItem = new Меню
            {
                РесторанId = restaurant.Id,
                Название = Название,
                Описание = Описание,
                Цена = Цена,
                СсылкаНаИзображение = imageUrl, 
                Доступность = true
            };

            await _menuRepository.AddMenuItemAsync(newMenuItem);
            return RedirectToPage();
        }




        public async Task<IActionResult> OnPostDeleteMenuItemAsync(int Id)
        {
            var menuItem = await _menuRepository.GetMenuItemByIdAsync(Id);
            if (menuItem == null)
            {
                return NotFound();
            }

            await _menuRepository.DeleteMenuItemAsync(Id);
            return RedirectToPage(); 
        }

        public async Task<IActionResult> OnPostToggleAvailabilityAsync(int Id)
        {
            var menuItem = await _menuRepository.GetMenuItemByIdAsync(Id);
            if (menuItem == null)
            {
                return NotFound();
            }

            await _menuRepository.ToggleMenuItemAvailabilityAsync(Id);
            return RedirectToPage();
        }


    }

}
