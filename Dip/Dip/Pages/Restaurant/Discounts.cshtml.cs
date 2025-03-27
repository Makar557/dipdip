using Dip.Models;
using Dip.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dip.Pages.Restaurant
{
    public class DiscountsModel : PageModel
    {
        private readonly СкидкиRepository _скидкиRepository;
        private readonly МенюRepository _менюRepository;
        private readonly РесторанRepository _ресторанRepository;

        public List<Меню> Блюда { get; set; }
        public List<Скидки> Скидки { get; set; }
        public int RestaurantId { get; private set; }

        public DiscountsModel(СкидкиRepository скидкиRepository, МенюRepository менюRepository, РесторанRepository ресторанRepository)
        {
            _скидкиRepository = скидкиRepository;
            _менюRepository = менюRepository;
            _ресторанRepository = ресторанRepository;
        }

        public async Task OnGetAsync()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                RedirectToPage("/Account/Login");
                return;
            }

            int userId = int.Parse(userIdClaim.Value);
            var ресторан = await _ресторанRepository.GetByUserIdAsync(userId);
            if (ресторан == null)
            {
                RedirectToPage("/Restaurant/Dashboard");
                return;
            }

            RestaurantId = ресторан.Id;
            Блюда = await _менюRepository.GetByRestaurantIdAsync(RestaurantId);
            Скидки = await _скидкиRepository.GetByRestaurantIdAsync(RestaurantId);
        }

        public async Task<IActionResult> OnPostAsync(int БлюдоId, decimal ПроцентСкидки, DateTime ДатаНачала, DateTime ДатаОкончания)
        {
            var скидка = new Скидки
            {
                МенюId = БлюдоId,
                ПроцентСкидки = ПроцентСкидки,
                ДатаНачала = DateOnly.FromDateTime(ДатаНачала),   
                ДатаОкончания = DateOnly.FromDateTime(ДатаОкончания) 
            };

            await _скидкиRepository.AddAsync(скидка);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int СкидкаId)
        {
            await _скидкиRepository.RemoveAsync(СкидкаId);
            return RedirectToPage();
        }
    }
}
