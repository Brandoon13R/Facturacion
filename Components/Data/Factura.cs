namespace Facturación.Components.Data
{
    public class Factura
    {
        public int Identificador { get; set; }
        public String Fecha_emision { get; set; }
        public String Nombre_Cliente { get; set; }
        public int Precio_Total { get; set; }
        public List<Articulo> Articulos { get; set; } = new List<Articulo>();
    }
}