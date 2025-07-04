using System.ComponentModel.DataAnnotations;

namespace ProyectoDAS.Models
{
    public class Clientes
    {
        [Key]
        public int Id { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Correo { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public DateTime FechaCreacion { get; set; }
        public Boolean Bloqueo { get; set; }
        public DateTime UltimoAcceso { get; set; }

    }
}
