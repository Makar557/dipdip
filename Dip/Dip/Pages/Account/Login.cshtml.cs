using Dip.Models;
using Dip.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly ПользователиRepository _userRepository;
        private readonly РесторанRepository _restaurantRepository;
        private readonly IPasswordHasher<Пользователи> _passwordHasher;

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public LoginModel(ПользователиRepository userRepository, РесторанRepository restaurantRepository)
        {
            _userRepository = userRepository;
            _restaurantRepository = restaurantRepository;
            _passwordHasher = new PasswordHasher<Пользователи>();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userRepository.GetByLoginAsync(Username);

            if (user == null)
            {
                ErrorMessage = "Неверный логин или пароль!";
                return Page();
            }

            if (VerifyPassword(Password, user.ХэшПароля))
            {
                var claims = new List<Claim>
                    {
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                            new Claim(ClaimTypes.Name, user.Логин),
                            new Claim(ClaimTypes.Role, user.РолиId.ToString()) 
                         };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                if (user.РолиId == 4) 
                {
                    return RedirectToPage("/Admin/AdminDashboard");
                }
                else if (user.РолиId == 3)
                {
                    var restaurant = await _restaurantRepository.GetByUserIdAsync(user.Id);
                    if (restaurant == null)
                    {
                        return RedirectToPage("/Restaurant/Register");
                    }
                    else
                    {
                        return RedirectToPage("/Restaurant/Dashboard");
                    }
                }
                else if (user.РолиId == 2) 
                {
                    return RedirectToPage("/Courier/CourierDashboard");
                }

                return RedirectToPage("/Index");
            }


            ErrorMessage = "Неверный логин или пароль!";
            return Page();
        }
        private bool VerifyPassword(string enteredPassword, string storedHash)
        {
            using (var sha256 = SHA256.Create())
            {
                var enteredPasswordBytes = Encoding.UTF8.GetBytes(enteredPassword);
                var enteredPasswordHash = sha256.ComputeHash(enteredPasswordBytes);
                var enteredPasswordHashBase64 = Convert.ToBase64String(enteredPasswordHash);
                return enteredPasswordHashBase64 == storedHash;
            }
        }
    }
}
