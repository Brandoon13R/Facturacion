namespace Facturación.Components.Data
{
    public class Factura
    {
        public int Identificador { get; set; }
        public DateOnly Fecha_emision { get; set; }
        public required string Nombre_Cliente { get; set; }
        public required string Articulo { get; set; }
        public int Precio_Total { get; set; }
    }
}
