using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public IActionResult Crear()
        {
            var categoriasActivas = _db.Categoria
                           .Where(c => c.EsActivo)
                           .ToList();

            ViewBag.Categorias = new SelectList(categoriasActivas, "Id", "Nombre");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(Productos productos)
        {
            if (ModelState.IsValid)
            {
                productos.FechaCreacion = DateTime.Now;
                _db.Productos.Add(productos);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Volver a cargar el combo si hay errores de validación
            var categorias = _db.Categoria
                                .Where(c => c.EsActivo)
                                .ToList();
            ViewBag.Categorias = new SelectList(categorias, "Id", "Nombre");
            return View(productos);
        }

        //Editar
        public IActionResult Editar(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = _db.Productos.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            // 🔽 Cargar categorías activas
            var categorias = _db.Categoria.Where(c => c.EsActivo).ToList();
            ViewBag.Categorias = new SelectList(categorias, "Id", "Nombre", obj.CategoriaId); // ← preselecciona la actual

            return View(obj);
        }

        //Post Editar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Productos productos)
        {
            if (ModelState.IsValid)
            {
                _db.Productos.Update(productos);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            // 🔽 Recargar combo si hay error
            var categorias = _db.Categoria.Where(c => c.EsActivo).ToList();
            ViewBag.Categorias = new SelectList(categorias, "Id", "Nombre", productos.CategoriaId); // ← mantener selección

            return View(productos);
        }

        //Eliminar
        //Get
        public IActionResult Eliminar(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _db.Productos.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            // 🔽 Cargar categorías activas
            var categorias = _db.Categoria.Where(c => c.EsActivo).ToList();
            ViewBag.Categorias = new SelectList(categorias, "Id", "Nombre", obj.CategoriaId); // ← preselecciona la actual

            return View(obj);
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(Productos productos)
        {
            if (productos == null)
            {
                return NotFound();
            }
            _db.Productos.Remove(productos);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


    }
}

