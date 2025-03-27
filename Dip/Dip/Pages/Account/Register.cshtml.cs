namespace Dip.Pages.Account
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Dip.Models;
    using Dip.Repository;

    public class RegisterModel : PageModel
    {
        private readonly ПользователиRepository _userRepository;

        [BindProperty]
        public string FirstName { get; set; }

        [BindProperty]
        public string PhoneNumber { get; set; }

        [BindProperty]
        public string LastName { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        [BindProperty]
        public string ConfirmPassword { get; set; }

        public string ErrorMessage { get; set; }

        public RegisterModel(ПользователиRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Password != ConfirmPassword)
            {
                ErrorMessage = "Пароли не совпадают!";
                return Page();
            }

            try
            {
                var newUser = new Пользователи
                {
                    Имя = FirstName,
                    Фамилия = LastName,
                    ЭлектроннаяПочта = Email,
                    Логин = Username,
                    ХэшПароля = HashPassword(Password),
                    РолиId = 2,
                    ДатаРегистрации = DateTime.Now,
                    Телефон = PhoneNumber
                };

                await _userRepository.AddAsync(newUser);

                return RedirectToPage("/Account/Login");
            }
            catch (Exception ex)
            {
                ErrorMessage = "Ошибка при регистрации: " + ex.Message;
                return Page();
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }

}
