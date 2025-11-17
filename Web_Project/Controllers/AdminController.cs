using Microsoft.AspNetCore.Mvc;
using Web_Project.Models;
using System.Collections.Generic;

namespace Web_Project.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult DashBoard()
        {
            return View();
        }

        public IActionResult AddEditBook()
        {
            var books = new List<Book>
            {
                new Book { Id = 1, Title="The Cruel Prince", Author="H. Author", Category="Fantasy", Price=1450 },
                new Book { Id = 2, Title="Powerless", Author="A. Author", Category="Fantasy", Price=1200 },
                new Book { Id = 3, Title="Ignite Me", Author="S. Author", Category="Fantasy", Price=1350 },
                new Book { Id = 4, Title="Once Upon a Broken Heart", Author="C. Author", Category="Fantasy", Price=1500 },
                new Book { Id = 5, Title="Heart of the Raven Prince", Author="M. Author", Category="Fantasy", Price=1400 },
                new Book { Id = 6, Title="The Housemaid", Author="T. Author", Category="Thriller", Price=1350 },
                new Book { Id = 7, Title="Silent Patient", Author="A. Smith", Category="Thriller", Price=1400 },
                new Book { Id = 8, Title="The Teacher", Author="B. Brown", Category="Thriller", Price=1250 },
                new Book { Id = 9, Title="The Locked Door", Author="L. James", Category="Thriller", Price=1300 },
                new Book { Id = 10, Title="Not Quite Dead Yet", Author="R. Taylor", Category="Thriller", Price=1300 }

            };

            return View(books);
        }

        [HttpPost]
        public IActionResult AddEditBook(Book book)
        {
            return RedirectToAction("AddEditBook");
        }

        public IActionResult AddBook()
        {
            return View();
        }

        public IActionResult EditBook()
        {
            return View();
        }

        public IActionResult ManageCategories()
        {
            return View();
        }

        public IActionResult ManageOrders()
        {
            return View();
        }

        public IActionResult Reports()
        {
            return View();
        }

        public IActionResult Settings()
        {
           
            return View();
        }
        public IActionResult Update()
        {

            return View();
        }
        [HttpPost]
        public IActionResult UpdateSettings(string Email, string Password, string MaintenanceMode)
        {
            TempData["Message"] = "Settings saved successfully!";
            return RedirectToAction("Settings");
        }
    }
}
