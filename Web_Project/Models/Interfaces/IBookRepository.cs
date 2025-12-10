//using System.Collections.Generic;
//namespace Web_Project.Models.Interfaces
//{
//    public interface IBookRepository
//    {
//        IEnumerable<Book> GetAllBooks();
//        Book GetBookById(int id);

//        void AddBook(Book book);
//        void UpdateBook(Book book);
//        void DeleteBook(int id);

//        IEnumerable<Book> GetBooksByCategory(int categoryId);
//        IEnumerable<Book> SearchBooks(string keyword);
//    }
//}
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Web_Project.Models.Interfaces
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<Book> GetBookByIdAsync(int id);

        Task AddBookAsync(Book book);
        Task UpdateBookAsync(Book book);
        Task DeleteBookAsync(int id);

        Task<IEnumerable<Book>> GetBooksByCategoryAsync(int categoryId);
        Task<IEnumerable<Book>> SearchBooksAsync(string keyword);
        Task<int> CountBooksAsync();
    }
}
