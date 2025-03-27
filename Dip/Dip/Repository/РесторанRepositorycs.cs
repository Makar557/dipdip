using Dip.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dip.Repository
{
    public class РесторанRepository
    {
        private readonly DiplomaDbContext _context;

        public РесторанRepository(DiplomaDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Рестораны ресторан)
        {
            _context.Рестораныs.Add(ресторан);
            await _context.SaveChangesAsync();
        }
        public async Task<Рестораны> s(int id)
        {
            return await _context.Рестораныs.FindAsync(id);
        }

        public async Task<List<Рестораны>> GetAllAsync()
        {
            return await _context.Рестораныs.ToListAsync();
        }

        public async Task<List<Рестораны>> GetByNameAsync(string название)
        {
            return await _context.Рестораныs
                .Where(r => EF.Functions.Like(r.Название, $"%{название}%"))
                .ToListAsync();
        }

        public async Task UpdateAsync(Рестораны restaurant)
        {
            try
            {
                var existing = await _context.Рестораныs.FindAsync(restaurant.Id);
                if (existing == null) throw new Exception("Ресторан не найден");

                _context.Entry(existing).CurrentValues.SetValues(restaurant);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обновлении: {ex.Message}");
                throw;
            }
        }


        public async Task<Пользователи?> GetUserByIdAsync(int userId)
        {
            return await _context.Пользователиs.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task DeleteAsync(int id)
        {
            var ресторан = await _context.Рестораныs.FindAsync(id);
            if (ресторан != null)
            {
                _context.Рестораныs.Remove(ресторан);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Рестораны>> SearchAsync(string searchTerm)
        {
            return await _context.Рестораныs
                .Where(r => EF.Functions.Like(r.Название, $"%{searchTerm}%") ||
                            EF.Functions.Like(r.Описание, $"%{searchTerm}%"))
                .ToListAsync();
        }

        public async Task<Рестораны?> GetByUserIdAsync(int userId)
        {
            return await _context.Рестораныs.FirstOrDefaultAsync(r => r.ПользовательId == userId);
        }


    }
}
