using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace Web_Project.Models.ViewModels
{
    
        public class CheckoutVM
        {
            [Required, Display(Name = "Full name")]
            public string CustomerName { get; set; } = string.Empty;

            [Required, EmailAddress, Display(Name = "Email")]
            public string CustomerEmail { get; set; } = string.Empty;

            [Required, Display(Name = "Phone")]
            public string CustomerPhone { get; set; } = string.Empty;

            [Required, Display(Name = "Address")]
            public string Address { get; set; } = string.Empty;

            [Display(Name = "City")]
            public string City { get; set; } = string.Empty;

            [Display(Name = "ZIP / Postal Code")]
            public string Zip { get; set; } = string.Empty;

            [Required, Display(Name = "Payment Method")]
            public string PaymentMethod { get; set; } = string.Empty;

            public List<Web_Project.Models.ViewModels.CartItemVM> Items { get; set; } = new List<Web_Project.Models.ViewModels.CartItemVM>();

            public decimal Subtotal { get; set; }
            public decimal ShippingFee { get; set; }
            public decimal Total { get; set; }
        }
    }

