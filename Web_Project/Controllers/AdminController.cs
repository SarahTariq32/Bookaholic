
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Web_Project.Models;
//using Web_Project.Models.Interfaces;
//using Web_Project.Models.ViewModels;
//using Web_Project.Services;

//namespace Web_Project.Controllers
//{
//    public class OrderDetailsVM
//    {
//        public int OrderId { get; set; }
//        public string CustomerName { get; set; }
//        public string Status { get; set; }

//        public List<OrderItemVM> Items { get; set; } = new List<OrderItemVM>();
//        public decimal TotalAmount { get; set; }
//        public decimal ShippingFee { get; set; }
//        public DateTime EstimatedDelivery { get; set; }
//        public string Address { get; set; } = string.Empty;
//        public string CustomerEmail { get; set; } = string.Empty;
//        public string CustomerPhone { get; set; } = string.Empty;
//        public string PaymentMethod { get; set; } = "N/A";



//    }

//    public class OrderItemVM
//    {
//        public string BookTitle { get; set; }
//        public int Quantity { get; set; }
//        public decimal Price { get; set; }
//    }
//    [Authorize(Policy = "AdminAccess")]
//    public class AdminController : Controller
//    {
//        private readonly IBookService _bookService;
//        private readonly ICategoryService _categoryService;
//        private readonly IOrderService _orderService;

//        public AdminController(
//            IBookService bookService,
//            ICategoryService categoryService,
//            IOrderService orderService)
//        {
//            _bookService = bookService;
//            _categoryService = categoryService;
//            _orderService = orderService;
//        }

//        // -------------------- Dashboard --------------------
//        public async Task<IActionResult> Dashboard()
//        {
//            var stats = new
//            {
//                TotalBooks = await _bookService.CountBooksAsync(),
//                TotalOrders = await _orderService.CountOrdersAsync(),
//                Delivered = await _orderService.CountDeliveredAsync(),
//            };

//            return View(stats);
//        }

//        [HttpGet]
//        public async Task<IActionResult> ManageBooks()
//        {
//            var books = await _bookService.GetAllBooksAsync();
//            var categories = await _categoryService.GetAllCategories();
//            ViewBag.Categories = categories.ToList();
//            ViewBag.CategoryNames = categories.ToDictionary(c => c.CategoryID, c => c.CategoryName);

//            return View(books);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> AddBook(Book model, string ImageName)
//        {
//            // 🚑 FIX: remove CoverImage from validation
//            ModelState.Remove("CoverImage");

//            if (string.IsNullOrWhiteSpace(ImageName))
//            {
//                TempData["Error"] = "Please enter a cover image name.";
//                return RedirectToAction("ManageBooks");
//            }

//            string parentCategory = (model.CategoryID <= 5) ? "fiction" : "non-fiction";

//            string subCategory = model.CategoryID switch
//            {
//                1 => "fantasy",
//                2 => "thriller",
//                3 => "horror",
//                4 => "romance",
//                5 => "children",
//                6 => "islamic",
//                7 => "urdu",
//                8 => "history",
//                9 => "self-help",
//                _ => "unknown"
//            };

//            model.CoverImage = $"/images/{parentCategory}/{subCategory}/{ImageName}.jpg";

//            if (!ModelState.IsValid)
//            {
//                TempData["Error"] = "Please fill out all fields correctly.";
//                return RedirectToAction("ManageBooks");
//            }

//            await _bookService.AddBookAsync(model);

//            TempData["Success"] = $"Book '{model.Title}' added successfully!";
//            return RedirectToAction("ManageBooks");
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteBook(int id)
//        {
//            var book = await _bookService.GetBookByIdAsync(id);
//            if (book == null)
//            {
//                TempData["Error"] = "Book not found.";
//                return RedirectToAction("ManageBooks");
//            }

//            await _bookService.DeleteBookAsync(id);
//            TempData["Success"] = $"Book '{book.Title}' deleted successfully!";
//            return RedirectToAction("ManageBooks");
//        }

//        // Ensure Edit is an explicit POST and validate antiforgery token.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> EditBook([Bind("BookID,Title,Author,CategoryID,Price,StockQuantity,Description,CoverImage")] Book model)
//        {
//            // preserve existing CoverImage (form sends hidden CoverImage) and prevent validation errors
//            ModelState.Remove("CoverImage");

//            if (!ModelState.IsValid)
//            {
//                TempData["Error"] = "Please fill out all fields correctly.";
//                return RedirectToAction("ManageBooks");
//            }

//            var success = await _bookService.UpdateBookAsync(model);
//            if (!success)
//            {
//                TempData["Error"] = "Unable to update book. Check server logs.";
//                return RedirectToAction("ManageBooks");
//            }

//            TempData["Success"] = $"Book '{model.Title}' updated successfully!";
//            return RedirectToAction("ManageBooks");
//        }
//        // -------------------- CATEGORIES --------------------

//        public async Task<IActionResult> ManageCategories()
//        {

//            var categories = (await _categoryService.GetAllCategories()).ToList();
//            return View(categories);
//        }

//        [HttpPost]
//        public async Task<IActionResult> AddCategory(string CategoryName)
//        {
//            if (string.IsNullOrWhiteSpace(CategoryName))
//            {
//                TempData["Error"] = "Category name cannot be empty.";
//                return RedirectToAction("ManageCategories");
//            }
//            IEnumerable<Category> existingCategories = await _categoryService.GetAllCategories();
//            if (existingCategories.Any(c => c.CategoryName.Equals(CategoryName.Trim(), StringComparison.OrdinalIgnoreCase)))
//            {
//                TempData["Error"] = $"Category '{CategoryName}' already exists.";
//                return RedirectToAction("ManageCategories");
//            }

//            var newCategory = new Category
//            {
//                CategoryName = CategoryName.Trim()
//            };
//            await _categoryService.AddCategory(newCategory);

//            TempData["Success"] = $"Category '{CategoryName}' added successfully!";
//            return RedirectToAction("ManageCategories");
//        }


//        [HttpPost]
//        public async Task<IActionResult> DeleteCategory(int id)
//        {
//            Category category = await _categoryService.GetCategoryById(id);
//            if (category == null)
//            {
//                TempData["Error"] = "Category not found.";
//                return RedirectToAction("ManageCategories");
//            }
//            await _categoryService.DeleteCategory(id);

//            TempData["Success"] = $"Category '{category.CategoryName}' deleted successfully!";
//            return RedirectToAction("ManageCategories");
//        }
//        // -------------------- ORDERS --------------------
//        // ... rest unchanged ...

//        // replace the ManageOrders action with this implementation (adds ViewBag.BookTitles)
//        [HttpGet]
//        public async Task<IActionResult> ManageOrders()
//        {
//            var orders = (await _orderService.GetAllOrdersAsync()).OrderByDescending(o => o.OrderDate).ToList();

//            // Build a dictionary of book titles so the view's GetBookTitle can look them up safely.
//            // This avoids loading titles inline in the view and fixes blank book names in the modal.
//            var books = (await _bookService.GetAllBooksAsync()).ToList();
//            ViewBag.BookTitles = books.ToDictionary(b => b.BookID, b => b.Title);

//            return View(orders);
//        }

//        // inside OrderDetails action: set PaymentMethod from order if present
//        [HttpGet]
//        public async Task<IActionResult> OrderDetails(int id)
//        {
//            var order = await _orderService.GetOrderByIdAsync(id);
//            if (order == null) return NotFound();

//            var vm = new OrderDetailsVM
//            {
//                OrderId = order.Id,
//                CustomerName = order.CustomerName,
//                Status = order.Status,
//                Items = new List<OrderItemVM>(),
//                TotalAmount = order.TotalAmount,
//                ShippingFee = 200,
//                EstimatedDelivery = DateTime.UtcNow.AddDays(3),

//                // populate from order (PaymentMethod will be null if not stored)
//                Address = order.Address ?? string.Empty,
//                CustomerEmail = order.CustomerEmail ?? string.Empty,
//                CustomerPhone = order.CustomerPhone ?? string.Empty,
//                PaymentMethod = string.IsNullOrWhiteSpace(order.PaymentMethod) ? "N/A" : order.PaymentMethod
//            };

//            foreach (var detail in order.OrderDetails ?? Enumerable.Empty<Web_Project.Models.OrderDetail>())
//            {
//                var book = await _bookService.GetBookByIdAsync(detail.BookID);
//                vm.Items.Add(new OrderItemVM
//                {
//                    BookTitle = book?.Title ?? "Unknown Book",
//                    Quantity = detail.Quantity,
//                    Price = detail.PriceAtPurchase
//                });
//            }

//            return PartialView("_OrderDetailsModal", vm);
//        }
//        //[HttpGet]
//        //public async Task<IActionResult> ManageOrders()
//        //{
//        //    var orders = (await _orderService.GetAllOrdersAsync()).OrderByDescending(o => o.OrderDate).ToList();
//        //    return View(orders);
//        //}

//        //[HttpPost]
//        //public async Task<IActionResult> MarkDelivered(int id)
//        //{
//        //    var order = await _orderService.GetOrderByIdAsync(id);
//        //    if (order != null && order.Status != "Delivered")
//        //    {
//        //        await _orderService.UpdateOrderStatusAsync(id, "Delivered");
//        //        TempData["Success"] = $"Order #{id} marked as Delivered.";
//        //    }
//        //    else
//        //    {
//        //        TempData["Error"] = "Order not found or already delivered.";
//        //    }
//        //    return RedirectToAction("ManageOrders");
//        //}

//        //[HttpPost]
//        //public async Task<IActionResult> DeleteOrder(int id)
//        //{
//        //    var order = await _orderService.GetOrderByIdAsync(id);
//        //    if (order != null)
//        //    {
//        //        await _orderService.DeleteOrderAsync(id);
//        //        TempData["Success"] = $"Order #{id} deleted successfully.";
//        //    }
//        //    else
//        //    {
//        //        TempData["Error"] = "Order not found.";
//        //    }
//        //    return RedirectToAction("ManageOrders");
//        //}

//        //[HttpGet]
//        //public async Task<IActionResult> OrderDetails(int id)
//        //{
//        //    var order = await _orderService.GetOrderByIdAsync(id);
//        //    if (order == null) return NotFound();

//        //    var vm = new OrderDetailsVM
//        //    {
//        //        OrderId = order.Id,
//        //        CustomerName = order.CustomerName,
//        //        Status = order.Status,
//        //        Items = new List<OrderItemVM>(),
//        //        TotalAmount = order.TotalAmount,
//        //        ShippingFee = 200,
//        //        EstimatedDelivery = DateTime.UtcNow.AddDays(3)
//        //    };

//        //    foreach (var detail in order.OrderDetails)
//        //    {
//        //        var book = await _bookService.GetBookByIdAsync(detail.BookID);
//        //        vm.Items.Add(new OrderItemVM
//        //        {
//        //            BookTitle = book?.Title ?? "Unknown Book",
//        //            Quantity = detail.Quantity,
//        //            Price = detail.PriceAtPurchase
//        //        });
//        //    }

//        //    return PartialView("_OrderDetailsModal", vm);
//        //}

//        // inside AdminController class — replace only the OrderDetails action with the updated method below
//        [HttpGet]
//        public async Task<IActionResult> OrderDetails(int id)
//        {
//            var order = await _orderService.GetOrderByIdAsync(id);
//            if (order == null) return NotFound();

//            var vm = new OrderDetailsVM
//            {
//                OrderId = order.Id,
//                CustomerName = order.CustomerName,
//                Status = order.Status,
//                Items = new List<OrderItemVM>(),
//                TotalAmount = order.TotalAmount,
//                ShippingFee = 200,
//                EstimatedDelivery = DateTime.UtcNow.AddDays(3),

//                // new fields populated from order where available
//                Address = order.Address ?? string.Empty,
//                CustomerEmail = order.CustomerEmail ?? string.Empty,
//                CustomerPhone = order.CustomerPhone ?? string.Empty,
//                PaymentMethod = "N/A"
//            };

//            foreach (var detail in order.OrderDetails ?? Enumerable.Empty<Web_Project.Models.OrderDetail>())
//            {
//                var book = await _bookService.GetBookByIdAsync(detail.BookID);
//                vm.Items.Add(new OrderItemVM
//                {
//                    BookTitle = book?.Title ?? "Unknown Book",
//                    Quantity = detail.Quantity,
//                    Price = detail.PriceAtPurchase
//                });
//            }

//            return PartialView("_OrderDetailsModal", vm);
//        }

//        [HttpGet]
//        public async Task<IActionResult> Reports()
//        {
//            var orders = (await _orderService.GetAllOrdersAsync()).ToList();

//            var totalOrders = orders.Count;
//            var totalRevenue = orders.Sum(o => o.TotalAmount);
//            var totalCustomers = orders
//                .Select(o => (o.CustomerEmail ?? string.Empty).Trim().ToLowerInvariant())
//                .Where(e => !string.IsNullOrEmpty(e))
//                .Distinct()
//                .Count();

//            var now = DateTime.UtcNow;

//            // Monthly: last 12 months
//            var monthStart = new DateTime(now.Year, now.Month, 1).AddMonths(-11);
//            var monthlyLabels = new List<string>();
//            var monthlyRevenue = new List<decimal>();
//            var monthlyOrders = new List<int>();
//            for (int i = 0; i < 12; i++)
//            {
//                var start = monthStart.AddMonths(i);
//                var end = start.AddMonths(1);
//                var bucket = orders.Where(o => o.OrderDate >= start && o.OrderDate < end).ToList();
//                monthlyLabels.Add(start.ToString("MMM yyyy"));
//                monthlyRevenue.Add(bucket.Sum(o => o.TotalAmount));
//                monthlyOrders.Add(bucket.Count);
//            }

//            // Daily: last 30 days (including today)
//            var dailyStart = now.Date.AddDays(-29);
//            var dailyLabels = new List<string>();
//            var dailyRevenue = new List<decimal>();
//            var dailyOrders = new List<int>();
//            for (int d = 0; d < 30; d++)
//            {
//                var day = dailyStart.AddDays(d);
//                var nextDay = day.AddDays(1);
//                var bucket = orders.Where(o => o.OrderDate >= day && o.OrderDate < nextDay).ToList();
//                dailyLabels.Add(day.ToString("dd MMM"));
//                dailyRevenue.Add(bucket.Sum(o => o.TotalAmount));
//                dailyOrders.Add(bucket.Count);
//            }

//            var vm = new ReportsVM
//            {
//                TotalOrders = totalOrders,
//                TotalRevenue = totalRevenue,
//                TotalCustomers = totalCustomers,
//                RevenueLabels = monthlyLabels,
//                RevenueValues = monthlyRevenue,
//                OrdersPerMonth = monthlyOrders,
//                DailyLabels = dailyLabels,
//                DailyRevenue = dailyRevenue,
//                DailyOrders = dailyOrders
//            };

//            return View(vm);
//        }
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Logout()
//        {
//            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
//            await HttpContext.SignOutAsync();
//            return Redirect("~/");
//        }
//    }
//}

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using Web_Project.Hubs;
using Web_Project.Models;
using Web_Project.Models.Interfaces;
using Web_Project.Models.ViewModels;
using Web_Project.Services;

namespace Web_Project.Controllers
{
    public class OrderDetailsVM
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;

        public List<OrderItemVM> Items { get; set; } = new List<OrderItemVM>();
        public decimal TotalAmount { get; set; }
        public decimal ShippingFee { get; set; }
        public DateTime EstimatedDelivery { get; set; }
        public string Address { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = "N/A";
    }

    public class OrderItemVM
    {
        public string BookTitle { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

    [Authorize(Policy = "AdminAccess")]
    public class AdminController : Controller
    {
        private readonly IBookService _bookService;
        private readonly ICategoryService _categoryService;
        private readonly IOrderService _orderService;
        private readonly IHubContext<OrderHub> _hub;
        private readonly ILogger<AdminController> _logger;


        public AdminController(
            IBookService bookService,
            ICategoryService categoryService,
            IOrderService orderService, IHubContext<OrderHub> hub,
            ILogger<AdminController> logger)
        {
            _bookService = bookService;
            _categoryService = categoryService;
            _orderService = orderService;
            _hub = hub ?? throw new ArgumentNullException(nameof(hub));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        
        }

        // -------------------- Dashboard --------------------
        public async Task<IActionResult> Dashboard()
        {
            var stats = new
            {
                TotalBooks = await _bookService.CountBooksAsync(),
                TotalOrders = await _orderService.CountOrdersAsync(),
                Delivered = await _orderService.CountDeliveredAsync(),
            };

            return View(stats);
        }

        [HttpGet]
        public async Task<IActionResult> ManageBooks()
        {
            var books = await _bookService.GetAllBooksAsync();
            var categories = await _categoryService.GetAllCategories();
            ViewBag.Categories = categories.ToList();
            ViewBag.CategoryNames = categories.ToDictionary(c => c.CategoryID, c => c.CategoryName);

            return View(books);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBook(Book model, string ImageName)
        {
            // 🚑 FIX: remove CoverImage from validation
            ModelState.Remove("CoverImage");

            if (string.IsNullOrWhiteSpace(ImageName))
            {
                TempData["Error"] = "Please enter a cover image name.";
                return RedirectToAction("ManageBooks");
            }

            string parentCategory = (model.CategoryID <= 5) ? "fiction" : "non-fiction";

            string subCategory = model.CategoryID switch
            {
                1 => "fantasy",
                2 => "thriller",
                3 => "horror",
                4 => "romance",
                5 => "children",
                6 => "islamic",
                7 => "urdu",
                8 => "history",
                9 => "self-help",
                _ => "unknown"
            };

            model.CoverImage = $"/images/{parentCategory}/{subCategory}/{ImageName}.jpg";

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please fill out all fields correctly.";
                return RedirectToAction("ManageBooks");
            }

            await _bookService.AddBookAsync(model);

            TempData["Success"] = $"Book '{model.Title}' added successfully!";

            // (insert inside your AddBook action after the book is saved successfully)
            // after successful add (keep TempData line above)
            var newBookPayload = new
            {
                bookId = model.BookID,
                title = model.Title ?? string.Empty,
                cover = model.CoverImage ?? string.Empty,
                categoryId = model.CategoryID
            };

            _logger?.LogInformation("Sending NewBookAdded SignalR payload: {@payload}", newBookPayload);
            try
            {
                await _hub.Clients.All.SendAsync("NewBookAdded", newBookPayload);
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "Failed sending NewBookAdded SignalR message");
            }
            return RedirectToAction("ManageBooks");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                TempData["Error"] = "Book not found.";
                return RedirectToAction("ManageBooks");
            }

            await _bookService.DeleteBookAsync(id);
            TempData["Success"] = $"Book '{book.Title}' deleted successfully!";
            return RedirectToAction("ManageBooks");
        }

        // Ensure Edit is an explicit POST and validate antiforgery token.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBook([Bind("BookID,Title,Author,CategoryID,Price,StockQuantity,Description,CoverImage")] Book model)
        {
            // preserve existing CoverImage (form sends hidden CoverImage) and prevent validation errors
            ModelState.Remove("CoverImage");

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please fill out all fields correctly.";
                return RedirectToAction("ManageBooks");
            }

            var success = await _bookService.UpdateBookAsync(model);
            if (!success)
            {
                TempData["Error"] = "Unable to update book. Check server logs.";
                return RedirectToAction("ManageBooks");
            }

            TempData["Success"] = $"Book '{model.Title}' updated successfully!";
            return RedirectToAction("ManageBooks");
        }

        // -------------------- CATEGORIES --------------------
        public async Task<IActionResult> ManageCategories()
        {
            var categories = (await _categoryService.GetAllCategories()).ToList();
            return View(categories);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(string CategoryName)
        {
            if (string.IsNullOrWhiteSpace(CategoryName))
            {
                TempData["Error"] = "Category name cannot be empty.";
                return RedirectToAction("ManageCategories");
            }
            IEnumerable<Category> existingCategories = await _categoryService.GetAllCategories();
            if (existingCategories.Any(c => c.CategoryName.Equals(CategoryName.Trim(), StringComparison.OrdinalIgnoreCase)))
            {
                TempData["Error"] = $"Category '{CategoryName}' already exists.";
                return RedirectToAction("ManageCategories");
            }

            var newCategory = new Category
            {
                CategoryName = CategoryName.Trim()
            };
            await _categoryService.AddCategory(newCategory);

            TempData["Success"] = $"Category '{CategoryName}' added successfully!";
            return RedirectToAction("ManageCategories");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            Category category = await _categoryService.GetCategoryById(id);
            if (category == null)
            {
                TempData["Error"] = "Category not found.";
                return RedirectToAction("ManageCategories");
            }
            await _categoryService.DeleteCategory(id);

            TempData["Success"] = $"Category '{category.CategoryName}' deleted successfully!";
            return RedirectToAction("ManageCategories");
        }

        // -------------------- ORDERS --------------------
        // replace the ManageOrders action with this implementation (adds ViewBag.BookTitles)
        [HttpGet]
        public async Task<IActionResult> ManageOrders()
        {
            var orders = (await _orderService.GetAllOrdersAsync()).OrderByDescending(o => o.OrderDate).ToList();

            // Build a dictionary of book titles so the view's GetBookTitle can look them up safely.
            // This avoids loading titles inline in the view and fixes blank book names in the modal.
            var books = (await _bookService.GetAllBooksAsync()).ToList();
            ViewBag.BookTitles = books.ToDictionary(b => b.BookID, b => b.Title);

            return View(orders);
        }

        // single OrderDetails action (duplicate removed)
        //[HttpGet]
        //public async Task<IActionResult> OrderDetails(int id)
        //{
        //    var order = await _orderService.GetOrderByIdAsync(id);
        //    if (order == null) return NotFound();

        //    var vm = new OrderDetailsVM
        //    {
        //        OrderId = order.Id,
        //        CustomerName = order.CustomerName,
        //        Status = order.Status,
        //        Items = new List<OrderItemVM>(),
        //        TotalAmount = order.TotalAmount,
        //        ShippingFee = 200,
        //        EstimatedDelivery = DateTime.UtcNow.AddDays(3),

        //        // populate from order (PaymentMethod will be null if not stored)
        //        Address = order.Address ?? string.Empty,
        //        CustomerEmail = order.CustomerEmail ?? string.Empty,
        //        CustomerPhone = order.CustomerPhone ?? string.Empty,
        //        PaymentMethod = string.IsNullOrWhiteSpace(order.PaymentMethod) ? "N/A" : order.PaymentMethod
        //    };

        //    foreach (var detail in order.OrderDetails ?? Enumerable.Empty<Web_Project.Models.OrderDetail>())
        //    {
        //        var book = await _bookService.GetBookByIdAsync(detail.BookID);
        //        vm.Items.Add(new OrderItemVM
        //        {
        //            BookTitle = book?.Title ?? "Unknown Book",
        //            Quantity = detail.Quantity,
        //            Price = detail.PriceAtPurchase
        //        });
        //    }

        //    return PartialView("_OrderDetailsModal", vm);
        //}

        // Replace the existing OrderDetails action implementation with this one.
        // It loads order details via IOrderService.GetOrderDetailsAsync instead of
        // relying on order.OrderDetails (which may be null if repository didn't include them).

        //[HttpGet]
        //public async Task<IActionResult> OrderDetails(int id)
        //{
        //    var order = await _orderService.GetOrderByIdAsync(id);
        //    if (order == null) return NotFound();

        //    // Fetch details from the order-detail repository through the service.
        //    var details = (await _orderService.GetOrderDetailsAsync(id))?.ToList()
        //                  ?? new List<Web_Project.Models.OrderDetail>();

        //    var vm = new OrderDetailsVM
        //    {
        //        OrderId = order.Id,
        //        CustomerName = order.CustomerName,
        //        Status = order.Status,
        //        Items = new List<OrderItemVM>(),
        //        // Prefer the stored total if present; otherwise compute from details
        //        TotalAmount = order.TotalAmount,
        //        ShippingFee = 200,
        //        EstimatedDelivery = order.OrderDate == default ? DateTime.UtcNow.AddDays(3) : order.OrderDate.AddDays(3),
        //        Address = order.Address ?? string.Empty,
        //        CustomerEmail = order.CustomerEmail ?? string.Empty,
        //        CustomerPhone = order.CustomerPhone ?? string.Empty,
        //        PaymentMethod = string.IsNullOrWhiteSpace(order.PaymentMethod) ? "N/A" : order.PaymentMethod
        //    };

        //    foreach (var detail in details)
        //    {
        //        var book = await _bookService.GetBookByIdAsync(detail.BookID);
        //        vm.Items.Add(new OrderItemVM
        //        {
        //            BookTitle = book?.Title ?? "Unknown Book",
        //            Quantity = detail.Quantity,
        //            Price = detail.PriceAtPurchase
        //        });
        //    }

        //    // If TotalAmount was not populated by repository, compute it from details.
        //    if (vm.TotalAmount == 0 && vm.Items.Any())
        //    {
        //        vm.TotalAmount = vm.Items.Sum(i => i.Price * i.Quantity) + vm.ShippingFee;
        //    }

        //    return PartialView("_OrderDetailsModal", vm);
        //}

        [HttpGet]
        public async Task<IActionResult> OrderDetails(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null) return NotFound();

            // load order lines from the order-detail repository via service
            var details = (await _orderService.GetOrderDetailsAsync(id))?.ToList()
                          ?? new List<Web_Project.Models.OrderDetail>();

            var vm = new Web_Project.Models.ViewModels.OrderDetailsVM
            {
                OrderId = order.Id,
                CustomerName = order.CustomerName,
                Status = order.Status,
                Items = new List<Web_Project.Models.ViewModels.OrderItemVM>(),
                TotalAmount = order.TotalAmount,
                ShippingFee = 200,
                EstimatedDelivery = order.OrderDate == default ? DateTime.UtcNow.AddDays(3) : order.OrderDate.AddDays(3),
                Address = order.Address ?? string.Empty,
                CustomerEmail = order.CustomerEmail ?? string.Empty,
                CustomerPhone = order.CustomerPhone ?? string.Empty,
                PaymentMethod = string.IsNullOrWhiteSpace(order.PaymentMethod) ? "N/A" : order.PaymentMethod
            };

            foreach (var d in details)
            {
                var book = await _bookService.GetBookByIdAsync(d.BookID);
                vm.Items.Add(new Web_Project.Models.ViewModels.OrderItemVM
                {
                    BookTitle = book?.Title ?? "Unknown Book",
                    Quantity = d.Quantity,
                    Price = d.PriceAtPurchase
                });
            }

            // fallback compute total if repository didn't supply it
            if (vm.TotalAmount == 0 && vm.Items.Any())
            {
                vm.TotalAmount = vm.Items.Sum(i => i.Price * i.Quantity) + vm.ShippingFee;
            }

            return PartialView("_OrderDetailsModal", vm);
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> MarkDelivered(int id)
        //{
        //    var order = await _orderService.GetOrderByIdAsync(id);
        //    if (order == null)
        //    {
        //        // If AJAX request, return JSON
        //        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        //            return Json(new { success = false, message = "Order not found." });

        //        TempData["Error"] = "Order not found.";
        //        return RedirectToAction("ManageOrders");
        //    }

        //    if (string.Equals(order.Status, "Delivered", StringComparison.OrdinalIgnoreCase))
        //    {
        //        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        //            return Json(new { success = false, message = "Order already delivered." });

        //        TempData["Error"] = "Order already delivered.";
        //        return RedirectToAction("ManageOrders");
        //    }

        //    await _orderService.UpdateOrderStatusAsync(id, "Delivered");
        //    TempData["Success"] = $"Order #{id} marked as Delivered.";

        //    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        //    {
        //        return Json(new { success = true, message = "Order is being delivered.", orderId = id });
        //    }
        //    return RedirectToAction("ManageOrders");
        //}



        // (Replace or merge into your existing AdminController — these methods assume _bookService and _orderService are available.)

        public async Task<IActionResult> MarkDelivered(int id)
            {
                var order = await _orderService.GetOrderByIdAsync(id);
                if (order == null)
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                        return Json(new { success = false, message = "Order not found." });

                    TempData["Error"] = "Order not found.";
                    return RedirectToAction("ManageOrders");
                }

                if (string.Equals(order.Status, "Delivered", StringComparison.OrdinalIgnoreCase))
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                        return Json(new { success = false, message = "Order already delivered." });

                    TempData["Error"] = "Order already delivered.";
                    return RedirectToAction("ManageOrders");
                }

                // Mark order delivered
                await _orderService.UpdateOrderStatusAsync(id, "Delivered");

                // Adjust stock for each ordered item.
                // We fetch order details from service to be safe.
                var details = (await _orderService.GetOrderDetailsAsync(id))?.ToList() ?? new List<Web_Project.Models.OrderDetail>();
                foreach (var d in details)
                {
                    var book = await _bookService.GetBookByIdAsync(d.BookID);
                    if (book == null) continue;

                    // Decrease stock by ordered quantity. Ensure non-negative.
                    var newQty = Math.Max(0, book.StockQuantity - d.Quantity);
                    if (newQty != book.StockQuantity)
                    {
                        book.StockQuantity = newQty;
                        await _bookService.UpdateBookAsync(book);
                    }
                }

                TempData["Success"] = $"Order #{id} marked as Delivered.";

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Order marked delivered and stock updated.", orderId = id });
                }

                return RedirectToAction("ManageOrders");
            }

            // GET: /Admin/LowStock
            [HttpGet]
            public async Task<IActionResult> LowStock(int threshold = 5)
            {
                // Get all books and filter low-stock ones.
                var all = (await _bookService.GetAllBooksAsync()).ToList();
                var low = all.Where(b => b.StockQuantity <= threshold).OrderBy(b => b.StockQuantity).ToList();

                // Pass threshold so UI can show it
                ViewBag.LowStockThreshold = threshold;
                return View("LowStock", low);
            }

        // POST: /Admin/Restock
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Restock(int id, int amount = 10)
        //{
        //    if (amount <= 0) amount = 10;
        //    var book = await _bookService.GetBookByIdAsync(id);
        //    if (book == null)
        //    {
        //        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        //            return Json(new { success = false, message = "Book not found." });

        //        TempData["Error"] = "Book not found.";
        //        return RedirectToAction("LowStock");
        //    }

        //    book.StockQuantity = book.StockQuantity + amount;
        //    await _bookService.UpdateBookAsync(book);

        //    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        //    {
        //        return Json(new { success = true, message = "Book restocked.", bookId = id, newQty = book.StockQuantity });
        //    }

        //    TempData["Success"] = $"Book '{book.Title}' restocked by {amount}.";

        //    // after successful restock and TempData set
        //    var restockPayload = new
        //    {
        //        bookId = book.BookID,
        //        title = book.Title ?? string.Empty,
        //        newQty = book.StockQuantity
        //    };

        //    _logger?.LogInformation("Sending BookRestocked SignalR payload: {@payload}", restockPayload);
        //    try
        //    {
        //        await _hub.Clients.All.SendAsync("BookRestocked", restockPayload);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger?.LogWarning(ex, "Failed sending BookRestocked SignalR message");
        //    }
        //    return RedirectToAction("LowStock");
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restock(int id, int amount = 10)
        {
            if (amount <= 0) amount = 10;
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return Json(new { success = false, message = "Book not found." });

                TempData["Error"] = "Book not found.";
                return RedirectToAction("LowStock");
            }

            book.StockQuantity = book.StockQuantity + amount;
            await _bookService.UpdateBookAsync(book);

            // Build payload and send BEFORE returning (so both AJAX and non-AJAX paths get the notification)
            var restockPayload = new
            {
                bookId = book.BookID,
                title = book.Title ?? string.Empty,
                newQty = book.StockQuantity
            };

            _logger?.LogInformation("Sending BookRestocked SignalR payload: {@payload}", restockPayload);
            try
            {
                await _hub.Clients.All.SendAsync("BookRestocked", restockPayload);
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "Failed sending BookRestocked SignalR message");
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Book restocked.", bookId = id, newQty = book.StockQuantity });
            }

            TempData["Success"] = $"Book '{book.Title}' restocked by {amount}.";
            return RedirectToAction("LowStock");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return Json(new { success = false, message = "Order not found." });

                TempData["Error"] = "Order not found.";
                return RedirectToAction("ManageOrders");
            }
            await _orderService.DeleteOrderAsync(id);
            TempData["Success"] = $"Order #{id} deleted successfully.";

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Order deleted.", orderId = id });
            }

            return RedirectToAction("ManageOrders");
        }

        [HttpGet]
        public async Task<IActionResult> Reports()
        {
            var orders = (await _orderService.GetAllOrdersAsync()).ToList();

            var totalOrders = orders.Count;
            var totalRevenue = orders.Sum(o => o.TotalAmount);
            var totalCustomers = orders
                .Select(o => (o.CustomerEmail ?? string.Empty).Trim().ToLowerInvariant())
                .Where(e => !string.IsNullOrEmpty(e))
                .Distinct()
                .Count();

            var now = DateTime.UtcNow;

            // Monthly: last 12 months
            var monthStart = new DateTime(now.Year, now.Month, 1).AddMonths(-11);
            var monthlyLabels = new List<string>();
            var monthlyRevenue = new List<decimal>();
            var monthlyOrders = new List<int>();
            for (int i = 0; i < 12; i++)
            {
                var start = monthStart.AddMonths(i);
                var end = start.AddMonths(1);
                var bucket = orders.Where(o => o.OrderDate >= start && o.OrderDate < end).ToList();
                monthlyLabels.Add(start.ToString("MMM yyyy"));
                monthlyRevenue.Add(bucket.Sum(o => o.TotalAmount));
                monthlyOrders.Add(bucket.Count);
            }

            // Daily: last 30 days (including today)
            var dailyStart = now.Date.AddDays(-29);
            var dailyLabels = new List<string>();
            var dailyRevenue = new List<decimal>();
            var dailyOrders = new List<int>();
            for (int d = 0; d < 30; d++)
            {
                var day = dailyStart.AddDays(d);
                var nextDay = day.AddDays(1);
                var bucket = orders.Where(o => o.OrderDate >= day && o.OrderDate < nextDay).ToList();
                dailyLabels.Add(day.ToString("dd MMM"));
                dailyRevenue.Add(bucket.Sum(o => o.TotalAmount));
                dailyOrders.Add(bucket.Count);
            }

            var vm = new ReportsVM
            {
                TotalOrders = totalOrders,
                TotalRevenue = totalRevenue,
                TotalCustomers = totalCustomers,
                RevenueLabels = monthlyLabels,
                RevenueValues = monthlyRevenue,
                OrdersPerMonth = monthlyOrders,
                DailyLabels = dailyLabels,
                DailyRevenue = dailyRevenue,
                DailyOrders = dailyOrders
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            await HttpContext.SignOutAsync();
            return Redirect("~/");
        }
    }
}