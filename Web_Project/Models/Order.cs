namespace Web_Project.Models
{
        public class Order
        {
            public int Id { get; set; }              
            public string CustomerName { get; set; }
            public string CustomerEmail { get; set; }
            public string CustomerPhone { get; set; }
            public string Address { get; set; }
            public DateTime OrderDate { get; set; }
            public decimal TotalAmount { get; set; }
            public string Status { get; set; }
            public List<OrderDetail> OrderDetails { get; set; }

        }
    }


