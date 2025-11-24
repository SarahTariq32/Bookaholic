using Microsoft.AspNetCore.Mvc;

namespace Web_Project.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
