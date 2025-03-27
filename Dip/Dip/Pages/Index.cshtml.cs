using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Dip.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Dip.Pages
{
    public class IndexModel : PageModel
    {
        private readonly DiplomaDbContext _context;

        public IndexModel(DiplomaDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string SearchQuery { get; set; }

        public List<Рестораны> Restaurants { get; set; } = new List<Рестораны>();

        public void OnGet()
        {
            Restaurants = _context.Рестораныs
                .Include(r => r.Менюs)
                .AsNoTracking()
                .ToList();
        }

        public void OnPost()
        {
            if (!string.IsNullOrEmpty(SearchQuery))
            {
                Restaurants = _context.Рестораныs
                    .Where(r => r.Название.ToLower().Contains(SearchQuery.ToLower()))
                    .Include(r => r.Менюs)
                    .AsNoTracking()
                    .ToList();
            }
            else
            {
                Restaurants = _context.Рестораныs
                    .Include(r => r.Менюs)
                    .AsNoTracking()
                    .ToList();
            }
        }
    }
}
