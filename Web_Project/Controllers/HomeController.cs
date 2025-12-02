//using Microsoft.AspNetCore.Mvc;
//using System.Diagnostics;
//using Web_Project.Models;
//using Web_Project.Models.Interfaces;

//namespace Web_Project.Controllers
//{
//    public class HomeController : Controller
//    {
//        private readonly ILogger<HomeController> _logger;
//        private readonly IBookRepository _bookRepo;
//        private readonly ICategoryRepository _categoryRepo;

//        public HomeController(ILogger<HomeController> logger,
//                              IBookRepository bookRepo,
//                              ICategoryRepository categoryRepo)
//        {
//            _logger = logger;
//            _bookRepo = bookRepo;
//            _categoryRepo = categoryRepo;
//        }

//        public IActionResult Index()
//        {
//            var books = _bookRepo.GetAllBooks();
//            var categories = _categoryRepo.GetAllCategories();
//            var fantasyBooks = books
//                .Where(b => categories.FirstOrDefault(c => c.CategoryID == b.CategoryID)?.CategoryName == "Fantasy")
//                .ToList();

//            var thrillerBooks = books
//                .Where(b => categories.FirstOrDefault(c => c.CategoryID == b.CategoryID)?.CategoryName == "Thriller")
//                .ToList();

//            var islamicBooks = books
//                .Where(b => categories.FirstOrDefault(c => c.CategoryID == b.CategoryID)?.CategoryName == "Islamic")
//                .ToList();

//            var horrorBooks = books
//                .Where(b => categories.FirstOrDefault(c => c.CategoryID == b.CategoryID)?.CategoryName == "Horror")
//                .ToList();

//            var selfHelpBooks = books
//                .Where(b => categories.FirstOrDefault(c => c.CategoryID == b.CategoryID)?.CategoryName == "Self Help")
//                .ToList();

//            var urduBooks = books
//                .Where(b => categories.FirstOrDefault(c => c.CategoryID == b.CategoryID)?.CategoryName == "Urdu")
//                .ToList();

//            var vm = new HomePageViewModel
//            {
//                Categories = categories,
//                FantasyBooks = fantasyBooks,
//                ThrillerBooks = thrillerBooks,
//                IslamicBooks = islamicBooks,
//                HorrorBooks = horrorBooks,
//                SelfHelpBooks = selfHelpBooks,
//                UrduBooks = urduBooks
//            };

//            return View(vm);
//        }

//        public IActionResult Privacy()
//        {
//            return View();
//        }

//        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
//        public IActionResult Error()
//        {
//            return View(new ErrorViewModel
//            {
//                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
//            });
//        }
//    }

//    public class HomePageViewModel
//    {
//        public List<Category> Categories { get; set; }
//        public List<Book> FantasyBooks { get; set; }
//        public List<Book> ThrillerBooks { get; set; }
//        public List<Book> IslamicBooks { get; set; }
//        public List<Book> HorrorBooks { get; set; }
//        public List<Book> SelfHelpBooks { get; set; }
//        public List<Book> UrduBooks { get; set; }
//    }
//}
//using Microsoft.AspNetCore.Mvc;
//using System.Diagnostics;
//using Web_Project.Models;
//using Web_Project.Models.Interfaces;

//namespace Web_Project.Controllers
//{
//    public class HomeController : Controller
//    {
//        private readonly ILogger<HomeController> _logger;
//        private readonly IBookRepository _bookRepo;
//        private readonly ICategoryRepository _categoryRepo;

//        public HomeController(ILogger<HomeController> logger,
//                              IBookRepository bookRepo,
//                              ICategoryRepository categoryRepo)
//        {
//            _logger = logger;
//            _bookRepo = bookRepo;
//            _categoryRepo = categoryRepo;
//        }

//        //public IActionResult Index()
//        //{
//        //    var categories = _categoryRepo.GetAllCategories();
//        //    var books = _bookRepo.GetAllBooks();

//        //    var vm = new HomePageViewModel
//        //    {
//        //        Categories = (List<Category>)categories,
//        //        BooksByCategory = categories.ToDictionary(
//        //            cat => cat.CategoryName,
//        //            cat => books.Where(b => b.CategoryID == cat.CategoryID).ToList()
//        //        )
//        //    };

//        //    return View(vm);
//        //}
//        public IActionResult Index()
//        {
//            var allCategories = _categoryRepo.GetAllCategories().ToList();
//            var allBooks = _bookRepo.GetAllBooks().ToList();
//            List<string> featuredCategoryNames = new List<string>
//            {
//                "Fantasy",
//                "Thriller",
//                "Islamic",
//                "Romance",
//                "Horror",
//                "Children"
//            };

//            Dictionary<string, List<Book>> booksByCategory = new Dictionary<string, List<Book>>();
//            foreach (var catName in featuredCategoryNames)
//            {
//                var matchedCategory = allCategories.FirstOrDefault(c => c.CategoryName == catName);

//                if (matchedCategory != null)
//                {
//                    List<Book> booksForThisCategory = new List<Book>();

//                    foreach (var book in allBooks)
//                    {
//                        if (book.CategoryID == matchedCategory.CategoryID)
//                        {
//                            booksForThisCategory.Add(book);
//                        }
//                    }

//                    booksForThisCategory = booksForThisCategory.Take(10).ToList();
//                    booksByCategory.Add(catName, booksForThisCategory);
//                }
//            }

//            var vm = new HomePageViewModel
//            {
//                Categories = allCategories,        
//                BooksByCategory = booksByCategory  
//            };

//            return View(vm);
//        }

//        public IActionResult Privacy()
//        {
//            return View();
//        }

//        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
//        public IActionResult Error()
//        {
//            return View(new ErrorViewModel
//            {
//                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
//            });
//        }
//    }

//    public class HomePageViewModel
//    {
//        public List<Category> Categories { get; set; }
//        public Dictionary<string, List<Book>> BooksByCategory { get; set; }
//    }
//}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Web_Project.Models;
using Web_Project.Models.Interfaces;

namespace Web_Project.Controllers
{
    [Authorize]

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
            var allCategories = _categoryRepo.GetAllCategories().ToList();
            var allBooks = _bookRepo.GetAllBooks().ToList();
            List<string> featuredCategoryNames = new List<string>
            {
                "Fantasy",
                "Thriller",
                "Islamic",
                "Romance",
                "Horror",
                "Children"
            };

            Dictionary<string, List<Book>> booksByCategory = new Dictionary<string, List<Book>>();
            foreach (var catName in featuredCategoryNames)
            {
                var category = allCategories.FirstOrDefault(c => c.CategoryName == catName);
                if (category != null)
                {
                    var booksForCategory = allBooks
                        .Where(b => b.CategoryID == category.CategoryID)
                        .Take(10)
                        .ToList();

                    booksByCategory.Add(catName, booksForCategory);
                }
            }

            var vm = new HomePageViewModel
            {
                BooksByCategory = booksByCategory
            };

            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }
  
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
        [HttpGet]
        public IActionResult Search(string q)
        {
            if (string.IsNullOrWhiteSpace(q))
            {
                return PartialView("_SearchResults", new List<Web_Project.Models.Book>());
            }
            var allBooks = _bookRepo.GetAllBooks();
            var matched = allBooks
                .Where(b => (!string.IsNullOrEmpty(b.Title) && b.Title.Contains(q, StringComparison.OrdinalIgnoreCase))
                         || (!string.IsNullOrEmpty(b.Author) && b.Author.Contains(q, StringComparison.OrdinalIgnoreCase))
                         || (!string.IsNullOrEmpty(b.Description) && b.Description.Contains(q, StringComparison.OrdinalIgnoreCase)))
                .ToList();

            return PartialView("_SearchResults", matched);
        }

    }

    public class HomePageViewModel
    {
        public Dictionary<string, List<Book>> BooksByCategory { get; set; }
    }
}

