using System;
using System.Collections.Generic;
using System.Linq;
using Web_Project.Data;
using Web_Project.Models.Interfaces;
using Web_Project.Models;

namespace Web_Project.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Cartitem> GetCartItems(string userId)
        {
            try
            {
                return _context.Cartitems
                               .Where(c => c.UserID == userId)
                               .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching cart items for user {userId}", ex);
            }
        }

        public void AddToCart(string userId, int bookId, int quantity)
        {
            try
            {
                var existingItem = _context.Cartitems
                                           .FirstOrDefault(c => c.UserID == userId && c.BookID == bookId);
                if (existingItem != null)
                {
                    existingItem.Quantity += quantity;
                    _context.Cartitems.Update(existingItem);
                }
                else
                {
                    var cartItem = new Cartitem
                    {
                        UserID = userId,
                        BookID = bookId,
                        Quantity = quantity
                    };
                    _context.Cartitems.Add(cartItem);
                }
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding book {bookId} to cart for user {userId}", ex);
            }
        }

        public void UpdateCartItem(int cartId, int quantity)
        {
            try
            {
                var cartItem = _context.Cartitems.FirstOrDefault(c => c.CartID == cartId);
                if (cartItem != null)
                {
                    cartItem.Quantity = quantity;
                    _context.Cartitems.Update(cartItem);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating cart item {cartId}", ex);
            }
        }

        public void RemoveFromCart(int cartId)
        {
            try
            {
                var cartItem = _context.Cartitems.FirstOrDefault(c => c.CartID == cartId);
                if (cartItem != null)
                {
                    _context.Cartitems.Remove(cartItem);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error removing cart item {cartId}", ex);
            }
        }

        public void ClearCart(string userId)
        {
            try
            {
                var cartItems = _context.Cartitems.Where(c => c.UserID == userId).ToList();
                if (cartItems.Any())
                {
                    _context.Cartitems.RemoveRange(cartItems);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error clearing cart for user {userId}", ex);
            }
        }
    }
}
