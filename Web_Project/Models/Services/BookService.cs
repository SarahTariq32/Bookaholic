//using System;
//using System.Collections.Generic;
//using Web_Project.Models.Interfaces;
//using Web_Project.Models;


//namespace Web_Project.Services
//{
//    public class BookService: IBookService
//    {
//        private readonly IBookRepository _bookRepository;
//        private readonly ICategoryRepository _categoryRepository;

//        public BookService(IBookRepository bookRepository, ICategoryRepository categoryRepository)
//        {
//            _bookRepository = bookRepository;
//            _categoryRepository = categoryRepository;
//        }

//        public IEnumerable<Book> GetAllBooks()
//        {
//            return _bookRepository.GetAllBooks();
//        }

//        public Book GetBookById(int id)
//        {
//            return _bookRepository.GetBookById(id);
//        }

//        public void AddBook(Book book)
//        {
//            var category = _categoryRepository.GetCategoryById(book.CategoryID);
//            if (category == null)
//            {
//                throw new Exception("Invalid category.");
//            }
//            _bookRepository.AddBook(book);
//        }

//        public void UpdateBook(Book book)
//        {
//            _bookRepository.UpdateBook(book);
//        }

//        public void DeleteBook(int id)
//        {
//            _bookRepository.DeleteBook(id);
//        }

//        public IEnumerable<Book> GetBooksByCategory(int categoryId)
//        {
//            return _bookRepository.GetBooksByCategory(categoryId);
//        }

//        public IEnumerable<Book> SearchBooks(string keyword)
//        {
//            return _bookRepository.SearchBooks(keyword);
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
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            try
            {
                return await _bookRepository.GetAllBooksAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching all books: " + ex.Message);
                return Array.Empty<Book>();
            }
        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            try
            {
                return await _bookRepository.GetBookByIdAsync(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching book: " + ex.Message);
                return null;
            }
        }

        public async Task<bool> AddBookAsync(Book book)
        {
            try
            {
                await _bookRepository.AddBookAsync(book);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding book: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> UpdateBookAsync(Book book)
        {
            try
            {
                await _bookRepository.UpdateBookAsync(book);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating book: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            try
            {
                await _bookRepository.DeleteBookAsync(id);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting book: " + ex.Message);
                return false;
            }
        }
        public Task<int> CountBooksAsync() => _bookRepository.CountBooksAsync();
    }
}
