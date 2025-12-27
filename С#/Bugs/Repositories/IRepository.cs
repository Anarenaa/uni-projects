namespace Repositories
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Get();
        T? GetById(int id);
        void Add(T entity);
        void Delete(T entity);
        void SaveChanges();
    }
}
