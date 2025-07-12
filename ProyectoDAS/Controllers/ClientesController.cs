using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ProyectoDAS.Datos;
using ProyectoDAS.Models;

namespace ProyectoDAS.Controllers
{
    [Authorize]
    public class ClientesController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ClientesController(ApplicationDbContext db)
        {
            _db = db;
        }
        
        public IActionResult Index()
        {
            IEnumerable<Clientes> lista = _db.Clientes;
            return View(lista);
        }
        [Authorize]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(Clientes clientes)
        {
            clientes.FechaCreacion = DateTime.Now;
            clientes.UltimoAcceso = DateTime.Now;
            clientes.Bloqueo = false;
            _db.Clientes.Add(clientes);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        //Editar
        [Authorize]
        public IActionResult Editar(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _db.Clientes.Find(id);
            if (obj == null) 
            {
                return NotFound();
            }
            return View(obj);
        }

        //Post Editar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Clientes clientes)
        {
            if (ModelState.IsValid)
            {
                _db.Clientes.Update(clientes);
                _db.SaveChanges();
                return RedirectToAction("Index");   
            }
            return View(clientes);
        }

        //Eliminar
        //Get
        [Authorize]
        public IActionResult Eliminar(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _db.Clientes.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(Clientes clientes)
        {
            if (clientes == null)
            {
                return NotFound();
            }
            _db.Clientes.Remove(clientes);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
