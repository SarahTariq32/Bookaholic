using System;
using System.Collections.Generic;
using Web_Project.Models.Interfaces;
using Web_Project.Models;


namespace Web_Project.Services
{
    public class BookService: IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICategoryRepository _categoryRepository;

        public BookService(IBookRepository bookRepository, ICategoryRepository categoryRepository)
        {
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
        }

        public IEnumerable<Book> GetAllBooks()
        {
            return _bookRepository.GetAllBooks();
        }

        public Book GetBookById(int id)
        {
            return _bookRepository.GetBookById(id);
        }

        public void AddBook(Book book)
        {
            var category = _categoryRepository.GetCategoryById(book.CategoryID);
            if (category == null)
            {
                throw new Exception("Invalid category.");
            }
            _bookRepository.AddBook(book);
        }

        public void UpdateBook(Book book)
        {
            _bookRepository.UpdateBook(book);
        }

        public void DeleteBook(int id)
        {
            _bookRepository.DeleteBook(id);
        }

        public IEnumerable<Book> GetBooksByCategory(int categoryId)
        {
            return _bookRepository.GetBooksByCategory(categoryId);
        }

        public IEnumerable<Book> SearchBooks(string keyword)
        {
            return _bookRepository.SearchBooks(keyword);
        }
    }
}
