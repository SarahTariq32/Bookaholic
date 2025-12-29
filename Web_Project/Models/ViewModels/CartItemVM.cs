using System.ComponentModel.DataAnnotations;

namespace Web_Project.Models.ViewModels
{
    public class CartItemVM
    {
        public int BookID { get; set; }
        public string Title { get; set; }
        public string CoverImage { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal LineTotal => Price * Quantity;
    }

    
}



