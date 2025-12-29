//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Linq;
//using System.Threading.Tasks;
//using Web_Project.Models;
//using Web_Project.Models.Interfaces;
//using Web_Project.Models.Interfaces;

//namespace Web_Project.Controllers
//{
//    public class BooksController : Controller
//    {
//        private readonly IBookRepository _bookRepo;
//        private readonly ICategoryRepository _categoryRepo;
//        private readonly ILogger<BooksController> _logger;

//        public BooksController(
//            IBookRepository bookRepo,
//            ICategoryRepository categoryRepo,
//            ILogger<BooksController> logger)
//        {
//            _bookRepo = bookRepo;
//            _categoryRepo = categoryRepo;
//            _logger = logger;
//        }

//        public class BookViewModel
//        {
//            public int BookID { get; set; }
//            public string Title { get; set; } = string.Empty;
//            public string Author { get; set; } = string.Empty;
//            public decimal Price { get; set; }
//            public string CoverImage { get; set; } = string.Empty;
//            public string Description { get; set; } = string.Empty;
//            public string CategoryName { get; set; } = string.Empty;
//        }

//        // Keep Index minimal and forward to BookList to make routing explicit from the views
//        public IActionResult Index(string? category = null, int? categoryId = null)
//        {
//            // Forward any category/categoryId received to BookList so it shows the expected category.
//            return RedirectToAction(nameof(BookList), new { category, categoryId });
//        }


//        public async Task<IActionResult> BookList(string? category = null, int? categoryId = null, int page = 1)
//        {
//            const int pageSize = 20;
//            try
//            {
//                if (categoryId.HasValue)
//                {
//                    var catById = await _categoryRepo.GetCategoryById(categoryId.Value);
//                    if (catById == null)
//                    {
//                        _logger.LogWarning("Category with ID {CategoryId} not found.", categoryId);
//                        return NotFound();
//                    }
//                    category = catById.CategoryName;
//                }

//                // normalize category for routing and display
//                var routeCategory = string.IsNullOrWhiteSpace(category) ? "Fantasy" : category;

//                IEnumerable<Book> allSource;
//                string displayCategoryName;

//                if (string.Equals(routeCategory, "All", StringComparison.OrdinalIgnoreCase))
//                {
//                    allSource = await _bookRepo.GetAllBooksAsync();
//                    displayCategoryName = "All Books";
//                }
//                else
//                {
//                    var categoryObj = await _categoryRepo.GetCategoryByNameAsync(routeCategory);
//                    if (categoryObj == null)
//                    {
//                        _logger.LogWarning("Category '{Category}' not found.", routeCategory);
//                        return NotFound();
//                    }
//                    allSource = await _bookRepo.GetBooksByCategoryAsync(categoryObj.CategoryID);
//                    displayCategoryName = categoryObj.CategoryName;
//                }

//                var itemList = allSource.ToList();
//                var totalItems = itemList.Count;
//                var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
//                if (totalPages == 0) totalPages = 1;

//                if (page < 1) page = 1;
//                if (page > totalPages) page = totalPages;

//                var pageItems = itemList
//                    .Skip((page - 1) * pageSize)
//                    .Take(pageSize)
//                    .ToList();

//                var vmList = new List<BookViewModel>(capacity: pageItems.Count);
//                // avoid per-item async calls inside LINQ; fetch category names with dictionary if you want perf.
//                foreach (var b in pageItems)
//                {
//                    var cat = await _categoryRepo.GetCategoryById(b.CategoryID);
//                    vmList.Add(new BookViewModel
//                    {
//                        BookID = b.BookID,
//                        Title = b.Title,
//                        Author = b.Author,
//                        Price = b.Price,
//                        CoverImage = b.CoverImage,
//                        CategoryName = cat?.CategoryName ?? string.Empty,
//                        Description = b.Description
//                    });
//                }

//                ViewData["CategoryName"] = displayCategoryName;
//                ViewBag.CategoryRoute = routeCategory;
//                ViewBag.Page = page;
//                ViewBag.TotalPages = totalPages;
//                ViewBag.PageSize = pageSize;
//                ViewBag.TotalItems = totalItems;

//                return View("BookList", vmList);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error loading books for category {Category}", category);
//                return StatusCode(500, "An error occurred while loading books.");
//            }
//        }

//        public async Task<IActionResult> BookDetails(int id)
//        {
//            try
//            {
//                _logger.LogInformation("Fetching details for book ID: {BookId}", id);

//                var book = await _bookRepo.GetBookByIdAsync(id);
//                if (book == null)
//                {
//                    _logger.LogWarning("Book with ID {BookId} not found.", id);
//                    return NotFound();
//                }

//                var category = await _categoryRepo.GetCategoryById(book.CategoryID);

//                var bookVM = new BookViewModel
//                {
//                    BookID = book.BookID,
//                    Title = book.Title,
//                    Author = book.Author,
//                    Price = book.Price,
//                    CoverImage = book.CoverImage,
//                    Description = book.Description,
//                    CategoryName = category?.CategoryName ?? string.Empty
//                };

//                var relatedBooks = (await _bookRepo.GetBooksByCategoryAsync(book.CategoryID))
//                   .Where(b => b.BookID != book.BookID)
//                   .OrderByDescending(b => b.BookID)
//                   .Take(10)
//                   .Select(b => new BookViewModel
//                   {
//                       BookID = b.BookID,
//                       Title = b.Title,
//                       Author = b.Author,
//                       Price = b.Price,
//                       CoverImage = b.CoverImage,
//                       Description = b.Description,
//                       CategoryName = category?.CategoryName ?? string.Empty
//                   })
//                   .ToList();

//                ViewBag.RelatedBooks = relatedBooks;

//                return View("BookDetails", bookVM);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error fetching details for book ID {BookId}", id);
//                return StatusCode(500, "An error occurred while loading book details.");
//            }
//        }


//        // Add this method inside the existing BooksController class.
//        [HttpGet]
//        public async Task<IActionResult> Search(string q)
//        {
//            // Defensive: return empty when no query or very short to reduce load.
//            if (string.IsNullOrWhiteSpace(q) || q.Trim().Length < 2)
//                return Json(Array.Empty<BookViewModel>());

//            q = q.Trim();
//            try
//            {
//                // Fetch all books (use repository search later for performance)
//                var allBooks = await _bookRepo.GetAllBooksAsync();

//                // Cache categories to avoid repeated DB calls
//                var categoryCache = new Dictionary<int, string>();

//                var matches = new List<BookViewModel>();
//                foreach (var b in allBooks)
//                {
//                    string catName = string.Empty;
//                    if (b.CategoryID > 0)
//                    {
//                        if (!categoryCache.TryGetValue(b.CategoryID, out catName))
//                        {
//                            var c = await _categoryRepo.GetCategoryById(b.CategoryID);
//                            catName = c?.CategoryName ?? string.Empty;
//                            categoryCache[b.CategoryID] = catName;
//                        }
//                    }

//                    // Case-insensitive contains on Title, Author, Category
//                    if ((b.Title?.Contains(q, StringComparison.OrdinalIgnoreCase) ?? false)
//                        || (b.Author?.Contains(q, StringComparison.OrdinalIgnoreCase) ?? false)
//                        || (!string.IsNullOrEmpty(catName) && catName.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0))
//                    {
//                        matches.Add(new BookViewModel
//                        {
//                            BookID = b.BookID,
//                            Title = b.Title,
//                            Author = b.Author,
//                            Price = b.Price,
//                            CoverImage = b.CoverImage,
//                            CategoryName = catName,
//                            Description = b.Description
//                        });
//                    }
//                }

//                // Return minimal JSON (you can limit number of results here)
//                var resultToReturn = matches
//                    .OrderBy(m => m.Title)
//                    .Take(20)
//                    .Select(m => new {
//                        id = m.BookID,
//                        title = m.Title,
//                        author = m.Author,
//                        cover = string.IsNullOrWhiteSpace(m.CoverImage) ? Url.Content("~/images/placeholder-book.png") : Url.Content(m.CoverImage).Replace(" ", "%20"),
//                        category = m.CategoryName
//                    });

//                return Json(resultToReturn);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Search error for query '{Query}'", q);
//                return StatusCode(500);
//            }
//        }
//    }
//}




using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_Project.Models;
using Web_Project.Models.Interfaces;
using Web_Project.Services;

namespace Web_Project.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookRepository _bookRepo;
        private readonly ICategoryRepository _categoryRepo;
        private readonly ILogger<BooksController> _logger;

        public BooksController(
            IBookRepository bookRepo,
            ICategoryRepository categoryRepo,
            ILogger<BooksController> logger)
        {
            _bookRepo = bookRepo;
            _categoryRepo = categoryRepo;
            _logger = logger;
        }

        public class BookViewModel
        {
            public int BookID { get; set; }
            public string Title { get; set; } = string.Empty;
            public string Author { get; set; } = string.Empty;
            public decimal Price { get; set; }
            public string CoverImage { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public string CategoryName { get; set; } = string.Empty;
            public int StockQuantity { get; set; }
        }

        // Keep Index minimal and forward to BookList to make routing explicit from the views
        public IActionResult Index(string? category = null, int? categoryId = null)
        {
            // Forward any category/categoryId received to BookList so it shows the expected category.
            return RedirectToAction(nameof(BookList), new { category, categoryId });
        }

        // Updated: support a search query 'q' in addition to category/categoryId and paging.
        public async Task<IActionResult> BookList(string? category = null, int? categoryId = null, string? q = null, int page = 1)
        {
            const int pageSize = 20;
            try
            {
                // If a free-text search 'q' is provided, perform search across Title, Author and Category.
                if (!string.IsNullOrWhiteSpace(q))
                {
                    var query = q.Trim();
                    var allBooks = await _bookRepo.GetAllBooksAsync();

                    // Build category cache to map category id -> name
                    var categoryCache = new Dictionary<int, string>();
                    var results = new List<BookViewModel>();

                    foreach (var b in allBooks)
                    {
                        string catName = string.Empty;
                        if (b.CategoryID > 0)
                        {
                            if (!categoryCache.TryGetValue(b.CategoryID, out catName))
                            {
                                var c = await _categoryRepo.GetCategoryById(b.CategoryID);
                                catName = c?.CategoryName ?? string.Empty;
                                categoryCache[b.CategoryID] = catName;
                            }
                        }

                        if ((b.Title?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false)
                            || (b.Author?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false)
                            || (!string.IsNullOrEmpty(catName) && catName.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0))
                        {
                            results.Add(new BookViewModel
                            {
                                BookID = b.BookID,
                                Title = b.Title,
                                Author = b.Author,
                                Price = b.Price,
                                CoverImage = b.CoverImage,
                                CategoryName = catName,
                                Description = b.Description
                            });
                        }
                    }

                    // Simple paging for search results
                    var totalItems = results.Count;
                    var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
                    if (totalPages == 0) totalPages = 1;
                    if (page < 1) page = 1;
                    if (page > totalPages) page = totalPages;

                    var pageItems = results
                        .OrderBy(r => r.Title)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    ViewData["CategoryName"] = $"Search results for \"{query}\"";
                    ViewBag.CategoryRoute = "Search";
                    ViewBag.Page = page;
                    ViewBag.TotalPages = totalPages;
                    ViewBag.PageSize = pageSize;
                    ViewBag.TotalItems = totalItems;

                    return View("BookList", pageItems);
                }

                if (categoryId.HasValue)
                {
                    var catById = await _categoryRepo.GetCategoryById(categoryId.Value);
                    if (catById == null)
                    {
                        _logger.LogWarning("Category with ID {CategoryId} not found.", categoryId);
                        return NotFound();
                    }
                    category = catById.CategoryName;
                }

                // normalize category for routing and display
                var routeCategory = string.IsNullOrWhiteSpace(category) ? "Fantasy" : category;

                IEnumerable<Book> allSource;
                string displayCategoryName;

                if (string.Equals(routeCategory, "All", StringComparison.OrdinalIgnoreCase))
                {
                    allSource = await _bookRepo.GetAllBooksAsync();
                    displayCategoryName = "All Books";
                }
                else
                {
                    var categoryObj = await _categoryRepo.GetCategoryByNameAsync(routeCategory);
                    if (categoryObj == null)
                    {
                        _logger.LogWarning("Category '{Category}' not found.", routeCategory);
                        return NotFound();
                    }
                    allSource = await _bookRepo.GetBooksByCategoryAsync(categoryObj.CategoryID);
                    displayCategoryName = categoryObj.CategoryName;
                }

                var itemList = allSource.ToList();
                var totalItemsCount = itemList.Count;
                var totalPagesCount = (int)Math.Ceiling(totalItemsCount / (double)pageSize);
                if (totalPagesCount == 0) totalPagesCount = 1;

                if (page < 1) page = 1;
                if (page > totalPagesCount) page = totalPagesCount;

                var pageItemsList = itemList
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var vmList = new List<BookViewModel>(capacity: pageItemsList.Count);
                foreach (var b in pageItemsList)
                {
                    var cat = await _categoryRepo.GetCategoryById(b.CategoryID);
                    vmList.Add(new BookViewModel
                    {
                        BookID = b.BookID,
                        Title = b.Title,
                        Author = b.Author,
                        Price = b.Price,
                        CoverImage = b.CoverImage,
                        CategoryName = cat?.CategoryName ?? string.Empty,
                        Description = b.Description
                    });
                }

                ViewData["CategoryName"] = displayCategoryName;
                ViewBag.CategoryRoute = routeCategory;
                ViewBag.Page = page;
                ViewBag.TotalPages = totalPagesCount;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalItems = totalItemsCount;

                return View("BookList", vmList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading books for category {Category}", category);
                return StatusCode(500, "An error occurred while loading books.");
            }
        }

        public async Task<IActionResult> BookDetails(int id)
        {
            try
            {
                _logger.LogInformation("Fetching details for book ID: {BookId}", id);

                var book = await _bookRepo.GetBookByIdAsync(id);
                if (book == null)
                {
                    _logger.LogWarning("Book with ID {BookId} not found.", id);
                    return NotFound();
                }

                var category = await _categoryRepo.GetCategoryById(book.CategoryID);

                var bookVM = new BookViewModel
                {
                    BookID = book.BookID,
                    Title = book.Title,
                    Author = book.Author,
                    Price = book.Price,
                    CoverImage = book.CoverImage,
                    Description = book.Description,
                    CategoryName = category?.CategoryName ?? string.Empty,
                    StockQuantity = book.StockQuantity
                };

                var relatedBooks = (await _bookRepo.GetBooksByCategoryAsync(book.CategoryID))
                   .Where(b => b.BookID != book.BookID)
                   .OrderByDescending(b => b.BookID)
                   .Take(10)
                   .Select(b => new BookViewModel
                   {
                       BookID = b.BookID,
                       Title = b.Title,
                       Author = b.Author,
                       Price = b.Price,
                       CoverImage = b.CoverImage,
                       Description = b.Description,
                       CategoryName = category?.CategoryName ?? string.Empty,
                       StockQuantity = b.StockQuantity
                   })
                   .ToList();

                ViewBag.RelatedBooks = relatedBooks;

                return View("BookDetails", bookVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching details for book ID {BookId}", id);
                return StatusCode(500, "An error occurred while loading book details.");
            }
        }

        // Keep existing Search endpoint (returns JSON for suggestions etc.)
        [HttpGet]
        public async Task<IActionResult> Search(string q)
        {
            if (string.IsNullOrWhiteSpace(q) || q.Trim().Length < 2)
                return Json(Array.Empty<BookViewModel>());

            q = q.Trim();
            try
            {
                var allBooks = await _bookRepo.GetAllBooksAsync();
                var categoryCache = new Dictionary<int, string>();

                var matches = new List<BookViewModel>();
                foreach (var b in allBooks)
                {
                    string catName = string.Empty;
                    if (b.CategoryID > 0)
                    {
                        if (!categoryCache.TryGetValue(b.CategoryID, out catName))
                        {
                            var c = await _categoryRepo.GetCategoryById(b.CategoryID);
                            catName = c?.CategoryName ?? string.Empty;
                            categoryCache[b.CategoryID] = catName;
                        }
                    }

                    if ((b.Title?.Contains(q, StringComparison.OrdinalIgnoreCase) ?? false)
                        || (b.Author?.Contains(q, StringComparison.OrdinalIgnoreCase) ?? false)
                        || (!string.IsNullOrEmpty(catName) && catName.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0))
                    {
                        matches.Add(new BookViewModel
                        {
                            BookID = b.BookID,
                            Title = b.Title,
                            Author = b.Author,
                            Price = b.Price,
                            CoverImage = b.CoverImage,
                            CategoryName = catName,
                            Description = b.Description
                        });
                    }
                }

                var resultToReturn = matches
                    .OrderBy(m => m.Title)
                    .Take(20)
                    .Select(m => new {
                        id = m.BookID,
                        title = m.Title,
                        author = m.Author,
                        cover = string.IsNullOrWhiteSpace(m.CoverImage) ? Url.Content("~/images/placeholder-book.png") : Url.Content(m.CoverImage).Replace(" ", "%20"),
                        category = m.CategoryName
                    });

                return Json(resultToReturn);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Search error for query '{Query}'", q);
                return StatusCode(500);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Autocomplete(string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return Json(Array.Empty<object>());

            // Prefer a service/repository method in real app; this is simple and safe.
            var books = (await _bookRepo.GetAllBooksAsync()) // returns IEnumerable<Book>
                .Where(b => !string.IsNullOrEmpty(b.Title) && b.Title.Contains(q, StringComparison.OrdinalIgnoreCase))
                .OrderBy(b => b.Title)
                .Take(10)
                .Select(b => new { id = b.BookID, title = b.Title })
                .ToList();

            return Json(books);
        }
    }
}