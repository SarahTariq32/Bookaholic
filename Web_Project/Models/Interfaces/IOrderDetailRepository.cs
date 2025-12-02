namespace Web_Project.Models.Interfaces
{
    public interface IOrderDetailRepository
    {
        void AddOrderDetail(OrderDetail detail);
        IEnumerable<OrderDetail> GetOrderDetailsByOrderId(int orderId);
    }
}
