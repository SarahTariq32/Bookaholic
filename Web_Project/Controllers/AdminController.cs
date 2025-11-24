using Microsoft.AspNetCore.Mvc;
using Web_Project.Models;
using System.Collections.Generic;

namespace Web_Project.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult AddEditBook()
        {
            var books = new List<Book>
            {
                new Book { BookID = 1, Title = "The Cruel Prince", Author = "H. Author", CategoryID = 1, Price = 1450 },
                new Book { BookID = 2, Title = "Powerless", Author = "A. Author", CategoryID = 1, Price = 1200 },
                new Book { BookID = 3, Title = "Ignite Me", Author = "S. Author", CategoryID = 1, Price = 1350 },
                new Book { BookID = 4, Title = "Once Upon a Broken Heart", Author = "C. Author", CategoryID = 1, Price = 1500 },
                new Book { BookID = 5, Title = "Heart of the Raven Prince", Author = "M. Author", CategoryID = 1, Price = 1400 },
                new Book { BookID = 6, Title = "The Housemaid", Author = "T. Author", CategoryID = 2, Price = 1350 },
                new Book { BookID = 7, Title = "Silent Patient", Author = "A. Smith", CategoryID = 2, Price = 1400 },
                new Book { BookID = 8, Title = "The Teacher", Author = "B. Brown", CategoryID = 2, Price = 1250 },
                new Book { BookID = 9, Title = "The Locked Door", Author = "L. James", CategoryID = 2, Price = 1300 },
                new Book { BookID = 10, Title = "Not Quite Dead Yet", Author = "R. Taylor", CategoryID = 2, Price = 1300 }
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

        public IActionResult EditBook(int id)
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

        [HttpPost]
        public IActionResult UpdateSettings(string Email, string Password, string MaintenanceMode)
        {
            TempData["Message"] = "Settings saved successfully!";
            return RedirectToAction("Settings");
        }
    }
}
