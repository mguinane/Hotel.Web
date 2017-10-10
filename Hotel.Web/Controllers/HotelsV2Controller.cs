using Microsoft.AspNetCore.Mvc;

namespace Hotel.Web.Controllers
{
    public class HotelsV2Controller : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
