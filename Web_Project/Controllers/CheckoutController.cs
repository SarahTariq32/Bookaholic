using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web_Project.Models;
using Web_Project.Models;

namespace Web_Project.Controllers
{
    [Authorize(Policy = "UserAccess")]
    public class CheckoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PlaceOrder(Order order)
        {
            return RedirectToAction("Confirmation");
        }

        public IActionResult Confirmation()
        {
            return View("OrderConfirmation");
        }
    }
}
