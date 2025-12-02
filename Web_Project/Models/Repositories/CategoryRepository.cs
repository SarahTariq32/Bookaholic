using System;
using System.Collections.Generic;
using System.Linq;
using Web_Project.Data;
using Web_Project.Models;
using Web_Project.Models.Interfaces;

namespace Web_Project.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Category> GetAllCategories()
        {
            try
            {
                return _context.Categories.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching all categories", ex);
            }
        }

        public Category GetCategoryById(int id)
        {
            try
            {
                return _context.Categories.FirstOrDefault(c => c.CategoryID == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching category with ID {id}", ex);
            }
        }

        public Category GetCategoryByName(string name)
        {
            try
            {
                return _context.Categories.FirstOrDefault(c => c.CategoryName == name);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching category with Name {name}", ex);
            }
        }
        public void AddCategory(Category category)
        {
            try
            {
                _context.Categories.Add(category);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding new category", ex);
            }
        }

        public void UpdateCategory(Category category)
        {
            try
            {
                _context.Categories.Update(category);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating category with ID {category.CategoryID}", ex);
            }
        }

        public void DeleteCategory(int id)
        {
            try
            {
                var category = _context.Categories.FirstOrDefault(c => c.CategoryID == id);
                if (category != null)
                {
                    _context.Categories.Remove(category);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting category with ID {id}", ex);
            }
        }
    }
}
