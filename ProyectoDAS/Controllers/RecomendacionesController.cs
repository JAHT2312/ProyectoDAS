using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProyectoDAS.Datos;
using ProyectoDAS.Models;
using System.Text.Json;

namespace ProyectoDAS.Controllers
{

    public class RecomendacionesController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly string _apiKey;

        public RecomendacionesController(ApplicationDbContext db, IOptions<OpenAISettings> openAIOptions)
        {
            _db = db;
            _apiKey = openAIOptions.Value.ApiKey;
        }

        public IActionResult Index()
        {
            var clientes = _db.Clientes
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Nombres + " " + c.Apellidos
                }).ToList();

            ViewBag.Clientes = clientes;
            return View();
        }

        [HttpGet]
        public IActionResult ObtenerOrdenesPorCliente(int id)
        {
            var ordenes = _db.Ordenes
                .Where(o => o.ClienteId == id)
                .Select(o => new
                {
                    fechaOrden = o.FechaOrden.ToString("yyyy-MM-dd"),
                    estado = o.Estado,
                    metodoPago = o.MetodoPago,
                    total = o.Total
                }).ToList();

            return Json(ordenes);
        }

        [HttpGet]
        public IActionResult ObtenerOrdenesProcesadas(int id)
        {
            var ordenes = _db.Ordenes
                .Where(o => o.ClienteId == id && o.Estado == "Procesada")
                .Select(o => new
                {
                    fechaOrden = o.FechaOrden.ToString("yyyy-MM-dd"),
                    total = o.Total,
                    detalles = _db.OrdenDetalles
                        .Where(d => d.OrdenId == o.Id)
                        .Select(d => new
                        {
                            nombreProducto = _db.Productos
                                .Where(p => p.Id == d.ProductoId)
                                .Select(p => p.Nombre)
                                .FirstOrDefault(),
                            cantidad = d.Cantidad
                        }).ToList()
                }).ToList();

            return Json(ordenes);
        }

        [HttpPost]
        public async Task<IActionResult> GenerarRecomendacion([FromBody] int clienteId)
        {
            var productosComprados = _db.Ordenes
                .Where(o => o.ClienteId == clienteId && o.Estado == "Procesada")
                .SelectMany(o => _db.OrdenDetalles
                    .Where(d => d.OrdenId == o.Id)
                    .Select(d => new
                    {
                        ProductoId = d.ProductoId,
                        Cantidad = d.Cantidad
                    }))
                .GroupBy(p => p.ProductoId)
                .Select(g => new
                {
                    ProductoId = g.Key,
                    TotalCantidad = g.Sum(x => x.Cantidad)
                })
                .ToList();

            var productosTexto = productosComprados.Select(p =>
            {
                var producto = _db.Productos.FirstOrDefault(prod => prod.Id == p.ProductoId);
                return $"- {producto?.Nombre ?? "Producto desconocido"} (Cantidad: {p.TotalCantidad})";
            });

            var listaProductosComprados = string.Join("\n", productosTexto);

            var productosDisponibles = _db.Productos
                .Where(p => p.Esactivo)
                .Select(p => $"- {p.Nombre} (ID: {p.Id}, Precio: S/ {p.Precio:F2})")
                .ToList();

            var listaProductosDisponibles = string.Join("\n", productosDisponibles);

            var prompt = $@"
                    Estas son las compras recientes de un cliente:

                    {listaProductosComprados}

                    Y estos son los productos disponibles actualmente para vender:

                    {listaProductosDisponibles}

                    Recomienda entre 3 a 5 productos que el cliente podría comprar, basándote en productos similares o complementarios. Solo recomienda productos disponibles y que aún no haya comprado.

                    👉 Por favor, responde en una tabla Markdown **exactamente en este formato**:

                    | ID Producto | Nombre Producto |
                    |-------------|-----------------|
                    | 12          | Café Premium    |
                    | 34          | Té Verde        |

                    Asegúrate que la primera columna sea el **ID del producto (número)** y la segunda columna sea el **nombre del producto (texto)**. No inviertas el orden.
                    ";

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
            Console.WriteLine("✅ API KEY USADA: " + _apiKey);


            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new { role = "user", content = prompt }
                },
                temperature = 0.7
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), System.Text.Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
            var responseString = await response.Content.ReadAsStringAsync();

            var json = JsonDocument.Parse(responseString);

            if (!json.RootElement.TryGetProperty("choices", out var choices))
            {
                return Json(new
                {
                    error = true,
                    mensaje = "La respuesta de OpenAI no contiene 'choices'.",
                    detalle = responseString
                });
            }

            var recommendation = choices[0].GetProperty("message").GetProperty("content").GetString();

            return Json(new
            {
                recomendacion = recommendation
            });
        }
    }
}
