using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using ProyectoDAS.Models;
using ProyectoDAS.Datos;
using Microsoft.EntityFrameworkCore;

namespace ProyectoDAS.Controllers
{
    public class CarritoInteligenteController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CarritoInteligenteController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            // ✅ El carrito empieza vacío por defecto
            List<ProductoRecomendado> productosFinal = new();

            // Si TempData existe, lo reconstruye
            if (TempData["ProductosRecomendados"] is string json && !string.IsNullOrEmpty(json))
            {
                var productosRecomendados = JsonSerializer.Deserialize<List<ProductoRecomendado>>(json);

                if (productosRecomendados != null && productosRecomendados.Any())
                {
                    var ids = productosRecomendados.Select(p => p.IdProducto).ToList();

                    var productosBD = _db.Productos
                        .Where(p => ids.Contains(p.Id))
                        .ToList();

                    productosFinal = productosRecomendados.Select(pr =>
                    {
                        var productoBD = productosBD.FirstOrDefault(p => p.Id == pr.IdProducto);
                        return new ProductoRecomendado
                        {
                            IdProducto = pr.IdProducto,
                            NombreProducto = productoBD?.Nombre ?? pr.NombreProducto,
                            Precio = productoBD?.Precio ?? 0M,
                            Cantidad = pr.Cantidad
                        };
                    }).ToList();

                    TempData.Keep("ProductosRecomendados");
                }
            }

            return View(productosFinal);
        }

        [HttpPost]
        public IActionResult GuardarProductos([FromBody] List<ProductoRecomendado> productos)
        {
            if (productos == null || !productos.Any())
            {
                return BadRequest("No se enviaron productos.");
            }

            // Obtener precios actualizados desde la base de datos
            var dbProductos = _db.Productos
                .Where(p => productos.Select(x => x.IdProducto).Contains(p.Id))
                .ToDictionary(p => p.Id, p => p.Precio);

            foreach (var prod in productos)
            {
                if (dbProductos.TryGetValue(prod.IdProducto, out var precio))
                {
                    prod.Precio = precio;
                }
            }

            // Guardar productos actualizados en TempData
            TempData["ProductosRecomendados"] = JsonSerializer.Serialize(productos);
            return Ok();
        }
    }
}

