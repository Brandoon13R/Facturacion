using Microsoft.Data.Sqlite;

namespace Facturación.Components.Data
{
    public class ServicioFactura
    {
        private List<Factura> facturas = new List<Factura>();
        private string _connectionString = "DataSource=FacturacionBase.db";

        public Task<List<Factura>> ObtenerFacturas() => Task.FromResult(facturas);

        public async Task AgregarFactura(Factura factura)
        {
            using (var conexion = new SqliteConnection(_connectionString))
            {
                await conexion.OpenAsync();

                // --- Inserta la Factura (el "encabezado") ---
                var cmdFactura = conexion.CreateCommand();
                cmdFactura.CommandText =
                    "INSERT INTO facturas (Identificador, Fecha_emision, Nombre_Cliente, Precio_Total) " +
                    "VALUES (@id, @fecha, @cliente, @total)";

                cmdFactura.Parameters.AddWithValue("@id", factura.Identificador);
                cmdFactura.Parameters.AddWithValue("@fecha", factura.Fecha_emision.ToString("yyyy-MM-dd"));
                cmdFactura.Parameters.AddWithValue("@cliente", factura.Nombre_Cliente);
                cmdFactura.Parameters.AddWithValue("@total", factura.Precio_Total);
                await cmdFactura.ExecuteNonQueryAsync();

                foreach (var articulo in factura.Articulos)
                {
                    var cmdArticulo = conexion.CreateCommand();
                    cmdArticulo.CommandText =
                        "INSERT INTO articulos (Nombre, Precio, FacturaId) " +
                        "VALUES (@nombre, @precio, @facturaId)";

                    cmdArticulo.Parameters.AddWithValue("@nombre", articulo.Nombre);
                    cmdArticulo.Parameters.AddWithValue("@precio", articulo.Precio);
                    cmdArticulo.Parameters.AddWithValue("@facturaId", factura.Identificador); // El vínculo
                    await cmdArticulo.ExecuteNonQueryAsync();
                }
            }
            facturas.Add(factura);
        }
    }
}
