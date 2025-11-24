using System.Collections.Generic;
namespace Web_Project.Models.Interfaces
{
    public interface IBookRepository
    {
        Book GetBookById(int id);
        List<Book> GetBooksByCategoryId(int categoryId);
        List<Book> GetAllBooks();
    }
}
