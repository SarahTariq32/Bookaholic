using System.Collections.Generic;
using Web_Project.Models;

namespace Web_Project.Models.Interfaces
{
    public interface IBookService
    {
        IEnumerable<Book> GetAllBooks();
        Book GetBookById(int id);
        void AddBook(Book book);
        void UpdateBook(Book book);
        void DeleteBook(int id);
        IEnumerable<Book> GetBooksByCategory(int categoryId);
        IEnumerable<Book> SearchBooks(string keyword);
    }
}
