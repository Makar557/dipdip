namespace Dip.Repository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> ПолучитьВсе();
        T ПолучитьПоId(int id);
        void Добавить(T entity);
        void Обновить(T entity);
        void Удалить(int id);
    }
}
