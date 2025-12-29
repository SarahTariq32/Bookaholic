using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using Web_Project.Models;
using Web_Project.Models.Interfaces;

namespace Web_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBookRepository _bookRepo;
        private readonly ICategoryRepository _categoryRepo;

        public HomeController(
            ILogger<HomeController> logger,
            IBookRepository bookRepo,
            ICategoryRepository categoryRepo)
        {
            _logger = logger;
            _bookRepo = bookRepo;
            _categoryRepo = categoryRepo;
        }

        public async Task<IActionResult> Index()
        {
            var allCategories = (await _categoryRepo.GetAllCategories()).ToList();
            var allBooks = (await _bookRepo.GetAllBooksAsync()).ToList();

            // FEATURED: 2 fiction + 2 non-fiction (adjust names/IDs if your DB differs)
            var featuredCategoryNames = new[] { "Fantasy", "Thriller", "Islamic", "Horror" };

            var booksByCategory = new Dictionary<string, List<Book>>(StringComparer.OrdinalIgnoreCase);
            var featuredIds = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            foreach (var catName in featuredCategoryNames)
            {
                var category = allCategories.FirstOrDefault(c => string.Equals(c.CategoryName, catName, StringComparison.OrdinalIgnoreCase));
                if (category == null)
                {
                    // keep key with empty list so view logic remains simple
                    booksByCategory[catName] = new List<Book>();
                    featuredIds[catName] = 0;
                    continue;
                }

                var booksForCategory = allBooks
                    .Where(b => b.CategoryID == category.CategoryID)
                    .Take(12)        // limit shown books; adjust as needed
                    .ToList();

                booksByCategory[catName] = booksForCategory;
                featuredIds[catName] = category.CategoryID;
            }

            // expose IDs if you prefer dynamic links instead of hardcoded asp-route-categoryId in view
            ViewBag.FeaturedCategoryIds = featuredIds;

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
        public async Task<IActionResult> Search(string q)
        {
            if (string.IsNullOrWhiteSpace(q))
            {
                return PartialView("_SearchResults", new List<Book>());
            }

            var allBooks = await _bookRepo.GetAllBooksAsync();

            var matched = allBooks
                .Where(b =>
                    (!string.IsNullOrEmpty(b.Title) && b.Title.Contains(q, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(b.Author) && b.Author.Contains(q, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(b.Description) && b.Description.Contains(q, StringComparison.OrdinalIgnoreCase))
                )
                .ToList();

            return PartialView("_SearchResults", matched);
        }

        public async Task<IActionResult> ContactUs()
        {           
            return View();
        }
    }

    public class HomePageViewModel
    {
        public Dictionary<string, List<Book>> BooksByCategory { get; set; }
    }
}

