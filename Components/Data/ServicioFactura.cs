using Microsoft.Data.Sqlite;

namespace Facturación.Components.Data
{
    public class ServicioFactura
    {
        private List<Factura> facturas = new List<Factura>();
        private string _connectionString = "DataSource=FacturacionBase.db";

        public async Task<List<Factura>> ObtenerFacturas()
        {
            facturas.Clear();
            String ruta = "FacturacionBase.db";
            using var conexion = new SqliteConnection($"DataSource={ruta}");
            await conexion.OpenAsync();

            var comando = conexion.CreateCommand();
            comando.CommandText = "SELECT Identificador, Fecha_emision, Nombre_Cliente, Precio_Total FROM facturas";
            using var lector = await comando.ExecuteReaderAsync();

            while (await lector.ReadAsync())
            {
                facturas.Add(new Factura
                {
                    Identificador = lector.GetInt32(0),
                    Fecha_emision = DateOnly.FromDateTime(lector.GetDateTime(1)),
                    Nombre_Cliente = lector.GetString(2),
                    Precio_Total = lector.GetInt32(3)
                });
            }
            return facturas;
        }

        public async Task AgregarFactura(Factura factura)
        {
            using (var conexion = new SqliteConnection(_connectionString))
            {
                await conexion.OpenAsync();

                var Factura = conexion.CreateCommand();
                Factura.CommandText =
                    "INSERT INTO facturas (Identificador, Fecha_emision, Nombre_Cliente, Precio_Total) " +
                    "VALUES (@identificador, @fecha, @cliente, @total)";

                Factura.Parameters.AddWithValue("@identificador", factura.Identificador);
                Factura.Parameters.AddWithValue("@fecha", factura.Fecha_emision.ToString("yyyy-MM-dd"));
                Factura.Parameters.AddWithValue("@cliente", factura.Nombre_Cliente);
                Factura.Parameters.AddWithValue("@total", factura.Precio_Total);
                await Factura.ExecuteNonQueryAsync();

                foreach (var articulo in factura.Articulos)
                {
                    var Articulo = conexion.CreateCommand();
                    Articulo.CommandText =
                        "INSERT INTO articulos (Nombre, Precio, FacturaId) " +
                        "VALUES (@nombre, @precio, @facturaId)";

                    Articulo.Parameters.AddWithValue("@nombre", articulo.Nombre);
                    Articulo.Parameters.AddWithValue("@precio", articulo.Precio);
                    Articulo.Parameters.AddWithValue("@facturaId", factura.Identificador);
                    await Articulo.ExecuteNonQueryAsync();
                }
            }
            facturas.Add(factura);
        }

        public async Task EliminarFactura(int Identificador)
        {
            using (var conexion = new SqliteConnection(_connectionString))
            {
                await conexion.OpenAsync();

                var comando = conexion.CreateCommand();
                comando.CommandText = "DELETE FROM facturas WHERE Identificador = @identificador";
                comando.Parameters.AddWithValue("@identificador", Identificador);

                await comando.ExecuteNonQueryAsync();

                var facturaABorrar = facturas.FirstOrDefault(f => f.Identificador == Identificador);
                if (facturaABorrar != null)
                {
                    facturas.Remove(facturaABorrar);
                }
            }
        }
    }
}
