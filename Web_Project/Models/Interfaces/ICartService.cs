using System.Collections.Generic;
using Web_Project.Models;

namespace Web_Project.Models.Interfaces
{
    public interface ICartService
    {
        IEnumerable<Cartitem> GetCartItems(string userId);
        void AddToCart(string userId, int bookId, int quantity);
        void UpdateCartItem(int cartId, int quantity);
        void RemoveFromCart(int cartId);
        void ClearCart(string userId);
        decimal GetCartTotal(string userId);
    }
}
