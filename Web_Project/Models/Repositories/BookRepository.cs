using System.Collections.Generic;
using System.Linq;
using Web_Project.Models.Interfaces;

namespace Web_Project.Models.Repositories
{
    public class BookRepository : IBookRepository
    {
        private List<Book> _books;

        public BookRepository()
        {
            _books = new List<Book>
            {
                new Book { BookID = 1, Title="The Cruel Prince", Author="H. Author", CategoryID=1, Price=1450 },
                new Book { BookID = 2, Title="Powerless", Author="A. Author", CategoryID=1, Price=1200 },
                new Book { BookID = 3, Title="Ignite Me", Author="S. Author", CategoryID=1, Price=1350 },
                new Book { BookID = 4, Title="Once Upon a Broken Heart", Author="C. Author", CategoryID=1, Price=1500 },
                new Book { BookID = 5, Title="Heart of the Raven Prince", Author="M. Author", CategoryID=1, Price=1400 },
                new Book { BookID = 6, Title="The Housemaid", Author="T. Author", CategoryID=2, Price=1350 },
                new Book { BookID = 7, Title="Silent Patient", Author="A. Smith", CategoryID=2, Price=1400 },
                new Book { BookID = 8, Title="The Teacher", Author="B. Brown", CategoryID=2, Price=1250 },
                new Book { BookID = 9, Title="The Locked Door", Author="L. James", CategoryID=2, Price=1300 },
                new Book { BookID = 10, Title="Not Quite Dead Yet", Author="R. Taylor", CategoryID=2, Price=1300 }
            };
        }

        public List<Book> GetAllBooks()
        {
            return _books;
        }

        public Book GetBookById(int id)
        {
            return _books.FirstOrDefault(b => b.BookID == id);
        }

        public List<Book> GetBooksByCategoryId(int categoryId)
        {
            return _books.Where(b => b.CategoryID == categoryId).ToList();
        }
    }
}
