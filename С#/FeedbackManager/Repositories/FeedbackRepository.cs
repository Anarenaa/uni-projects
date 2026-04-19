using Core.Context;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class FeedbackRepository : Repository<Feedback>
    {
        public FeedbackRepository(DataContext context) : base(context) { }

        public List<Feedback> GetAllFiltered(bool isAnalyzed, IEnumerable<int>? categoryIds = null, string? searchText = null)
        {
            var query = _context.Feedbacks
                .Include(f => f.FeedbackCategories)
                    .ThenInclude(fc => fc.Category)
                .AsQueryable();

            query = query.Where(f => f.IsAnalyzed == isAnalyzed);

            // Filter by category IDs if provided
            if (categoryIds != null && categoryIds.Any())
            {
                foreach (var id in categoryIds)
                {
                    query = query.Where(f => f.FeedbackCategories.Any(fc => fc.CategoryId == id));
                }
            }

            // Filter by search text if provided
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                string lowerText = searchText.ToLower();
                query = query.Where(f => f.Comment.ToLower().Contains(lowerText));
            }

            return query.ToList();
        }
    }
}
