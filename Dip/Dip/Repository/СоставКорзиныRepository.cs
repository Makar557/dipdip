using Dip.Models;

namespace Dip.Repository
{
    public class СоставКорзиныRepository : IRepository<СоставКорзины>
    {
        private readonly DiplomaDbContext _context;

        public СоставКорзиныRepository(DiplomaDbContext context)
        {
            _context = context;
        }

        public IEnumerable<СоставКорзины> ПолучитьВсе() => _context.СоставКорзиныs.ToList();
        public СоставКорзины ПолучитьПоId(int id) => _context.СоставКорзиныs.Find(id);

        public void Добавить(СоставКорзины элементКорзины)
        {
            _context.СоставКорзиныs.Add(элементКорзины);
            _context.SaveChanges();
        }

        public void Обновить(СоставКорзины элементКорзины)
        {
            _context.СоставКорзиныs.Update(элементКорзины);
            _context.SaveChanges();
        }

        public void Удалить(int id)
        {
            var элементКорзины = _context.СоставКорзиныs.Find(id);
            if (элементКорзины != null)
            {
                _context.СоставКорзиныs.Remove(элементКорзины);
                _context.SaveChanges();
            }
        }
    }

}
