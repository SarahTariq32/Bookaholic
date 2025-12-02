using System;
using System.Collections.Generic;
using System.Linq;
using Web_Project.Data;
using Web_Project.Models;
using Web_Project.Models.Interfaces;

namespace Web_Project.Repository
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderDetailRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddOrderDetail(OrderDetail detail)
        {
            try
            {
                _context.OrderDetails.Add(detail);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding order detail", ex);
            }
        }

        public IEnumerable<OrderDetail> GetOrderDetailsByOrderId(int orderId)
        {
            try
            {
                return _context.OrderDetails
                               .Where(od => od.OrderID == orderId)
                               .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching order details for order ID {orderId}", ex);
            }
        }
    }
}
