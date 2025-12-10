using System.Collections.Generic;
using System.Threading.Tasks;

namespace Web_Project.Models.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<Book> GetBookByIdAsync(int id);

        Task<bool> AddBookAsync(Book book);
        Task<bool> UpdateBookAsync(Book book);
        Task<bool> DeleteBookAsync(int id);
        Task<int> CountBooksAsync();
    }
}
