using Dip.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dip.Repository
{
    public class ПользователиRepository
    {
        private readonly DiplomaDbContext _context;

        public ПользователиRepository(DiplomaDbContext context)
        {
            _context = context;
        }
        public async Task<Пользователи> GetUserByIdAsync(int userId)
        {
            return await _context.Пользователиs.FindAsync(userId);
        }

        public async Task AddAsync(Пользователи user)
        {
            _context.Пользователиs.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<Пользователи> GetByIdAsync(int id)
        {
            return await _context.Пользователиs.FindAsync(id);
        }

        public async Task<Пользователи> GetByLoginAsync(string login)
        {
            return await _context.Пользователиs.FirstOrDefaultAsync(u => u.Логин == login);
        }

        public async Task<List<Пользователи>> GetAllAsync()
        {
            return await _context.Пользователиs.ToListAsync();
        }

        public async Task UpdateAsync(Пользователи user)
        {
            var existingUser = await _context.Пользователиs.FindAsync(user.Id);
            if (existingUser != null)
            {
                existingUser.Имя = user.Имя;
                existingUser.Фамилия = user.Фамилия;
                existingUser.ЭлектроннаяПочта = user.ЭлектроннаяПочта;
                existingUser.Логин = user.Логин;
                existingUser.ХэшПароля = user.ХэшПароля;
                existingUser.РолиId = user.РолиId;
                existingUser.ДатаРегистрации = user.ДатаРегистрации;

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _context.Пользователиs.FindAsync(id);
            if (user != null)
            {
                _context.Пользователиs.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
