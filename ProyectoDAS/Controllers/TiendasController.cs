using Microsoft.AspNetCore.Mvc;

namespace ProyectoDAS.Controllers
{
    public class TiendasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
