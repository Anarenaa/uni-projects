using Core.Context;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DataContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository()
        {
            _context = new DataContext();
            _dbSet = _context.Set<T>();
        }
        public IQueryable<T> Get() => _dbSet;
        public T? GetById(int id) => _dbSet.Find(id);
        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }
        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
