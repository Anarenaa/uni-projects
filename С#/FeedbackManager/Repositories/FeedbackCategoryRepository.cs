using Core.Context;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class FeedbackCategoryRepository
    {
        private readonly DataContext _context;
        private readonly DbSet<FeedbackCategory> _dbSet;
        public FeedbackCategoryRepository(DataContext context)
        {
            _context = context;
            _dbSet = _context.Set<FeedbackCategory>();
        }
        public List<FeedbackCategory> GetByFeedbackId(int feedbackId)
        {
            return _dbSet.Where(fc => fc.FeedbackId == feedbackId)
                .Include(fc => fc.Category)
                .ToList();
        }
        public List<FeedbackCategory> GetByCategoryId(int categoryId)
        {
            return _dbSet.Where(fc => fc.CategoryId == categoryId)
                .Include(fc => fc.Feedback)
                .ToList();
        }
        public void Add(FeedbackCategory entity)
        {
            _dbSet.Add(entity);
        }
        public void Delete(FeedbackCategory entity)
        {
            _dbSet.Remove(entity);
        }
        public void DeleteByCategoryId(int categoryId)
        {
            var entities = _dbSet.Where(fc => fc.CategoryId == categoryId).ToList();
            _dbSet.RemoveRange(entities);
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
