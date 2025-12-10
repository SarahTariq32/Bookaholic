using System.Collections.Generic;
using System.Threading.Tasks;
using Web_Project.Models;
using Web_Project.Models.Interfaces;
using Web_Project.Services.Interfaces;

namespace Web_Project.Services
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IOrderDetailRepository _repository;

        public OrderDetailService(IOrderDetailRepository repository)
        {
            _repository = repository;
        }

        public async Task AddOrderDetailAsync(OrderDetail detail)
        {
            await _repository.AddOrderDetailAsync(detail);
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId)
        {
            return await _repository.GetOrderDetailsByOrderIdAsync(orderId);
        }
    }
}
