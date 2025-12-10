//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Web_Project.Data;
//using Web_Project.Models.Interfaces;
//using Web_Project.Models;
//using Microsoft.Extensions.Logging;

//namespace Web_Project.Repository
//{
//    public class BookRepository : IBookRepository
//    {
//        private readonly ApplicationDbContext _context;
//        private readonly ILogger<BookRepository> _logger;

//        public BookRepository(ApplicationDbContext context, ILogger<BookRepository> logger)
//        {
//            _context = context;
//            _logger = logger;
//        }

//        public IEnumerable<Book> GetAllBooks()
//        {
//            try
//            {
//                _logger.LogInformation("Fetching all books from the database.");
//                return _context.Books.ToList();
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error fetching all books");
//                throw new Exception("Error fetching all books", ex);
//            }
//        }

//        public Book GetBookById(int id)
//        {
//            try
//            {
//                _logger.LogInformation($"Fetching book with ID {id}");
//                return _context.Books.FirstOrDefault(b => b.BookID == id);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, $"Error fetching book with ID {id}");
//                throw new Exception($"Error fetching book with ID {id}", ex);
//            }
//        }

//        public void AddBook(Book book)
//        {
//            try
//            {
//                _context.Books.Add(book);
//                _context.SaveChanges();
//                _logger.LogInformation($"Book '{book.Title}' added successfully with ID {book.BookID}");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error adding new book");
//                throw new Exception("Error adding new book", ex);
//            }
//        }

//        public void UpdateBook(Book book)
//        {
//            try
//            {
//                _context.Books.Update(book);
//                _context.SaveChanges();
//                _logger.LogInformation($"Book '{book.Title}' updated successfully with ID {book.BookID}");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, $"Error updating book with ID {book.BookID}");
//                throw new Exception($"Error updating book with ID {book.BookID}", ex);
//            }
//        }

//        public void DeleteBook(int id)
//        {
//            try
//            {
//                var book = _context.Books.FirstOrDefault(b => b.BookID == id);
//                if (book != null)
//                {
//                    _context.Books.Remove(book);
//                    _context.SaveChanges();
//                    _logger.LogInformation($"Book '{book.Title}' deleted successfully with ID {id}");
//                }
//                else
//                {
//                    _logger.LogWarning($"Attempted to delete book with ID {id}, but it was not found.");
//                }
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, $"Error deleting book with ID {id}");
//                throw new Exception($"Error deleting book with ID {id}", ex);
//            }
//        }

//        public IEnumerable<Book> GetBooksByCategory(int categoryId)
//        {
//            try
//            {
//                _logger.LogInformation($"Fetching books for category ID {categoryId}");
//                return _context.Books
//                    .Where(b => b.CategoryID == categoryId)
//                    .ToList();
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, $"Error fetching books for category {categoryId}");
//                throw new Exception($"Error fetching books for category {categoryId}", ex);
//            }
//        }

//        public IEnumerable<Book> SearchBooks(string keyword)
//        {
//            try
//            {
//                _logger.LogInformation($"Searching books with keyword '{keyword}'");
//                return _context.Books
//                    .Where(b =>
//                        b.Title.Contains(keyword) ||
//                        b.Author.Contains(keyword) ||
//                        b.Description.Contains(keyword))
//                    .ToList();
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, $"Error searching books with keyword '{keyword}'");
//                throw new Exception($"Error searching books with keyword '{keyword}'", ex);
//            }
//        }
//    }
//}
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Web_Project.Models;
using Web_Project.Models.Interfaces;

namespace Web_Project.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<BookRepository> _logger;

        public BookRepository(IConfiguration config, ILogger<BookRepository> logger)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
            _logger = logger;
        }

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            const string sql = "SELECT * FROM Books";
            try
            {
                using var conn = CreateConnection();
                return await conn.QueryAsync<Book>(sql);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all books");
                throw;
            }
        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            const string sql = "SELECT * FROM Books WHERE BookID = @Id";
            try
            {
                using var conn = CreateConnection();
                return await conn.QueryFirstOrDefaultAsync<Book>(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching book with ID {id}");
                throw;
            }
        }

        public async Task AddBookAsync(Book book)
        {
            const string sql = @"
                INSERT INTO Books (Title, Author, CategoryID, Price, StockQuantity, Description, CoverImage)
                VALUES (@Title, @Author, @CategoryID, @Price, @StockQuantity, @Description, @CoverImage);
            ";
            try
            {
                using var conn = CreateConnection();
                await conn.ExecuteAsync(sql, book);
                _logger.LogInformation("Book added: {Title}", book.Title);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding book");
                throw;
            }
        }

        public async Task UpdateBookAsync(Book book)
        {
            const string sql = @"
                UPDATE Books SET
                    Title = @Title,
                    Author = @Author,
                    CategoryID = @CategoryID,
                    Price = @Price,
                    StockQuantity = @StockQuantity,
                    Description = @Description,
                    CoverImage = @CoverImage
                WHERE BookID = @BookID;
            ";
            try
            {
                using var conn = CreateConnection();
                await conn.ExecuteAsync(sql, book);
                _logger.LogInformation("Book updated: {Id}", book.BookID);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating book {book.BookID}");
                throw;
            }
        }

        public async Task DeleteBookAsync(int id)
        {
            const string sql = "DELETE FROM Books WHERE BookID = @Id";
            try
            {
                using var conn = CreateConnection();
                await conn.ExecuteAsync(sql, new { Id = id });
                _logger.LogInformation("Book deleted: {Id}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting book {id}");
                throw;
            }
        }

        public async Task<IEnumerable<Book>> GetBooksByCategoryAsync(int categoryId)
        {
            const string sql = "SELECT * FROM Books WHERE CategoryID = @CategoryID";
            try
            {
                using var conn = CreateConnection();
                return await conn.QueryAsync<Book>(sql, new { CategoryID = categoryId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching books by category {categoryId}");
                throw;
            }
        }

        public async Task<IEnumerable<Book>> SearchBooksAsync(string keyword)
        {
            const string sql = @"
                SELECT * FROM Books
                WHERE Title LIKE '%' + @Key + '%'
                   OR Author LIKE '%' + @Key + '%'
                   OR Description LIKE '%' + @Key + '%';
            ";
            try
            {
                using var conn = CreateConnection();
                return await conn.QueryAsync<Book>(sql, new { Key = keyword });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error searching books with '{keyword}'");
                throw;
            }
        }

        public async Task<int> CountBooksAsync()
        {
            const string sql = "SELECT COUNT(1) FROM Books";
            using var conn = CreateConnection();
            return await conn.ExecuteScalarAsync<int>(sql);
        }
    }
}
