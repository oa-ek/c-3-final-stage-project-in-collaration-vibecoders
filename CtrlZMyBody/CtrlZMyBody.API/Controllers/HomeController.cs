using Microsoft.AspNetCore.Mvc;

namespace CtrlZMyBody.API.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["BodyClass"] = "landing-page";
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
