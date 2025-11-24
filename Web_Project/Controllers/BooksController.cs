using Microsoft.AspNetCore.Mvc;
using Web_Project.Models;
using Web_Project.Models.Interfaces;
using Web_Project.Models.Repositories;

namespace Web_Project.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookRepository _bookRepo;
        private readonly ICategoryRepository _categoryRepo;

        public BooksController(IBookRepository bookRepo, ICategoryRepository categoryRepo)
        {
            _bookRepo = bookRepo;
            _categoryRepo = categoryRepo;
        }
        public class BookViewModel
        {
            public int BookID { get; set; }
            public string Title { get; set; }
            public string Author { get; set; }
            public decimal Price { get; set; }
            public string CoverImage { get; set; }
            public string Description { get; set; }
            public string CategoryName { get; set; }
        }

        public IActionResult Index(string category = "Fantasy")
        {
            var categoryObj = _categoryRepo.GetCategoryByName(category);
            if (categoryObj == null) return NotFound();

            var books = _bookRepo.GetBooksByCategoryId(categoryObj.CategoryID);

            var bookVMs = books.Select(b => new BookViewModel
            {
                BookID = b.BookID,
                Title = b.Title,
                Author = b.Author,
                Price = b.Price,
                CoverImage = b.CoverImage,
                CategoryName = categoryObj.CategoryName,
                Description = b.Description
            }).ToList();

            ViewData["CategoryName"] = categoryObj.CategoryName;
            return View("BookList", bookVMs);
        }

        public IActionResult Details(int id)
        {
            var book = _bookRepo.GetBookById(id);
            if (book == null) return NotFound();

            var category = _categoryRepo.GetCategoryById(book.CategoryID);

            var bookVM = new BookViewModel
            {
                BookID = book.BookID,
                Title = book.Title,
                Author = book.Author,
                Price = book.Price,
                CoverImage = book.CoverImage,
                Description = book.Description,
                CategoryName = category?.CategoryName
            };

            return View("BookDetails", bookVM);
        }
    }
}
