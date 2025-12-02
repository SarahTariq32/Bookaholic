using System;
using System.Collections.Generic;
using System.Linq;
using Web_Project.Data;
using Web_Project.Models.Interfaces;
using Web_Project.Models;
using Microsoft.Extensions.Logging;

namespace Web_Project.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BookRepository> _logger;

        public BookRepository(ApplicationDbContext context, ILogger<BookRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<Book> GetAllBooks()
        {
            try
            {
                _logger.LogInformation("Fetching all books from the database.");
                return _context.Books.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all books");
                throw new Exception("Error fetching all books", ex);
            }
        }

        public Book GetBookById(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching book with ID {id}");
                return _context.Books.FirstOrDefault(b => b.BookID == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching book with ID {id}");
                throw new Exception($"Error fetching book with ID {id}", ex);
            }
        }

        public void AddBook(Book book)
        {
            try
            {
                _context.Books.Add(book);
                _context.SaveChanges();
                _logger.LogInformation($"Book '{book.Title}' added successfully with ID {book.BookID}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding new book");
                throw new Exception("Error adding new book", ex);
            }
        }

        public void UpdateBook(Book book)
        {
            try
            {
                _context.Books.Update(book);
                _context.SaveChanges();
                _logger.LogInformation($"Book '{book.Title}' updated successfully with ID {book.BookID}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating book with ID {book.BookID}");
                throw new Exception($"Error updating book with ID {book.BookID}", ex);
            }
        }

        public void DeleteBook(int id)
        {
            try
            {
                var book = _context.Books.FirstOrDefault(b => b.BookID == id);
                if (book != null)
                {
                    _context.Books.Remove(book);
                    _context.SaveChanges();
                    _logger.LogInformation($"Book '{book.Title}' deleted successfully with ID {id}");
                }
                else
                {
                    _logger.LogWarning($"Attempted to delete book with ID {id}, but it was not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting book with ID {id}");
                throw new Exception($"Error deleting book with ID {id}", ex);
            }
        }

        public IEnumerable<Book> GetBooksByCategory(int categoryId)
        {
            try
            {
                _logger.LogInformation($"Fetching books for category ID {categoryId}");
                return _context.Books
                    .Where(b => b.CategoryID == categoryId)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching books for category {categoryId}");
                throw new Exception($"Error fetching books for category {categoryId}", ex);
            }
        }

        public IEnumerable<Book> SearchBooks(string keyword)
        {
            try
            {
                _logger.LogInformation($"Searching books with keyword '{keyword}'");
                return _context.Books
                    .Where(b =>
                        b.Title.Contains(keyword) ||
                        b.Author.Contains(keyword) ||
                        b.Description.Contains(keyword))
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error searching books with keyword '{keyword}'");
                throw new Exception($"Error searching books with keyword '{keyword}'", ex);
            }
        }
    }
}
