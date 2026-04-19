using Core.Context;
using Core.Models;

namespace Repositories
{
    public class CategoryRepository : Repository<Category>
    {
        public CategoryRepository(DataContext context) : base(context) { }
        public List<CategoryStatDto> GetCategoryAnalytics()
        {
            return _dbSet
                .Select(c => new CategoryStatDto
                {
                    Name = c.Name,
                    TotalCount = c.FeedbackCategories.Count(fc => fc.Feedback.IsAnalyzed),
                    AverageRating = c.FeedbackCategories.Where(fc => fc.Feedback.IsAnalyzed)
                                     .Any() ? c.FeedbackCategories.Where(fc => fc.Feedback.IsAnalyzed)
                                     .Average(fc => fc.Feedback.Rate) : 0
                }).ToList();
        }

        public class CategoryStatDto
        {
            public required string Name { get; set; }
            public int TotalCount { get; set; }
            public double AverageRating { get; set; }
        }
    }
}
