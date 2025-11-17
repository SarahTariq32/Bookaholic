namespace Web_Project.Models
{
   
        public class Report
        {
            public int Id { get; set; }
            public string Title { get; set; }       
            public string Description { get; set; }   
            public DateTime CreatedDate { get; set; } 
            public decimal TotalSales { get; set; }  
            public int TotalOrders { get; set; }      
        }
    }


