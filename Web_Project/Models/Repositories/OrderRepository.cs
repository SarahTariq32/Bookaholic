using System;
using System.Collections.Generic;
using System.Linq;
using Web_Project.Data;
using Web_Project.Models;
using Web_Project.Models.Interfaces;

namespace Web_Project.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public int CreateOrder(Order order)
        {
            try
            {
                _context.Orders.Add(order);
                _context.SaveChanges();
                return order.Id; 
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating order", ex);
            }
        }

        public Order GetOrderById(int id)
        {
            try
            {
                return _context.Orders.FirstOrDefault(o => o.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching order with ID {id}", ex);
            }
        }

        public IEnumerable<Order> GetAllOrders()
        {
            try
            {
                return _context.Orders.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching all orders", ex);
            }
        }

        public IEnumerable<Order> GetOrdersByUser(string userId)
        {
            try
            {
                return _context.Orders
                               .Where(o => o.CustomerEmail == userId)
                               .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching orders for user {userId}", ex);
            }
        }

        public void UpdateOrderStatus(int orderId, string status)
        {
            try
            {
                var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);
                if (order != null)
                {
                    order.Status = status;
                    _context.Orders.Update(order);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating order status for order ID {orderId}", ex);
            }
        }

        public void DeleteOrder(int orderId)
        {
            try
            {
                var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);
                if (order != null)
                {
                    _context.Orders.Remove(order);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting order with ID {orderId}", ex);
            }
        }
    }
}
