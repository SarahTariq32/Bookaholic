//using System;
//using System.Collections.Generic;
//using Web_Project.Models;
//using Web_Project.Models.Interfaces;

//namespace Web_Project.Services
//{
//    public class OrderService : IOrderService
//    {
//        private readonly IOrderRepository _orderRepository;
//        private readonly IOrderDetailRepository _orderDetailRepository;
//        private readonly ICartRepository _cartRepository;
//        private readonly IBookRepository _bookRepository;

//        public OrderService(
//            IOrderRepository orderRepository,
//            IOrderDetailRepository orderDetailRepository,
//            ICartRepository cartRepository,
//            IBookRepository bookRepository)
//        {
//            _orderRepository = orderRepository;
//            _orderDetailRepository = orderDetailRepository;
//            _cartRepository = cartRepository;
//            _bookRepository = bookRepository;
//        }

//        public int PlaceOrder(string userId, string customerName, string email, string phone, string address)
//        {
//            var cartItems = _cartRepository.GetCartItems(userId);
//            if (cartItems == null || cartItems.Count() == 0)
//                throw new Exception("Cart is empty.");

//            decimal totalAmount = 0;

//            foreach (var item in cartItems)
//            {
//                var book = _bookRepository.GetBookById(item.BookID);
//                if (book == null) throw new Exception($"Book with ID {item.BookID} not found.");
//                if (book.StockQuantity < item.Quantity) throw new Exception($"Not enough stock for {book.Title}.");

//                totalAmount += book.Price * item.Quantity;
//            }
//            var order = new Order
//            {
//                CustomerName = customerName,
//                CustomerEmail = email,
//                CustomerPhone = phone,
//                Address = address,
//                OrderDate = DateTime.Now,
//                TotalAmount = totalAmount,
//                Status = "Pending",
//                OrderDetails = new List<OrderDetail>()
//            };

//            int orderId = _orderRepository.CreateOrder(order);

//            foreach (var item in cartItems)
//            {
//                var book = _bookRepository.GetBookById(item.BookID);
//                var detail = new OrderDetail
//                {
//                    OrderID = orderId,
//                    BookID = item.BookID,
//                    Quantity = item.Quantity,
//                    PriceAtPurchase = book.Price
//                };
//                _orderDetailRepository.AddOrderDetail(detail);
//                book.StockQuantity -= item.Quantity;
//                _bookRepository.UpdateBook(book);
//            }
//            _cartRepository.ClearCart(userId);

//            return orderId;
//        }

//        public Order GetOrderById(int orderId)
//        {
//            return _orderRepository.GetOrderById(orderId);
//        }

//        public IEnumerable<Order> GetAllOrders()
//        {
//            return _orderRepository.GetAllOrders();
//        }

//        public void UpdateOrderStatus(int orderId, string status)
//        {
//            _orderRepository.UpdateOrderStatus(orderId, status);
//        }

//        public void DeleteOrder(int orderId)
//        {
//            _orderRepository.DeleteOrder(orderId);
//        }

//        public IEnumerable<OrderDetail> GetOrderDetails(int orderId)
//        {
//            return _orderDetailRepository.GetOrderDetailsByOrderId(orderId);
//        }
//    }
//}
using Web_Project.Models;
using Web_Project.Models.Interfaces;

namespace Web_Project.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IBookRepository _bookRepository;

        public OrderService(
            IOrderRepository orderRepository,
            IOrderDetailRepository orderDetailRepository,
            IBookRepository bookRepository)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _bookRepository = bookRepository;
        }

        public async Task<int> PlaceOrderAsync(
            IEnumerable<Cartitem> cartItems,
            string customerName,
            string email,
            string phone,
            string address)
        {
            if (cartItems == null || !cartItems.Any())
                throw new Exception("Cart is empty.");

            decimal totalAmount = 0;
            foreach (var item in cartItems)
            {
                var book = await _bookRepository.GetBookByIdAsync(item.BookID);
                if (book == null)
                    throw new Exception($"Book {item.BookID} not found.");

                if (book.StockQuantity < item.Quantity)
                    throw new Exception($"Not enough stock for {book.Title}.");

                totalAmount += book.Price * item.Quantity;
            }
            var newOrder = new Order
            {
                CustomerName = customerName,
                CustomerEmail = email,
                CustomerPhone = phone,
                Address = address,
                OrderDate = DateTime.Now,
                TotalAmount = totalAmount,
                Status = "Pending"
            };

            int orderId = await _orderRepository.CreateOrderAsync(newOrder);
            foreach (var item in cartItems)
            {
                var book = await _bookRepository.GetBookByIdAsync(item.BookID);

                var detail = new OrderDetail
                {
                    OrderID = orderId,
                    BookID = item.BookID,
                    Quantity = item.Quantity,
                    PriceAtPurchase = book.Price
                };

                await _orderDetailRepository.AddOrderDetailAsync(detail);
                book.StockQuantity -= item.Quantity;
                await _bookRepository.UpdateBookAsync(book);
            }

            return orderId;
        }

        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await _orderRepository.GetOrderByIdAsync(orderId);
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllOrdersAsync();
        }

        public async Task UpdateOrderStatusAsync(int orderId, string status)
        {
            await _orderRepository.UpdateOrderStatusAsync(orderId, status);
        }

        public async Task DeleteOrderAsync(int orderId)
        {
            await _orderRepository.DeleteOrderAsync(orderId);
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsAsync(int orderId)
        {
            return await _orderDetailRepository.GetOrderDetailsByOrderIdAsync(orderId);
        }

        public Task<int> CountOrdersAsync() => _orderRepository.CountOrdersAsync();
        public Task<int> CountDeliveredAsync() => _orderRepository.CountDeliveredAsync();
    }
}
