using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web_Project.Controllers
{
    [Authorize(Policy = "UserAccess")]
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
