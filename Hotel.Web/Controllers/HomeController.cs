using Microsoft.AspNetCore.Mvc;

namespace Hotel.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Error()
        {
            return View();
        }
    }
}
