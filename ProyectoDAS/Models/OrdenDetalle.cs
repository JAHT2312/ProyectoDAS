using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoDAS.Models
{
    public class OrdenDetalle
    {
        [Key]
        public int Id { get; set; }

        public int OrdenId { get; set; }

        [ForeignKey("OrdenId")]
        public Orden Orden { get; set; }

        public int ProductoId { get; set; }

        [ForeignKey("ProductoId")]
        public Productos Producto { get; set; }

        public int Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }

        public decimal Subtotal => Cantidad * PrecioUnitario;
    }

}
