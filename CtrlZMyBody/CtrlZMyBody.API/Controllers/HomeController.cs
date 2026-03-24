using Microsoft.AspNetCore.Mvc;

namespace CtrlZMyBody.API.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Redirect("/admin/login");
        }
    }
}
