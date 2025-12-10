//using System.Collections.Generic;
//using Web_Project.Models;

//namespace Web_Project.Models.Interfaces
//{
//    public interface ICategoryService
//    {
//        IEnumerable<Category> GetAllCategories();
//        Category GetCategoryById(int id);
//        void AddCategory(Category category);
//        void UpdateCategory(Category category);
//        void DeleteCategory(int id);
//    }
//}

using System.Collections.Generic;
using System.Threading.Tasks;
using Web_Project.Models;

namespace Web_Project.Models.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategories();
        Task<Category> GetCategoryById(int id);
        Task AddCategory(Category category);
        Task UpdateCategory(Category category);
        Task DeleteCategory(int id);
    }
}
