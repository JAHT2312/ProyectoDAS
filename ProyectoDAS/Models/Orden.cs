using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ProyectoDAS.Models
{
    public class Orden
    {
        [Key]
        public int Id { get; set; }

        public int ClienteId { get; set; }
        [ValidateNever]
        public Clientes Cliente { get; set; }

        public DateTime FechaOrden { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; }
        public string Direccion { get; set; }

        public string MetodoPago { get; set; } 

        public bool EsAnulada { get; set; }

        [ValidateNever]
        public ICollection<OrdenDetalle> Detalles { get; set; }
    }

}
