using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;  // Necesario para DbContext y DbSet
using ProyectoDAS.Models;             // Para tus modelos de entidad, ajústalo si tu namespace es distinto

namespace ProyectoDAS.Datos
{/// <summary>
 /// Contexto de datos de la aplicación, responsable de la conexión a la base de datos
 /// y del mapeo de las entidades a las tablas de MySQL.
 /// </summary>
    public class ApplicationDbContext : IdentityDbContext
    {
        // Constructor que recibe opciones desde Program.cs
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Aquí defines los DbSet para cada modelo/entidad que necesites.
        // Ejemplo para un modelo "Producto":
        public DbSet<Clientes> Clientes { get; set; }

        public DbSet<Productos> Productos { get; set; }

        public DbSet<Categoria> Categoria { get; set; }

        public DbSet<Orden> Ordenes { get; set; }
        public DbSet<OrdenDetalle> OrdenDetalles { get; set; }

        // Si aún no tienes entidades creadas, deja esto vacío por ahora
        // y agrégalo cuando definas tus clases de dominio.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Productos>()
                .Property(p => p.Precio)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Orden>()
                .Property(o => o.Total)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<OrdenDetalle>()
                .Property(od => od.PrecioUnitario)
                .HasColumnType("decimal(18,2)");
        }

    }
}
