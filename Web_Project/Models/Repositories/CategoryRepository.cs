//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Web_Project.Data;
//using Web_Project.Models;
//using Web_Project.Models.Interfaces;

//namespace Web_Project.Repository
//{
//    public class CategoryRepository : ICategoryRepository
//    {
//        private readonly ApplicationDbContext _context;

//        public CategoryRepository(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        public IEnumerable<Category> GetAllCategories()
//        {
//            try
//            {
//                return _context.Categories.ToList();
//            }
//            catch (Exception ex)
//            {
//                throw new Exception("Error fetching all categories", ex);
//            }
//        }

//        public Category GetCategoryById(int id)
//        {
//            try
//            {
//                return _context.Categories.FirstOrDefault(c => c.CategoryID == id);
//            }
//            catch (Exception ex)
//            {
//                throw new Exception($"Error fetching category with ID {id}", ex);
//            }
//        }

//        public Category GetCategoryByName(string name)
//        {
//            try
//            {
//                return _context.Categories.FirstOrDefault(c => c.CategoryName == name);
//            }
//            catch (Exception ex)
//            {
//                throw new Exception($"Error fetching category with Name {name}", ex);
//            }
//        }
//        public void AddCategory(Category category)
//        {
//            try
//            {
//                _context.Categories.Add(category);
//                _context.SaveChanges();
//            }
//            catch (Exception ex)
//            {
//                throw new Exception("Error adding new category", ex);
//            }
//        }

//        public void UpdateCategory(Category category)
//        {
//            try
//            {
//                _context.Categories.Update(category);
//                _context.SaveChanges();
//            }
//            catch (Exception ex)
//            {
//                throw new Exception($"Error updating category with ID {category.CategoryID}", ex);
//            }
//        }

//        public void DeleteCategory(int id)
//        {
//            try
//            {
//                var category = _context.Categories.FirstOrDefault(c => c.CategoryID == id);
//                if (category != null)
//                {
//                    _context.Categories.Remove(category);
//                    _context.SaveChanges();
//                }
//            }
//            catch (Exception ex)
//            {
//                throw new Exception($"Error deleting category with ID {id}", ex);
//            }
//        }
//    }
//}
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Web_Project.Models;
using Web_Project.Models.Interfaces;

namespace Web_Project.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<CategoryRepository> _logger;

        public CategoryRepository(IConfiguration config, ILogger<CategoryRepository> logger)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
            _logger = logger;
        }

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            const string sql = "SELECT * FROM Categories";
            try
            {
                using var conn = CreateConnection();
                return await conn.QueryAsync<Category>(sql);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching categories");
                throw;
            }
        }

        public async Task<Category> GetCategoryById(int id)
        {
            const string sql = "SELECT * FROM Categories WHERE CategoryID = @Id";
            try
            {
                using var conn = CreateConnection();
                return await conn.QueryFirstOrDefaultAsync<Category>(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching category {id}");
                throw;
            }
        }

        public async Task AddCategory(Category category)
        {
            const string sql = "INSERT INTO Categories (CategoryName) VALUES (@CategoryName)";
            try
            {
                using var conn = CreateConnection();
                await conn.ExecuteAsync(sql, new { category.CategoryName });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding category");
                throw;
            }
        }

        public async Task UpdateCategory(Category category)
        {
            const string sql = "UPDATE Categories SET CategoryName = @CategoryName WHERE CategoryID = @CategoryID";
            try
            {
                using var conn = CreateConnection();
                await conn.ExecuteAsync(sql, new { category.CategoryName, category.CategoryID });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating category {category.CategoryID}");
                throw;
            }
        }

        public async Task DeleteCategory(int id)
        {
            const string sql = "DELETE FROM Categories WHERE CategoryID = @Id";
            try
            {
                using var conn = CreateConnection();
                await conn.ExecuteAsync(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting category {id}");
                throw;
            }
        }

        public async Task<Category> GetCategoryByNameAsync(string name)
        {
            const string sql = "SELECT * FROM Categories WHERE CategoryName = @Name";
            using var conn = CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<Category>(sql, new { Name = name });
        }
    }
}