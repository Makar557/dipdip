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

        public List<���������> Restaurants { get; set; } = new List<���������>();

        public void OnGet()
        {
            Restaurants = _context.���������s
                .Include(r => r.����s)
                .AsNoTracking()
                .ToList();
        }

        public void OnPost()
        {
            if (!string.IsNullOrEmpty(SearchQuery))
            {
                Restaurants = _context.���������s
                    .Where(r => r.��������.ToLower().Contains(SearchQuery.ToLower()))
                    .Include(r => r.����s)
                    .AsNoTracking()
                    .ToList();
            }
            else
            {
                Restaurants = _context.���������s
                    .Include(r => r.����s)
                    .AsNoTracking()
                    .ToList();
            }
        }
    }
}
