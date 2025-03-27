using Dip.Models;
using Microsoft.EntityFrameworkCore;

namespace Dip.Repository
{
    public class МенюRepository
    {
        private readonly DiplomaDbContext _context;

        public МенюRepository(DiplomaDbContext context)
        {
            _context = context;
        }

        public async Task<List<Меню>> GetMenuByRestaurantIdAsync(int restaurantId)
        {
            return await _context.Менюs
                .Where(m => m.РесторанId == restaurantId)
                .ToListAsync();
        }

        public async Task AddMenuItemAsync(Меню menuItem)
        {
            _context.Менюs.Add(menuItem);
            await _context.SaveChangesAsync();
        }

        public async Task ToggleMenuItemAvailabilityAsync(int menuItemId)
        {
            var menuItem = await _context.Менюs.FindAsync(menuItemId);
            if (menuItem != null)
            {
                menuItem.Доступность = !menuItem.Доступность;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateMenuItemAsync(Меню menuItem)
        {
            var existingItem = await _context.Менюs.FindAsync(menuItem.Id);
            if (existingItem != null)
            {
                existingItem.Название = menuItem.Название;
                existingItem.Описание = menuItem.Описание;
                existingItem.Цена = menuItem.Цена;
                existingItem.СсылкаНаИзображение = menuItem.СсылкаНаИзображение;

                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<Меню>> GetByRestaurantIdAsync(int restaurantId)
        {
            return await _context.Менюs
                .Where(m => m.РесторанId == restaurantId)
                .ToListAsync();
        }

        public async Task DeleteMenuItemAsync(int menuItemId)
        {
            var menuItem = await _context.Менюs.FindAsync(menuItemId);
            if (menuItem != null)
            {
                _context.Менюs.Remove(menuItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Меню?> GetMenuItemByIdAsync(int id)
        {
            return await _context.Менюs.FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
