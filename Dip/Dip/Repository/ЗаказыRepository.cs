using Dip.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dip.Repository
{

    public class ЗаказыRepository
    {
        private readonly DiplomaDbContext _context;

        public ЗаказыRepository(DiplomaDbContext context)
        {
            _context = context;
        }

        public async Task<List<Заказы>> GetAktualnyeZakazyAsync()
        {
            return await _context.Заказыs
                .Where(o => o.Статус == "ожидает" || o.Статус == "приготовлен")
                .Include(o => o.Ресторан) 
                .ToListAsync();
        }


        public async Task<List<Заказы>> GetZakazyByCourierIdAsync(int courierId)
        {
            return await _context.Заказыs
        .Where(z => z.КурьерId == courierId)
        .Include(z => z.Ресторан) 
        .Include(z => z.Клиент)   
        .ToListAsync();
        }

        public async Task AssignCourierToOrderAsync(int orderId, int courierId)
        {
            var order = await _context.Заказыs.FirstOrDefaultAsync(o => o.Id == orderId);
            if (order != null && order.КурьерId == null)
            {
                order.КурьерId = courierId;
                await _context.SaveChangesAsync();
            }
        }
        public async Task<Пользователи> GetUserByIdAsync(int userId)
        {
            return await _context.Пользователиs.FindAsync(userId);
        }

        public async Task UpdateUserAsync(Пользователи user)
        {
            _context.Пользователиs.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Заказы>> GetOrdersByRestaurantAsync(int restaurantId)
        {
            var orders = await _context.Заказыs
                .Where(o => o.РесторанId == restaurantId && o.Статус != "отменен")
                .OrderByDescending(o => o.ДатаСоздания) 
                .ToListAsync();

            foreach (var order in orders)
            {
                if (order.Статус == "получен курьером" || IsOrderBeyondCourier(order.Статус))
                {
                    order.Статус = "ГОТОВ";
                }
            }

            return orders.OrderByDescending(o => (o.ДатаСоздания ?? DateTime.MinValue).Month)
                         .ThenBy(o => StatusPriority(o.Статус))
                         .ToList();

        }

        private bool IsOrderBeyondCourier(string status)
        {
            return status == "в процессе" || status == "доставлен" || status == "получен заказчиком";
        }

        private int StatusPriority(string status)
        {
            return status switch
            {
                "ожидает" => 1,
                "приготовлен" => 2,
                "получен курьером" => 3,
                "ГОТОВ" => 4,
                _ => 5
            };
        }

        public async Task<Заказы> GetByIdAsync(int orderId)
        {
            return await _context.Заказыs.FindAsync(orderId);
        }

        public async Task UpdateAsync(Заказы order)
        {
            _context.Заказыs.Update(order);
            await _context.SaveChangesAsync();
        }
    }
}
