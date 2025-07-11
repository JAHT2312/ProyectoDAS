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
            List<ProductoRecomendado> productosFinal = new();

            if (TempData["ProductosRecomendados"] != null)
            {
                var json = TempData["ProductosRecomendados"] as string;
                var productosRecomendados = JsonSerializer.Deserialize<List<ProductoRecomendado>>(json);

                if (productosRecomendados != null)
                {
                    var ids = productosRecomendados.Select(p => p.IdProducto).ToList();

                    var productosBD = _db.Productos
                        .Where(p => ids.Contains(p.Id))
                        .ToList();

                    // Construir lista final con nombres y precios desde BD
                    productosFinal = productosRecomendados.Select(pr =>
                    {
                        var productoBD = productosBD.FirstOrDefault(p => p.Id == pr.IdProducto);
                        return new ProductoRecomendado
                        {
                            IdProducto = pr.IdProducto,
                            NombreProducto = productoBD?.Nombre ?? pr.NombreProducto,
                            Precio = productoBD?.Precio ?? 0M,
                            Cantidad = pr.Cantidad // Asegúrate de haberlo recibido bien
                        };
                    }).ToList();

                    TempData.Keep("ProductosRecomendados");
                }
            }

            return View(productosFinal); // ✅ Esto coincide con @model List<ProductoRecomendado>
        }

        [HttpPost]
        public IActionResult GuardarProductos([FromBody] List<ProductoRecomendado> productos)
        {
            if (productos == null || !productos.Any())
            {
                return BadRequest("No se enviaron productos.");
            }

            // Buscar los precios desde la base de datos
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

            TempData["ProductosRecomendados"] = JsonSerializer.Serialize(productos);
            return Ok();
        }

    }
}

