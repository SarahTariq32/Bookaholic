using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Web_Project.Models;
using Web_Project.Models.Interfaces;
using Web_Project.Models.Repositories;

namespace Web_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBookRepository _bookRepo;
        private readonly ICategoryRepository _categoryRepo;

        public HomeController(ILogger<HomeController> logger,
                              IBookRepository bookRepo,
                              ICategoryRepository categoryRepo)
        {
            _logger = logger;
            _bookRepo = bookRepo;
            _categoryRepo = categoryRepo;
        }

        public IActionResult Index()
        {
            var books = _bookRepo.GetAllBooks();
            var categories = _categoryRepo.GetAllCategories();
            var fantasyBooks = books
                .Where(b => categories.FirstOrDefault(c => c.CategoryID == b.CategoryID)?.CategoryName == "Fantasy")
                .ToList();

            var thrillerBooks = books
                .Where(b => categories.FirstOrDefault(c => c.CategoryID == b.CategoryID)?.CategoryName == "Thriller")
                .ToList();

            var islamicBooks = books
                .Where(b => categories.FirstOrDefault(c => c.CategoryID == b.CategoryID)?.CategoryName == "Islamic")
                .ToList();

            var horrorBooks = books
                .Where(b => categories.FirstOrDefault(c => c.CategoryID == b.CategoryID)?.CategoryName == "Horror")
                .ToList();

            var selfHelpBooks = books
                .Where(b => categories.FirstOrDefault(c => c.CategoryID == b.CategoryID)?.CategoryName == "Self Help")
                .ToList();

            var urduBooks = books
                .Where(b => categories.FirstOrDefault(c => c.CategoryID == b.CategoryID)?.CategoryName == "Urdu")
                .ToList();

            var vm = new HomePageViewModel
            {
                Categories = categories,
                FantasyBooks = fantasyBooks,
                ThrillerBooks = thrillerBooks,
                IslamicBooks = islamicBooks,
                HorrorBooks = horrorBooks,
                SelfHelpBooks = selfHelpBooks,
                UrduBooks = urduBooks
            };

            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }

    public class HomePageViewModel
    {
        public List<Category> Categories { get; set; }
        public List<Book> FantasyBooks { get; set; }
        public List<Book> ThrillerBooks { get; set; }
        public List<Book> IslamicBooks { get; set; }
        public List<Book> HorrorBooks { get; set; }
        public List<Book> SelfHelpBooks { get; set; }
        public List<Book> UrduBooks { get; set; }
    }
}
