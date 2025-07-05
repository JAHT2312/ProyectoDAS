using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ProyectoDAS.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public Boolean EsActivo { get; set; }
        public DateTime FechaCreacion { get; set; }

        // Opcional: lista de productos
        [ValidateNever]
        public ICollection<Productos> Productos { get; set; }
    }
}
