using Core.Models;
using Repositories;
using static Repositories.CategoryRepository;

namespace Services
{
    public class CategoryService
    {
        private readonly CategoryRepository _categoryRepository;
        private readonly FeedbackCategoryRepository _fcRepository;
        public CategoryService(CategoryRepository categoryRepository, FeedbackCategoryRepository feedbackCategoryRepository)
        {
            _categoryRepository = categoryRepository;
            _fcRepository = feedbackCategoryRepository;
        }
        public List<Category> GetAllCategories()
        {
            return _categoryRepository.GetAll();
        }
        public Category GetCategoryById(int id)
        {
            var category = _categoryRepository.GetById(id);
            if (category == null)
                throw new Exception($"Category with ID {id} not found.");
            return category;
        }
        public List<CategoryStatDto> GetCategoryAnalytics()
        {
            return _categoryRepository.GetCategoryAnalytics();
        }
        private void validateCategory(Category category)
        {
            if (string.IsNullOrWhiteSpace(category.Name))
                throw new Exception("Category name cannot be empty.");
            if (category.Name.Length > 50)
                throw new Exception("Category name is too long.");
        }
        public void AddCategory(Category cat)
        {
            validateCategory(cat);
            var category = new Category
            {
                Name = cat.Name
            };
            _categoryRepository.Add(category);
            _categoryRepository.SaveChanges();
        }
        public void EditCategory(int id, string name)
        {
            var category = _categoryRepository.GetById(id);
            if (category == null)
                throw new Exception($"Category with ID {id} is not found");
            if(string.IsNullOrWhiteSpace(name))
                throw new Exception("Category name cannot be empty.");
            if(name.Length > 50)
                throw new Exception("Category name is too long.");

            category.Name = name;
            _categoryRepository.SaveChanges();
        }
        public void DeleteCategory(int id)
        {
            var category = _categoryRepository.GetById(id);
            if (category == null)
                throw new Exception($"Category with ID {id} is not found");
            _categoryRepository.Delete(category);
            _fcRepository.DeleteByCategoryId(id);

            _categoryRepository.SaveChanges();
            _fcRepository.SaveChanges();
        }
    }
}
