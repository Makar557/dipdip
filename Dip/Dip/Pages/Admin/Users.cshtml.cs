using Dip.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dip.Pages.Admin
{
    [Authorize(Roles = "4")]
    public class UsersModel : PageModel
    {
        private readonly DiplomaDbContext _context;

        public UsersModel(DiplomaDbContext context)
        {
            _context = context;
        }

        public List<UserViewModel> Users { get; set; } = new();
        public List<Роли> Roles { get; set; } = new();

        [BindProperty]
        public UserViewModel EditUser { get; set; } = new();

        public async Task OnGetAsync()
        {
            Roles = await _context.Ролиs.ToListAsync(); 

            Users = await _context.Пользователиs
                .Include(u => u.Роли) 
                .Select(u => new UserViewModel
                {
                    Id = u.Id,
                    Имя = u.Имя,
                    ЭлектроннаяПочта = u.ЭлектроннаяПочта,
                    РольНазвание = u.Роли.Роль, 
                    РолиId = u.РолиId
                })
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var user = await _context.Пользователиs.FindAsync(id);
            if (user != null)
            {
                _context.Пользователиs.Remove(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _context.Пользователиs.FindAsync(EditUser.Id);
            if (user != null)
            {
                user.Имя = EditUser.Имя;
                user.ЭлектроннаяПочта = EditUser.ЭлектроннаяПочта;
                user.РолиId = EditUser.РолиId;

                _context.Пользователиs.Update(user);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }

    public class UserViewModel
    {
        public int Id { get; set; }
        public string Имя { get; set; } = "";
        public string ЭлектроннаяПочта { get; set; } = "";
        public int РолиId { get; set; }
        public string РольНазвание { get; set; } = ""; 
    }
}
