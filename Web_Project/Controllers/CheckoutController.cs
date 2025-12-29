
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.SignalR;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Web_Project.Data;
//using Web_Project.Helpers;
//using Web_Project.Hubs;
//using Web_Project.Models;
//using Web_Project.Models.ViewModels;

//namespace Web_Project.Controllers
//{
//    [Authorize(Policy = "UserAccess")]
//    public class CheckoutController : Controller
//    {
//        private const string SessionCartKey = "Cart";
//        private readonly ApplicationDbContext _db;
//        private readonly ILogger<CheckoutController> _logger;
//        private readonly IHubContext<OrderHub> _hub;
//        private const decimal DefaultShipping = 200m;

//        private class SessionCartItem
//        {
//            public int BookID { get; set; }
//            public int Quantity { get; set; }
//        }

//        public CheckoutController(ApplicationDbContext db, ILogger<CheckoutController> logger, IHubContext<OrderHub> hub)
//        {
//            _db = db ?? throw new ArgumentNullException(nameof(db));
//            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
//            _hub = hub ?? throw new ArgumentNullException(nameof(hub));
//        }

//        // GET: /Checkout
//        public async Task<IActionResult> Index()
//        {
//            var vm = await BuildCheckoutVmFromSession();
//            return View(vm);
//        }

//        // POST: /Checkout/PlaceOrder
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> PlaceOrder(CheckoutVM model)
//        {
//            if (!ModelState.IsValid)
//            {

//                var vm = await BuildCheckoutVmFromSession();
//                vm.CustomerName = model.CustomerName;
//                vm.CustomerEmail = model.CustomerEmail;
//                vm.CustomerPhone = model.CustomerPhone;
//                vm.Address = model.Address;
//                vm.City = model.City;
//                vm.Zip = model.Zip;
//                vm.PaymentMethod = model.PaymentMethod;
//                return View("Index", vm);
//            }

//            var cart = HttpContext.Session.GetObject<List<SessionCartItem>>(SessionCartKey) ?? new List<SessionCartItem>();
//            if (!cart.Any())
//            {
//                TempData["Error"] = "Your cart is empty.";
//                return RedirectToAction("Index", "Cart");
//            }

//            var order = new Order
//            {
//                CustomerName = model.CustomerName,
//                CustomerEmail = model.CustomerEmail,
//                CustomerPhone = model.CustomerPhone,
//                Address = $"{model.Address} {model.City} {model.Zip}".Trim(),
//                OrderDate = DateTime.UtcNow,
//                Status = "Pending",
//                OrderDetails = new List<OrderDetail>(),
//                PaymentMethod = model.PaymentMethod
//            };

//            decimal subtotal = 0m;
//            foreach (var sc in cart)
//            {
//                var book = await _db.Books.FindAsync(sc.BookID);
//                if (book == null) continue;

//                var qty = Math.Max(1, sc.Quantity);
//                var lineTotal = book.Price * qty;
//                subtotal += lineTotal;

//                order.OrderDetails.Add(new OrderDetail
//                {
//                    BookID = book.BookID,
//                    Quantity = qty,
//                    PriceAtPurchase = book.Price
//                });
//            }

//            order.TotalAmount = subtotal + DefaultShipping;

//            _db.Orders.Add(order);
//            await _db.SaveChangesAsync();

//            try
//            {
//                await _hub.Clients.Group("admins").SendAsync("NewOrder", new
//                {
//                    orderId = order.Id,
//                    customer = order.CustomerName,
//                    total = order.TotalAmount
//                });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Failed to send order notification via SignalR.");
//            }
//            HttpContext.Session.Remove(SessionCartKey);

//            return RedirectToAction(nameof(Confirmation), new { id = order.Id });
//        }
//        public async Task<IActionResult> Confirmation(int id)
//        {
//            var order = await _db.Orders
//                .Include(o => o.OrderDetails)
//                .FirstOrDefaultAsync(o => o.Id == id);

//            if (order == null) return NotFound();

//            var vm = new Web_Project.Models.ViewModels.OrderDetailsVM
//            {
//                OrderId = order.Id,
//                CustomerName = order.CustomerName,
//                Status = order.Status,
//                TotalAmount = order.TotalAmount,
//                ShippingFee = DefaultShipping,
//                EstimatedDelivery = DateTime.UtcNow.AddDays(3),
//                PaymentMethod = string.IsNullOrWhiteSpace(order.PaymentMethod) ? "N/A" : order.PaymentMethod // ensure UI-friendly

//            };

//            foreach (var d in order.OrderDetails)
//            {
//                var book = await _db.Books.FindAsync(d.BookID);
//                vm.Items.Add(new Web_Project.Models.ViewModels.OrderItemVM
//                {
//                    BookTitle = book?.Title ?? "Unknown",
//                    Quantity = d.Quantity,
//                    Price = d.PriceAtPurchase
//                });
//            }

//            return View("OrderConfirmation", vm);
//        }
//        private async Task<CheckoutVM> BuildCheckoutVmFromSession()
//        {
//            var vm = new CheckoutVM();
//            var cart = HttpContext.Session.GetObject<List<SessionCartItem>>(SessionCartKey) ?? new List<SessionCartItem>();

//            foreach (var sc in cart)
//            {
//                var book = await _db.Books.FindAsync(sc.BookID);
//                if (book == null) continue;

//                vm.Items.Add(new CartItemVM
//                {
//                    BookID = book.BookID,
//                    Title = book.Title,
//                    CoverImage = book.CoverImage ?? string.Empty,
//                    Price = book.Price,
//                    Quantity = sc.Quantity
//                });
//            }

//            vm.ShippingFee = DefaultShipping;
//            vm.Subtotal = vm.Items.Sum(i => i.LineTotal);
//            vm.Total = vm.Subtotal + vm.ShippingFee;
//            return vm;
//        }
//    }
//}



using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_Project.Data;
using Web_Project.Helpers;
using Web_Project.Hubs;
using Web_Project.Models;
using Web_Project.Models.ViewModels;

namespace Web_Project.Controllers
{
    [Authorize(Policy = "UserAccess")]
    public class CheckoutController : Controller
    {
        private const string SessionCartKey = "Cart";
        private readonly ApplicationDbContext _db;
        private readonly ILogger<CheckoutController> _logger;
        private readonly IHubContext<OrderHub> _hub;
        private const decimal DefaultShipping = 200m;

        private class SessionCartItem
        {
            public int BookID { get; set; }
            public int Quantity { get; set; }
        }

        public CheckoutController(ApplicationDbContext db, ILogger<CheckoutController> logger, IHubContext<OrderHub> hub)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _hub = hub ?? throw new ArgumentNullException(nameof(hub));
        }

        // GET: /Checkout
        public async Task<IActionResult> Index()
        {
            var vm = await BuildCheckoutVmFromSession();
            return View(vm);
        }

        // POST: /Checkout/PlaceOrder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder(CheckoutVM model)
        {
            if (!ModelState.IsValid)
            {
                var vmInvalid = await BuildCheckoutVmFromSession();
                vmInvalid.CustomerName = model.CustomerName;
                vmInvalid.CustomerEmail = model.CustomerEmail;
                vmInvalid.CustomerPhone = model.CustomerPhone;
                vmInvalid.Address = model.Address;
                vmInvalid.City = model.City;
                vmInvalid.Zip = model.Zip;
                vmInvalid.PaymentMethod = model.PaymentMethod;
                return View("Index", vmInvalid);
            }

            var cart = HttpContext.Session.GetObject<List<SessionCartItem>>(SessionCartKey) ?? new List<SessionCartItem>();
            if (!cart.Any())
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("Index", "Cart");
            }

            // Validate stock and reserve items in a transaction to avoid oversell.
            using var tx = await _db.Database.BeginTransactionAsync();
            try
            {
                // Load books involved and check availability
                var bookIds = cart.Select(c => c.BookID).Distinct().ToList();
                var books = await _db.Books.Where(b => bookIds.Contains(b.BookID)).ToDictionaryAsync(b => b.BookID);

                foreach (var sc in cart)
                {
                    if (!books.TryGetValue(sc.BookID, out var book))
                    {
                        ModelState.AddModelError("", $"Book (id:{sc.BookID}) was not found.");
                        break;
                    }

                    var qty = Math.Max(1, sc.Quantity);

                    // If not enough stock, return to checkout with error
                    if (book.StockQuantity < qty)
                    {
                        ModelState.AddModelError("", $"Not enough stock for \"{book.Title}\" — available: {book.StockQuantity}, requested: {qty}.");
                    }
                }

                if (!ModelState.IsValid)
                {
                    // Build VM to return to view with errors
                    var vmInvalid = await BuildCheckoutVmFromSession();
                    vmInvalid.CustomerName = model.CustomerName;
                    vmInvalid.CustomerEmail = model.CustomerEmail;
                    vmInvalid.CustomerPhone = model.CustomerPhone;
                    vmInvalid.Address = model.Address;
                    vmInvalid.City = model.City;
                    vmInvalid.Zip = model.Zip;
                    vmInvalid.PaymentMethod = model.PaymentMethod;
                    return View("Index", vmInvalid);
                }

                // All good: create order and decrement stock (reserve)
                var order = new Order
                {
                    CustomerName = model.CustomerName,
                    CustomerEmail = model.CustomerEmail,
                    CustomerPhone = model.CustomerPhone,
                    Address = $"{model.Address} {model.City} {model.Zip}".Trim(),
                    OrderDate = DateTime.UtcNow,
                    Status = "Pending",
                    OrderDetails = new List<OrderDetail>(),
                    PaymentMethod = model.PaymentMethod
                };

                decimal subtotal = 0m;
                var updatedBooks = new List<Book>(); // track books whose stock changed

                foreach (var sc in cart)
                {
                    var book = books[sc.BookID];
                    var qty = Math.Max(1, sc.Quantity);
                    var lineTotal = book.Price * qty;
                    subtotal += lineTotal;

                    order.OrderDetails.Add(new OrderDetail
                    {
                        BookID = book.BookID,
                        Quantity = qty,
                        PriceAtPurchase = book.Price
                    });

                    // Reserve / decrement stock immediately so other users see accurate availability
                    book.StockQuantity = Math.Max(0, book.StockQuantity - qty);
                    _db.Books.Update(book);

                    // remember to notify clients about this book later
                    updatedBooks.Add(book);
                }

                order.TotalAmount = subtotal + DefaultShipping;

                _db.Orders.Add(order);
                await _db.SaveChangesAsync();

                await tx.CommitAsync();

                // Notify admins about new order (existing behavior)
                try
                {
                    await _hub.Clients.Group("admins").SendAsync("NewOrder", new
                    {
                        orderId = order.Id,
                        customer = order.CustomerName,
                        total = order.TotalAmount
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to send order notification via SignalR.");
                }

                // Broadcast stock updates to clients so public pages (BookDetails) can update live
                try
                {
                    foreach (var b in updatedBooks)
                    {
                        // message shape: { bookId, newQty }
                        await _hub.Clients.All.SendAsync("StockUpdated", new { bookId = b.BookID, newQty = b.StockQuantity });
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to send stock update SignalR messages.");
                }

                HttpContext.Session.Remove(SessionCartKey);

                return RedirectToAction(nameof(Confirmation), new { id = order.Id });
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                _logger.LogError(ex, "Error placing order");

                TempData["Error"] = "An error occurred while placing your order. Please try again.";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Confirmation(int id)
        {
            var order = await _db.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return NotFound();

            var vm = new Web_Project.Models.ViewModels.OrderDetailsVM
            {
                OrderId = order.Id,
                CustomerName = order.CustomerName,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                ShippingFee = DefaultShipping,
                EstimatedDelivery = DateTime.UtcNow.AddDays(3),
                PaymentMethod = string.IsNullOrWhiteSpace(order.PaymentMethod) ? "N/A" : order.PaymentMethod // ensure UI-friendly

            };

            foreach (var d in order.OrderDetails)
            {
                var book = await _db.Books.FindAsync(d.BookID);
                vm.Items.Add(new Web_Project.Models.ViewModels.OrderItemVM
                {
                    BookTitle = book?.Title ?? "Unknown",
                    Quantity = d.Quantity,
                    Price = d.PriceAtPurchase
                });
            }

            return View("OrderConfirmation", vm);
        }

        private async Task<CheckoutVM> BuildCheckoutVmFromSession()
        {
            var vm = new CheckoutVM();
            var cart = HttpContext.Session.GetObject<List<SessionCartItem>>(SessionCartKey) ?? new List<SessionCartItem>();

            foreach (var sc in cart)
            {
                var book = await _db.Books.FindAsync(sc.BookID);
                if (book == null) continue;

                vm.Items.Add(new CartItemVM
                {
                    BookID = book.BookID,
                    Title = book.Title,
                    CoverImage = book.CoverImage ?? string.Empty,
                    Price = book.Price,
                    Quantity = sc.Quantity
                });
            }

            vm.ShippingFee = DefaultShipping;
            vm.Subtotal = vm.Items.Sum(i => i.LineTotal);
            vm.Total = vm.Subtotal + vm.ShippingFee;
            return vm;
        }
    }
}