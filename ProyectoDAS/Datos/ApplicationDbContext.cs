using Microsoft.EntityFrameworkCore;  // Necesario para DbContext y DbSet
using ProyectoDAS.Models;             // Para tus modelos de entidad, ajústalo si tu namespace es distinto

namespace ProyectoDAS.Datos
{/// <summary>
 /// Contexto de datos de la aplicación, responsable de la conexión a la base de datos
 /// y del mapeo de las entidades a las tablas de MySQL.
 /// </summary>
    public class ApplicationDbContext : DbContext
    {
        // Constructor que recibe opciones desde Program.cs
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Aquí defines los DbSet para cada modelo/entidad que necesites.
        // Ejemplo para un modelo "Producto":
        // public DbSet<Producto> Productos { get; set; }

        // Si aún no tienes entidades creadas, deja esto vacío por ahora
        // y agrégalo cuando definas tus clases de dominio.
    }
}
