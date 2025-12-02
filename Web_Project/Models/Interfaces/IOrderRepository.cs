namespace Web_Project.Models.Interfaces
{
    public interface IOrderRepository
    {
        int CreateOrder(Order order);
        Order GetOrderById(int id);
        IEnumerable<Order> GetAllOrders();
        IEnumerable<Order> GetOrdersByUser(string userId);
        void UpdateOrderStatus(int orderId, string status);
        void DeleteOrder(int orderId);
    }
}
