namespace Web_Project.Models.ViewModels
{
    public class OrderItemVM
    {
        public string BookTitle { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
