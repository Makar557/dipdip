using Dip.Models;

namespace Dip.Repository
{
    public class ОценкиRepository : IRepository<Оценки>
    {
        private readonly DiplomaDbContext _context;

        public ОценкиRepository(DiplomaDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Оценки> ПолучитьВсе() => _context.Оценкиs.ToList();
        public Оценки ПолучитьПоId(int id) => _context.Оценкиs.Find(id);

        public void Добавить(Оценки оценка)
        {
            _context.Оценкиs.Add(оценка);
            _context.SaveChanges();
        }

        public void Обновить(Оценки оценка)
        {
            _context.Оценкиs.Update(оценка);
            _context.SaveChanges();
        }

        public void Удалить(int id)
        {
            var оценка = _context.Оценкиs.Find(id);
            if (оценка != null)
            {
                _context.Оценкиs.Remove(оценка);
                _context.SaveChanges();
            }
        }
    }

}
