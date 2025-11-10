namespace Facturación.Components.Data
{
    public class Factura
    {
        public int Identificador { get; set; }
        public DateOnly Fecha_emision { get; set; }
        public String Nombre_Cliente { get; set; }
        public int Precio_Total { get; set; }

        // La factura "contiene" su lista de artículos
        public List<Articulo> Articulos { get; set; } = new List<Articulo>();
    }
}