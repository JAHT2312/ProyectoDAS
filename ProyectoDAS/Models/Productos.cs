using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoDAS.Models
{
    public class Productos
    {
        [Key]
        public int Id { get; set; }

        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public Boolean Esactivo { get; set; }
        public Boolean Esdestacado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string ImagenUrl { get; set; }
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }
    }


}
