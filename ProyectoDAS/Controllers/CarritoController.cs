using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProyectoDAS.Models;

namespace ProyectoDAS.Controllers
{
    public class CarritoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
