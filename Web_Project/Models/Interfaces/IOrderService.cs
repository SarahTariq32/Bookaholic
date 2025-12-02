using System.Collections.Generic;
using Web_Project.Models;

namespace Web_Project.Models.Interfaces
{
    public interface IOrderService
    {
        int PlaceOrder(string userId, string customerName, string email, string phone, string address);
        Order GetOrderById(int orderId);
        IEnumerable<Order> GetAllOrders();
        void UpdateOrderStatus(int orderId, string status);
        void DeleteOrder(int orderId);
        IEnumerable<OrderDetail> GetOrderDetails(int orderId);
    }
}
