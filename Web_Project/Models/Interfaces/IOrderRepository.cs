//namespace Web_Project.Models.Interfaces
//{
//    public interface IOrderRepository
//    {
//        int CreateOrder(Order order);
//        Order GetOrderById(int id);
//        IEnumerable<Order> GetAllOrders();
//        IEnumerable<Order> GetOrdersByUser(string userId);
//        void UpdateOrderStatus(int orderId, string status);
//        void DeleteOrder(int orderId);
//    }
//}

using Web_Project.Models;

namespace Web_Project.Models.Interfaces
{
    public interface IOrderRepository
    {
        Task<int> CreateOrderAsync(Order order);
        Task<Order?> GetOrderByIdAsync(int id);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<IEnumerable<Order>> GetOrdersByUserAsync(string email);
        Task UpdateOrderStatusAsync(int orderId, string status);
        Task DeleteOrderAsync(int orderId);
        Task<int> CountOrdersAsync();
        Task<int> CountDeliveredAsync();
    }
}
