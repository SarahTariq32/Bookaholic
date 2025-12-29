using Web_Project.Controllers;

namespace Web_Project.Models.ViewModels
{
    public class OrderDetailsVM
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public List<OrderItemVM> Items { get; set; } = new List<OrderItemVM>();
        public decimal TotalAmount { get; set; }
        public decimal ShippingFee { get; set; }
        public DateTime EstimatedDelivery { get; set; }

        // Added fields for richer modal display
        public string Address { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = "N/A";


    }
}
