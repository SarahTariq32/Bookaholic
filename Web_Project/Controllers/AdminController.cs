//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using System.Collections.Generic;
//using Web_Project.Models;

//namespace Web_Project.Controllers
//{
//    [Authorize(Policy = "AdminAccess")]
//    public class AdminController : Controller
//    {
//        public IActionResult Dashboard()
//        {
//            return View();
//        }

//        public IActionResult AddEditBook()
//        {
//            var books = new List<Book>
//            {
//                new Book { BookID = 1, Title = "The Cruel Prince", Author = "H. Author", CategoryID = 1, Price = 1450 },
//                new Book { BookID = 2, Title = "Powerless", Author = "A. Author", CategoryID = 1, Price = 1200 },
//                new Book { BookID = 3, Title = "Ignite Me", Author = "S. Author", CategoryID = 1, Price = 1350 },
//                new Book { BookID = 4, Title = "Once Upon a Broken Heart", Author = "C. Author", CategoryID = 1, Price = 1500 },
//                new Book { BookID = 5, Title = "Heart of the Raven Prince", Author = "M. Author", CategoryID = 1, Price = 1400 },
//                new Book { BookID = 6, Title = "The Housemaid", Author = "T. Author", CategoryID = 2, Price = 1350 },
//                new Book { BookID = 7, Title = "Silent Patient", Author = "A. Smith", CategoryID = 2, Price = 1400 },
//                new Book { BookID = 8, Title = "The Teacher", Author = "B. Brown", CategoryID = 2, Price = 1250 },
//                new Book { BookID = 9, Title = "The Locked Door", Author = "L. James", CategoryID = 2, Price = 1300 },
//                new Book { BookID = 10, Title = "Not Quite Dead Yet", Author = "R. Taylor", CategoryID = 2, Price = 1300 }
//            };

//            return View(books);
//        }

//        [HttpPost]
//        public IActionResult AddEditBook(Book book)
//        {
//            return RedirectToAction("AddEditBook");
//        }

//        public IActionResult AddBook()
//        {
//            return View();
//        }

//        public IActionResult EditBook(int id)
//        {
//            return View();
//        }

//        public IActionResult ManageCategories()
//        {
//            return View();
//        }

//        public IActionResult ManageOrders()
//        {
//            return View();
//        }

//        public IActionResult Reports()
//        {
//            return View();
//        }

//        public IActionResult Settings()
//        {
//            return View();
//        }

//        [HttpPost]
//        public IActionResult UpdateSettings(string Email, string Password, string MaintenanceMode)
//        {
//            TempData["Message"] = "Settings saved successfully!";
//            return RedirectToAction("Settings");
//        }
//    }
//}
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using System.Threading.Tasks;
//using Web_Project.Models;
//using Web_Project.Models.Interfaces;

//namespace Web_Project.Controllers
//{
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

//        // ===== DASHBOARD =====
//        public async Task<IActionResult> Dashboard()
//        {
//            var totalBooks = (await Task.Run(() => _bookService.GetAllBooksAsync())).Count();
//            var totalOrders = (await Task.Run(() => _orderService.GetAllOrdersAsync())).Count();
//            var totalDelivered = (await Task.Run(() => _orderService.GetAllOrdersAsync()))
//                                 .Count(o => o.Status == "Delivered");

//            ViewData["TotalBooks"] = totalBooks;
//            ViewData["TotalOrders"] = totalOrders;
//            ViewData["TotalDelivered"] = totalDelivered;

//            return View();
//        }

//        // ===== BOOKS =====
//        public async Task<IActionResult> Books()
//        {
//            var books = await Task.Run(() => _bookService.GetAllBooksAsync());
//            return View(books);
//        }

//        public IActionResult AddBook()
//        {
//            var categories = _categoryService.GetAllCategories();
//            ViewData["Categories"] = categories;
//            return View();
//        }

//        [HttpPost]
//        public async Task<IActionResult> AddBook(Book book)
//        {
//            if (ModelState.IsValid)
//            {
//                await Task.Run(() => _bookService.AddBookAsync(book));
//                return RedirectToAction(nameof(Books));
//            }
//            ViewData["Categories"] = _categoryService.GetAllCategories();
//            return View(book);
//        }

//        public async Task<IActionResult> EditBook(int id)
//        {
//            var book = await Task.Run(() => _bookService.GetBookByIdAsync(id));
//            if (book == null) return NotFound();

//            ViewData["Categories"] = _categoryService.GetAllCategories();
//            return View(book);
//        }

//        [HttpPost]
//        public async Task<IActionResult> EditBook(Book book)
//        {
//            if (ModelState.IsValid)
//            {
//                await Task.Run(() => _bookService.UpdateBookAsync(book));
//                return RedirectToAction(nameof(Books));
//            }
//            ViewData["Categories"] = _categoryService.GetAllCategories();
//            return View(book);
//        }

//        public async Task<IActionResult> DeleteBook(int id)
//        {
//            await Task.Run(() => _bookService.DeleteBookAsync(id));
//            return RedirectToAction(nameof(Books));
//        }

//        // ===== CATEGORIES =====
//        public IActionResult Categories()
//        {
//            var categories = _categoryService.GetAllCategories();
//            return View(categories);
//        }

//        [HttpPost]
//        public IActionResult AddCategory(Category category)
//        {
//            if (ModelState.IsValid)
//            {
//                _categoryService.AddCategory(category);
//            }
//            return RedirectToAction(nameof(Categories));
//        }

//        public IActionResult DeleteCategory(int id)
//        {
//            _categoryService.DeleteCategory(id);
//            return RedirectToAction(nameof(Categories));
//        }

//        // ===== ORDERS =====
//        public async Task<IActionResult> Orders()
//        {
//            var orders = await Task.Run(() => _orderService.GetAllOrdersAsync());
//            return View(orders);
//        }

//        public IActionResult OrderDetails(int id)
//        {
//            var order = _orderService.GetOrderByIdAsync(id);
//            if (order == null) return NotFound();

//            var details = _orderService.GetOrderDetailsAsync(id);
//            ViewData["OrderDetails"] = details;
//            return View(order);
//        }

//        [HttpPost]
//        public IActionResult UpdateOrderStatus(int orderId)
//        {
//            var order = _orderService.GetOrderByIdAsync(orderId);
//            string status = "Delivered";
//            if (order != null && order.Status != status)
//            {
//                _orderService.UpdateOrderStatusAsync(orderId, "Delivered");
//            }
//            return RedirectToAction(nameof(Orders));
//        }
//    }
//}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web_Project.Models;
using Web_Project.Models.Interfaces;

namespace Web_Project.Controllers
{
    public class OrderDetailsVM
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public string Status { get; set; }

        public List<OrderItemVM> Items { get; set; }
    }

    public class OrderItemVM
    {
        public string BookTitle { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
    [Authorize(Policy = "AdminAccess")]
    public class AdminController : Controller
    {
        private readonly IBookService _bookService;
        private readonly ICategoryService _categoryService;
        private readonly IOrderService _orderService;

        public AdminController(
            IBookService bookService,
            ICategoryService categoryService,
            IOrderService orderService)
        {
            _bookService = bookService;
            _categoryService = categoryService;
            _orderService = orderService;
        }

        // -------------------- Dashboard --------------------
        public async Task<IActionResult> Dashboard()
        {
            var stats = new
            {
                TotalBooks = await _bookService.CountBooksAsync(),
                TotalOrders = await _orderService.CountOrdersAsync(),
                Delivered = await _orderService.CountDeliveredAsync()
            };

            return View(stats);
        }

        // -------------------- BOOKS --------------------
        public async Task<IActionResult> ManageBooks()
        {
            var books = await _bookService.GetAllBooksAsync();
            return View(books);
        }

        public IActionResult AddBook()
        {
            ViewBag.Categories = _categoryService.GetAllCategories();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddBook(Book model)
        {
            await _bookService.AddBookAsync(model);
            return RedirectToAction("ManageBooks");
        }

        public async Task<IActionResult> EditBook(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            ViewBag.Categories = _categoryService.GetAllCategories();
            return View(book);
        }

        [HttpPost]
        public async Task<IActionResult> EditBook(Book model)
        {
            await _bookService.UpdateBookAsync(model);
            return RedirectToAction("ManageBooks");
        }

        public async Task<IActionResult> DeleteBook(int id)
        {
            await _bookService.DeleteBookAsync(id);
            return RedirectToAction("ManageBooks");
        }

        // -------------------- CATEGORIES --------------------
        public IActionResult ManageCategories()
        {
            var categories = _categoryService.GetAllCategories();
            return View(categories);
        }

        [HttpPost]
        public IActionResult AddCategory(Category model)
        {
            _categoryService.AddCategory(model);
            return RedirectToAction("ManageCategories");
        }

        public IActionResult DeleteCategory(int id)
        {
            _categoryService.DeleteCategory(id);
            return RedirectToAction("ManageCategories");
        }

        // -------------------- ORDERS --------------------
        public async Task<IActionResult> ManageOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return View(orders);
        }

        public async Task<IActionResult> OrderDetails(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound();

            var vm = new OrderDetailsVM
            {
                OrderId = order.Id,
                CustomerName = order.CustomerName,
                Status = order.Status,
                Items = new List<OrderItemVM>()
            };

            foreach (var d in order.OrderDetails)
            {
                var book = await _bookService.GetBookByIdAsync(d.BookID);

                vm.Items.Add(new OrderItemVM
                {
                    BookTitle = book?.Title ?? "Unknown Book",
                    Quantity = d.Quantity,
                    Price = d.PriceAtPurchase
                });
            }

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(int id, string status)
        {
            await _orderService.UpdateOrderStatusAsync(id, status);
            return RedirectToAction("ManageOrders");
        }

        public async Task<IActionResult> DeleteOrder(int id)
        {
            await _orderService.DeleteOrderAsync(id);
            return RedirectToAction("ManageOrders");
        }
    }
}
