using System;
using System.Collections.Generic;
using Web_Project.Models;
using Web_Project.Models.Interfaces;

namespace Web_Project.Services
{
    public class CartService: ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IBookRepository _bookRepository;

        public CartService(ICartRepository cartRepository, IBookRepository bookRepository)
        {
            _cartRepository = cartRepository;
            _bookRepository = bookRepository;
        }

        public IEnumerable<Cartitem> GetCartItems(string userId)
        {
            return _cartRepository.GetCartItems(userId);
        }

        public void AddToCart(string userId, int bookId, int quantity)
        {
            var book = _bookRepository.GetBookById(bookId);
            if (book == null) throw new Exception("Book not found.");
            if (book.StockQuantity < quantity) throw new Exception("Not enough stock.");

            _cartRepository.AddToCart(userId, bookId, quantity);
        }

        public void UpdateCartItem(int cartId, int quantity)
        {
            _cartRepository.UpdateCartItem(cartId, quantity);
        }

        public void RemoveFromCart(int cartId)
        {
            _cartRepository.RemoveFromCart(cartId);
        }

        public void ClearCart(string userId)
        {
            _cartRepository.ClearCart(userId);
        }

        public decimal GetCartTotal(string userId)
        {
            decimal total = 0;
            var items = _cartRepository.GetCartItems(userId);
            foreach (var item in items)
            {
                var book = _bookRepository.GetBookById(item.BookID);
                if (book != null)
                {
                    total += book.Price * item.Quantity;
                }
            }
            return total;
        }
    }
}
