using System.ComponentModel.DataAnnotations;

namespace Web_Project.Models
{
    public class Cartitem
    {
        [Key]
        public int CartID { get; set; }
        public string UserID { get; set; } 
        public int BookID { get; set; }
        public int Quantity { get; set; }
    }
}
