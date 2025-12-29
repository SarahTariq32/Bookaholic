namespace Web_Project.Models.ViewModels
{
    public class ReportsVM
    {
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalCustomers { get; set; }
        public List<string> RevenueLabels { get; set; } = new List<string>();
        public List<decimal> RevenueValues { get; set; } = new List<decimal>();

        public List<int> OrdersPerMonth { get; set; } = new List<int>();


        // Daily (last 30 days)
        public List<string> DailyLabels { get; set; } = new List<string>();
        public List<decimal> DailyRevenue { get; set; } = new List<decimal>();
        public List<int> DailyOrders { get; set; } = new List<int>();


    }
}
