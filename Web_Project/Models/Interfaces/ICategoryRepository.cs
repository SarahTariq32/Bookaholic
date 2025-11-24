using System.Collections.Generic;
namespace Web_Project.Models.Interfaces
{
    public interface ICategoryRepository
    {
        Category GetCategoryById(int id);
        Category GetCategoryByName(string name);
        List<Category> GetAllCategories();
    }
}
