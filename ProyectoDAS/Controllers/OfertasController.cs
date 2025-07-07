using Microsoft.AspNetCore.Mvc;

namespace ProyectoDAS.Controllers
{
    public class OfertasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
