//using System;
//using System.Collections.Generic;
//using Web_Project.Models;
//using Web_Project.Models.Interfaces;

//namespace Web_Project.Services
//{
//    public class CategoryService : ICategoryService
//    {
//        private readonly ICategoryRepository _categoryRepository;

//        public CategoryService(ICategoryRepository categoryRepository)
//        {
//            _categoryRepository = categoryRepository;
//        }

//        public IEnumerable<Category> GetAllCategories()
//        {
//            return _categoryRepository.GetAllCategories();
//        }

//        public Category GetCategoryById(int id)
//        {
//            return _categoryRepository.GetCategoryById(id);
//        }

//        public void AddCategory(Category category)
//        {
//            _categoryRepository.AddCategory(category);
//        }

//        public void UpdateCategory(Category category)
//        {
//            _categoryRepository.UpdateCategory(category);
//        }

//        public void DeleteCategory(int id)
//        {
//            _categoryRepository.DeleteCategory(id);
//        }
//    }
//}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web_Project.Models;
using Web_Project.Models.Interfaces;

namespace Web_Project.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            return await _categoryRepository.GetAllCategories();
        }

        public async Task<Category> GetCategoryById(int id)
        {
            return await _categoryRepository.GetCategoryById(id);
        }

        public async Task AddCategory(Category category)
        {
            try
            {
                await _categoryRepository.AddCategory(category);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding category: " + ex.Message);
            }
        }

        public async Task UpdateCategory(Category category)
        {
            try
            {
                await _categoryRepository.UpdateCategory(category);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating category: " + ex.Message);
            }
        }

        public async Task DeleteCategory(int id)
        {
            try
            {
                await _categoryRepository.DeleteCategory(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting category: " + ex.Message);
            }
        }
    }
}
