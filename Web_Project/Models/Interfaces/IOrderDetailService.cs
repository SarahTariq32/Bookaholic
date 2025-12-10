using System.Collections.Generic;
using System.Threading.Tasks;
using Web_Project.Models;

namespace Web_Project.Services.Interfaces
{
    public interface IOrderDetailService
    {
        Task AddOrderDetailAsync(OrderDetail detail);
        Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId);
    }
}
