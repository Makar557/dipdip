using Dip.Models;

namespace Dip.Repository
{
    public class РолиRepository : IRepository<Роли>
    {
        private readonly DiplomaDbContext _context;

        public РолиRepository(DiplomaDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Роли> ПолучитьВсе() => _context.Ролиs.ToList();
        public Роли ПолучитьПоId(int id) => _context.Ролиs.Find(id);

        public void Добавить(Роли роль)
        {
            _context.Ролиs.Add(роль);
            _context.SaveChanges();
        }

        public void Обновить(Роли роль)
        {
            _context.Ролиs.Update(роль);
            _context.SaveChanges();
        }

        public void Удалить(int id)
        {
            var роль = _context.Ролиs.Find(id);
            if (роль != null)
            {
                _context.Ролиs.Remove(роль);
                _context.SaveChanges();
            }
        }
    }

}
