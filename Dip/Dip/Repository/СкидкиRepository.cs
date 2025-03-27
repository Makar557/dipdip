using Dip.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dip.Repository
{
    public class СкидкиRepository
    {
        private readonly DiplomaDbContext _context;

        public СкидкиRepository(DiplomaDbContext context)
        {
            _context = context;
        }

        public async Task<List<Скидки>> GetByRestaurantIdAsync(int restaurantId)
        {
            return await _context.Скидкиs
                .Include(s => s.Меню)
                .Where(s => s.Меню.РесторанId == restaurantId)
                .ToListAsync();
        }

        public async Task AddAsync(Скидки скидка)
        {
            _context.Скидкиs.Add(скидка);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(int скидкаId)
        {
            var скидка = await _context.Скидкиs.FindAsync(скидкаId);
            if (скидка != null)
            {
                _context.Скидкиs.Remove(скидка);
                await _context.SaveChangesAsync();
            }
        }
    }
}