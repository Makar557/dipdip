using Dip.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Dip.Repository
{
    public class КорзинаRepository
    {
        private readonly DiplomaDbContext _context;

        public КорзинаRepository(DiplomaDbContext context)
        {
            _context = context;
        }

        public Корзины GetOrCreateCart(int userId)
        {
            var cart = _context.Корзиныs
                .Include(c => c.СоставКорзиныs)
                .FirstOrDefault(c => c.ПользовательId == userId && c.Статус == "активная");

            if (cart == null)
            {
                cart = new Корзины
                {
                    ПользовательId = userId,
                    Статус = "активная",
                    ДатаСоздания = DateTime.Now
                };
                _context.Корзиныs.Add(cart);
                _context.SaveChanges();
            }
            return cart;
        }

        public void AddToCart(int userId, int dishId)
        {
            var cart = GetOrCreateCart(userId);
            var dish = _context.Менюs.Find(dishId);
            if (dish == null) throw new Exception("Блюдо не найдено");

            var скидка = _context.Скидкиs
                .Where(s => s.МенюId == dishId
                         && s.ДатаНачала <= DateOnly.FromDateTime(DateTime.UtcNow)
                         && s.ДатаОкончания >= DateOnly.FromDateTime(DateTime.UtcNow))
                .FirstOrDefault();

            decimal ценаСоСкидкой = скидка != null
                ? dish.Цена * (1 - (скидка.ПроцентСкидки / 100))
                : dish.Цена;

            var cartItem = cart.СоставКорзиныs.FirstOrDefault(i => i.БлюдоId == dishId);

            if (cartItem != null)
            {
                cartItem.Количество++;
                cartItem.Цена += ценаСоСкидкой; 
            }
            else
            {
                _context.СоставКорзиныs.Add(new СоставКорзины
                {
                    КорзинаId = cart.Id,
                    БлюдоId = dishId,
                    Количество = 1,
                    Цена = ценаСоСкидкой 
                });
            }

            cart.ИтоговаяСтоимость = cart.СоставКорзиныs.Sum(i => i.Цена);
            _context.SaveChanges();
        }
        public void UpdateCartPrices(int userId)
        {
            var cart = _context.Корзиныs
                .Include(c => c.СоставКорзиныs)
                .FirstOrDefault(c => c.ПользовательId == userId && c.Статус == "активная");

            if (cart == null) return;

            foreach (var item in cart.СоставКорзиныs)
            {
                var dish = _context.Менюs.Find(item.БлюдоId);
                if (dish == null) continue;

                var скидка = _context.Скидкиs
                    .Where(s => s.МенюId == dish.Id
                             && s.ДатаНачала <= DateOnly.FromDateTime(DateTime.UtcNow)
                             && s.ДатаОкончания >= DateOnly.FromDateTime(DateTime.UtcNow))
                    .FirstOrDefault();

                decimal новаяЦена = скидка != null
                    ? dish.Цена * (1 - (скидка.ПроцентСкидки / 100))
                    : dish.Цена;

                item.Цена = новаяЦена * item.Количество; 
            }

            cart.ИтоговаяСтоимость = cart.СоставКорзиныs.Sum(i => i.Цена);
            _context.SaveChanges();
        }

    }
}
