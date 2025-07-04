using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoDAS.Datos;
using ProyectoDAS.Models;

namespace ProyectoDAS.Controllers
{
    public class ProductosController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProductosController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var lista = _db.Productos
                           .Include(p => p.Categoria) // 👈 Relación incluida
                           .ToList();

            return View(lista);
        }
    }
}

