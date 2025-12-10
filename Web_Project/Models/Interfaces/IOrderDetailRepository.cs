using System.Collections.Generic;
using System.Threading.Tasks;

namespace Web_Project.Models.Interfaces
{
    public interface IOrderDetailRepository
    {
        Task AddOrderDetailAsync(OrderDetail detail);
        Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId);
    }
}
