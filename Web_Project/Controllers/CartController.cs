////using Azure.Core;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Web_Project.Models.Interfaces;
//using Web_Project.Models.ViewModels;

//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Web_Project.Helpers;
//using Web_Project.Models.Interfaces;
//using Web_Project.Models.ViewModels;

//namespace Web_Project.Controllers
//{
//    [Authorize(Policy = "UserAccess")]
//    public class CartController : Controller
//    {
//        private const string SessionCartKey = "Cart";
//        private readonly IBookRepository _bookRepo;
//        private readonly ILogger<CartController> _logger;

//        public CartController(IBookRepository bookRepo, ILogger<CartController> logger)
//        {
//            _bookRepo = bookRepo ?? throw new ArgumentNullException(nameof(bookRepo));
//            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
//        }

//        // lightweight DTO stored in session
//        private class SessionCartItem
//        {
//            public int BookID { get; set; }
//            public int Quantity { get; set; }
//        }

//        // Show cart
//        public async Task<IActionResult> Index()
//        {
//            try
//            {
//                var cart = HttpContext.Session.GetObject<List<SessionCartItem>>(SessionCartKey) ?? new List<SessionCartItem>();
//                var vm = new List<CartItemVM>();

//                foreach (var item in cart)
//                {
//                    var book = await _bookRepo.GetBookByIdAsync(item.BookID);
//                    if (book == null) continue;

//                    vm.Add(new CartItemVM
//                    {
//                        BookID = book.BookID,
//                        Title = book.Title,
//                        CoverImage = book.CoverImage ?? string.Empty,
//                        Price = book.Price,
//                        Quantity = item.Quantity
//                    });
//                }

//                return View(vm);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error loading cart");
//                return StatusCode(500, "An error occurred while loading the cart.");
//            }
//        }

//        // Add to cart (POST)
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public IActionResult Add(int bookId, int quantity = 1)
//        {
//            if (quantity < 1) quantity = 1;

//            var cart = HttpContext.Session.GetObject<List<SessionCartItem>>(SessionCartKey) ?? new List<SessionCartItem>();
//            var existing = cart.FirstOrDefault(c => c.BookID == bookId);
//            if (existing != null)
//            {
//                existing.Quantity += quantity;
//            }
//            else
//            {
//                cart.Add(new SessionCartItem { BookID = bookId, Quantity = quantity });
//            }

//            HttpContext.Session.SetObject(SessionCartKey, cart);
//            TempData["Success"] = "Item added to cart.";

//            var referer = Request.Headers["Referer"].ToString();
//            if (!string.IsNullOrEmpty(referer)) return Redirect(referer);

//            return RedirectToAction("Index", "Books", new { id = bookId });
//        }

//        // Update quantity (POST)
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public IActionResult UpdateQuantity(int bookId, int quantity)
//        {
//            if (quantity < 1) quantity = 1;

//            var cart = HttpContext.Session.GetObject<List<SessionCartItem>>(SessionCartKey) ?? new List<SessionCartItem>();
//            var item = cart.FirstOrDefault(c => c.BookID == bookId);
//            if (item != null)
//            {
//                item.Quantity = quantity;
//                HttpContext.Session.SetObject(SessionCartKey, cart);
//            }

//            return RedirectToAction(nameof(Index));
//        }

//        // Remove item (POST)
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public IActionResult Remove(int bookId)
//        {
//            var cart = HttpContext.Session.GetObject<List<SessionCartItem>>(SessionCartKey) ?? new List<SessionCartItem>();
//            cart.RemoveAll(c => c.BookID == bookId);
//            HttpContext.Session.SetObject(SessionCartKey, cart);
//            return RedirectToAction(nameof(Index));
//        }

//        // Clear cart (POST)
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public IActionResult Clear()
//        {
//            HttpContext.Session.Remove(SessionCartKey);
//            return RedirectToAction(nameof(Index));
//        }

//        // Checkout - redirect to Checkout page/flow (implement Checkout controller/view separately)
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public IActionResult Checkout()
//        {
//            // For now redirect to a Checkout controller/action; implement order creation there.
//            return RedirectToAction("Index", "Checkout");
//        }

//        // Continue shopping
//        public IActionResult Continue()
//        {
//            return RedirectToAction("Index", "Home");
//        }
//    }
//}


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_Project.Helpers;
using Web_Project.Models.Interfaces;
using Web_Project.Models.ViewModels;

namespace Web_Project.Controllers
{
    [Authorize(Policy = "UserAccess")]
    public class CartController : Controller
    {
        private const string SessionCartKey = "Cart";
        private readonly IBookRepository _bookRepo;
        private readonly ILogger<CartController> _logger;

        public CartController(IBookRepository bookRepo, ILogger<CartController> logger)
        {
            _bookRepo = bookRepo ?? throw new ArgumentNullException(nameof(bookRepo));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // lightweight DTO stored in session
        private class SessionCartItem
        {
            public int BookID { get; set; }
            public int Quantity { get; set; }
        }

        // Show cart
        public async Task<IActionResult> Index()
        {
            try
            {
                var cart = HttpContext.Session.GetObject<List<SessionCartItem>>(SessionCartKey) ?? new List<SessionCartItem>();
                var vm = new List<CartItemVM>();

                foreach (var item in cart)
                {
                    var book = await _bookRepo.GetBookByIdAsync(item.BookID);
                    if (book == null) continue;

                    vm.Add(new CartItemVM
                    {
                        BookID = book.BookID,
                        Title = book.Title,
                        CoverImage = book.CoverImage ?? string.Empty,
                        Price = book.Price,
                        Quantity = item.Quantity
                    });
                }

                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading cart");
                return StatusCode(500, "An error occurred while loading the cart.");
            }
        }

        // Add to cart (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int bookId, int quantity = 1)
        {
            if (quantity < 1) quantity = 1;

            // load current book to validate availability
            var book = await _bookRepo.GetBookByIdAsync(bookId);
            if (book == null)
            {
                var notFoundMsg = "Book not found.";
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return Json(new { success = false, message = notFoundMsg });

                TempData["Error"] = notFoundMsg;
                var refererErr = Request.Headers["Referer"].ToString();
                if (!string.IsNullOrEmpty(refererErr)) return Redirect(refererErr);
                return RedirectToAction("Index", "Books");
            }

            var cart = HttpContext.Session.GetObject<List<SessionCartItem>>(SessionCartKey) ?? new List<SessionCartItem>();
            var existing = cart.FirstOrDefault(c => c.BookID == bookId);
            var alreadyInCart = existing?.Quantity ?? 0;
            var totalRequested = alreadyInCart + quantity;

            // Server-side availability check: total(in-cart) + requested must not exceed stock
            if (totalRequested > book.StockQuantity)
            {
                var availableNow = Math.Max(0, book.StockQuantity - alreadyInCart);
                var msg = availableNow > 0
                    ? $"Only {availableNow} more of \"{book.Title}\" can be added to your cart."
                    : $"\"{book.Title}\" has no more available stock.";

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return Json(new { success = false, message = msg });

                TempData["Error"] = msg;
                var referer = Request.Headers["Referer"].ToString();
                if (!string.IsNullOrEmpty(referer)) return Redirect(referer);

                return RedirectToAction("Index", "Books", new { id = bookId });
            }

            // Update session cart
            if (existing != null)
            {
                existing.Quantity += quantity;
            }
            else
            {
                cart.Add(new SessionCartItem { BookID = bookId, Quantity = quantity });
            }

            HttpContext.Session.SetObject(SessionCartKey, cart);

            // Return JSON for AJAX callers, otherwise redirect with TempData
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Item added to cart." });
            }

            TempData["Success"] = "Item added to cart.";

            var refererHeader = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(refererHeader)) return Redirect(refererHeader);

            return RedirectToAction("Index", "Books", new { id = bookId });
        }

        // Update quantity (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateQuantity(int bookId, int quantity)
        {
            if (quantity < 1) quantity = 1;

            var cart = HttpContext.Session.GetObject<List<SessionCartItem>>(SessionCartKey) ?? new List<SessionCartItem>();
            var item = cart.FirstOrDefault(c => c.BookID == bookId);
            if (item != null)
            {
                item.Quantity = quantity;
                HttpContext.Session.SetObject(SessionCartKey, cart);
            }

            return RedirectToAction(nameof(Index));
        }

        // Remove item (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remove(int bookId)
        {
            var cart = HttpContext.Session.GetObject<List<SessionCartItem>>(SessionCartKey) ?? new List<SessionCartItem>();
            cart.RemoveAll(c => c.BookID == bookId);
            HttpContext.Session.SetObject(SessionCartKey, cart);
            return RedirectToAction(nameof(Index));
        }

        // Clear cart (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Clear()
        {
            HttpContext.Session.Remove(SessionCartKey);
            return RedirectToAction(nameof(Index));
        }

        // Checkout - redirect to Checkout page/flow (implement Checkout controller/view separately)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Checkout()
        {
            // For now redirect to a Checkout controller/action; implement order creation there.
            return RedirectToAction("Index", "Checkout");
        }

        // Continue shopping
        public IActionResult Continue()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}