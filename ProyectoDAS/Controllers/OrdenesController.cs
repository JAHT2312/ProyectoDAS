using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoDAS.Datos;
using ProyectoDAS.Models;

namespace ProyectoDAS.Controllers
{
    [Authorize]
    public class OrdenesController : Controller
    {
        private readonly ApplicationDbContext _db;

        public OrdenesController(ApplicationDbContext db)
        {
            _db = db;
        }
        [Authorize]
        public IActionResult Index()
        {
            // 🔧 Incluimos al cliente relacionado con cada orden
            var lista = _db.Ordenes
                .Include(o => o.Cliente)
                .ToList();

            return View(lista);
        }
        [Authorize]
        public IActionResult Crear()
        {
            // 🔸 Combo de Clientes
            ViewBag.Clientes = _db.Clientes
                .Where(c => !c.Bloqueo) // solo clientes no bloqueados
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Nombres + " " + c.Apellidos
                })
                .ToList();

            // 🔸 Combo de Productos
            ViewBag.Productos = _db.Productos
                .Where(p => p.Esactivo)
                .Select(p => new
                {
                    p.Id,
                    p.Nombre,
                    p.Precio
                })
                .ToList();

            return View();
        }
    }
}

