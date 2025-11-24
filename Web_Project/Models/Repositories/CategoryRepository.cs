using System.Collections.Generic;
using System.Linq;
using Web_Project.Models.Interfaces;

namespace Web_Project.Models.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private List<Category> _categories;

        public CategoryRepository()
        {
            _categories = new List<Category>
            {
                new Category { CategoryID = 1, CategoryName = "Fantasy" },
                new Category { CategoryID = 2, CategoryName = "Thriller" }
            };
        }

        public List<Category> GetAllCategories()
        {
            return _categories;
        }

        public Category GetCategoryById(int id)
        {
            return _categories.FirstOrDefault(c => c.CategoryID == id);
        }

        public Category GetCategoryByName(string name)
        {
            return _categories.FirstOrDefault(c => c.CategoryName.ToLower() == name.ToLower());
        }
    }
}
