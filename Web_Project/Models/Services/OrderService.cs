using System;
using System.Collections.Generic;
using Web_Project.Models;
using Web_Project.Models.Interfaces;

namespace Web_Project.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IBookRepository _bookRepository;

        public OrderService(
            IOrderRepository orderRepository,
            IOrderDetailRepository orderDetailRepository,
            ICartRepository cartRepository,
            IBookRepository bookRepository)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _cartRepository = cartRepository;
            _bookRepository = bookRepository;
        }

        public int PlaceOrder(string userId, string customerName, string email, string phone, string address)
        {
            var cartItems = _cartRepository.GetCartItems(userId);
            if (cartItems == null || cartItems.Count() == 0)
                throw new Exception("Cart is empty.");

            decimal totalAmount = 0;

            foreach (var item in cartItems)
            {
                var book = _bookRepository.GetBookById(item.BookID);
                if (book == null) throw new Exception($"Book with ID {item.BookID} not found.");
                if (book.StockQuantity < item.Quantity) throw new Exception($"Not enough stock for {book.Title}.");

                totalAmount += book.Price * item.Quantity;
            }
            var order = new Order
            {
                CustomerName = customerName,
                CustomerEmail = email,
                CustomerPhone = phone,
                Address = address,
                OrderDate = DateTime.Now,
                TotalAmount = totalAmount,
                Status = "Pending",
                OrderDetails = new List<OrderDetail>()
            };

            int orderId = _orderRepository.CreateOrder(order);

            foreach (var item in cartItems)
            {
                var book = _bookRepository.GetBookById(item.BookID);
                var detail = new OrderDetail
                {
                    OrderID = orderId,
                    BookID = item.BookID,
                    Quantity = item.Quantity,
                    PriceAtPurchase = book.Price
                };
                _orderDetailRepository.AddOrderDetail(detail);
                book.StockQuantity -= item.Quantity;
                _bookRepository.UpdateBook(book);
            }
            _cartRepository.ClearCart(userId);

            return orderId;
        }

        public Order GetOrderById(int orderId)
        {
            return _orderRepository.GetOrderById(orderId);
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return _orderRepository.GetAllOrders();
        }

        public void UpdateOrderStatus(int orderId, string status)
        {
            _orderRepository.UpdateOrderStatus(orderId, status);
        }

        public void DeleteOrder(int orderId)
        {
            _orderRepository.DeleteOrder(orderId);
        }

        public IEnumerable<OrderDetail> GetOrderDetails(int orderId)
        {
            return _orderDetailRepository.GetOrderDetailsByOrderId(orderId);
        }
    }
}
