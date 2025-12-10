//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Web_Project.Data;
//using Web_Project.Models;
//using Web_Project.Models.Interfaces;

//namespace Web_Project.Repository
//{
//    public class OrderRepository : IOrderRepository
//    {
//        private readonly ApplicationDbContext _context;

//        public OrderRepository(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        public int CreateOrder(Order order)
//        {
//            try
//            {
//                _context.Orders.Add(order);
//                _context.SaveChanges();
//                return order.Id; 
//            }
//            catch (Exception ex)
//            {
//                throw new Exception("Error creating order", ex);
//            }
//        }

//        public Order GetOrderById(int id)
//        {
//            try
//            {
//                return _context.Orders.FirstOrDefault(o => o.Id == id);
//            }
//            catch (Exception ex)
//            {
//                throw new Exception($"Error fetching order with ID {id}", ex);
//            }
//        }

//        public IEnumerable<Order> GetAllOrders()
//        {
//            try
//            {
//                return _context.Orders.ToList();
//            }
//            catch (Exception ex)
//            {
//                throw new Exception("Error fetching all orders", ex);
//            }
//        }

//        public IEnumerable<Order> GetOrdersByUser(string userId)
//        {
//            try
//            {
//                return _context.Orders
//                               .Where(o => o.CustomerEmail == userId)
//                               .ToList();
//            }
//            catch (Exception ex)
//            {
//                throw new Exception($"Error fetching orders for user {userId}", ex);
//            }
//        }

//        public void UpdateOrderStatus(int orderId, string status)
//        {
//            try
//            {
//                var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);
//                if (order != null)
//                {
//                    order.Status = status;
//                    _context.Orders.Update(order);
//                    _context.SaveChanges();
//                }
//            }
//            catch (Exception ex)
//            {
//                throw new Exception($"Error updating order status for order ID {orderId}", ex);
//            }
//        }

//        public void DeleteOrder(int orderId)
//        {
//            try
//            {
//                var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);
//                if (order != null)
//                {
//                    _context.Orders.Remove(order);
//                    _context.SaveChanges();
//                }
//            }
//            catch (Exception ex)
//            {
//                throw new Exception($"Error deleting order with ID {orderId}", ex);
//            }
//        }
//    }
//}
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Web_Project.Models;
using Web_Project.Models.Interfaces;

namespace Web_Project.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly string _connectionString;

        public OrderRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        private SqlConnection GetConnection()
            => new SqlConnection(_connectionString);

        public async Task<int> CreateOrderAsync(Order order)
        {
            string sql = @"
                INSERT INTO Orders (CustomerName, CustomerEmail, CustomerPhone, Address, OrderDate, TotalAmount, Status)
                VALUES (@CustomerName, @CustomerEmail, @CustomerPhone, @Address, @OrderDate, @TotalAmount, @Status);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (var conn = GetConnection())
            {
                return await conn.ExecuteScalarAsync<int>(sql, order);
            }
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            string sql = @"SELECT * FROM Orders WHERE Id = @Id";

            using (var conn = GetConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<Order>(sql, new { Id = id });
            }
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            string sql = @"SELECT * FROM Orders";

            using (var conn = GetConnection())
            {
                return await conn.QueryAsync<Order>(sql);
            }
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserAsync(string email)
        {
            string sql = @"SELECT * FROM Orders WHERE CustomerEmail = @Email";

            using (var conn = GetConnection())
            {
                return await conn.QueryAsync<Order>(sql, new { Email = email });
            }
        }

        public async Task UpdateOrderStatusAsync(int orderId, string status)
        {
            string sql = @"UPDATE Orders SET Status = @Status WHERE Id = @OrderId";

            using (var conn = GetConnection())
            {
                await conn.ExecuteAsync(sql, new { OrderId = orderId, Status = status });
            }
        }

        public async Task DeleteOrderAsync(int orderId)
        {
            string sql = @"DELETE FROM Orders WHERE Id = @OrderId";

            using (var conn = GetConnection())
            {
                await conn.ExecuteAsync(sql, new { OrderId = orderId });
            }
        }

        public async Task<int> CountOrdersAsync()
        {
            const string sql = "SELECT COUNT(1) FROM Orders";
            using var conn = GetConnection();
            return await conn.ExecuteScalarAsync<int>(sql);
        }

        public async Task<int> CountDeliveredAsync()
        {
            const string sql = "SELECT COUNT(1) FROM Orders WHERE Status = @Status";
            using var conn = GetConnection();
            return await conn.ExecuteScalarAsync<int>(sql, new { Status = "Delivered" });
        }
    }
}
