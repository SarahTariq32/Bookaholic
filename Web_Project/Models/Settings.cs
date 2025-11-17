namespace Web_Project.Models
{
    public class Settings
    {
        public int Id { get; set; }
        public string StoreName { get; set; }
        public string ContactEmail { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Currency { get; set; } = "PKR";
    }
}
