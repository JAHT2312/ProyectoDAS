namespace ProyectoDAS.Models
{
    public class ProductoRecomendado
    {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public decimal Precio { get; set; } // 🟢 AÑADIDO
        public int Cantidad { get; set; } = 1; // Por defecto 1 unidad
    }
}
