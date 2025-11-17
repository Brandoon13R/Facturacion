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
            using var conexion = new SqliteConnection(_connectionString);
            await conexion.OpenAsync();

            var comando = conexion.CreateCommand();
            comando.CommandText = "SELECT Identificador, Fecha_emision, Nombre_Cliente, Precio_Total FROM facturas";

            using var lector = await comando.ExecuteReaderAsync();

            while (await lector.ReadAsync())
            {
                var factura = new Factura
                {
                    Identificador = lector.GetInt32(0),
                    Fecha_emision = lector.GetString(1),
                    Nombre_Cliente = lector.GetString(2),
                    Precio_Total = lector.GetInt32(3)
                };

                var comandoArticulos = conexion.CreateCommand();
                comandoArticulos.CommandText = "SELECT Nombre, Precio FROM articulos WHERE FacturaId = @facturaId";
                comandoArticulos.Parameters.AddWithValue("@facturaId", factura.Identificador);

                using (var lectorArticulos = await comandoArticulos.ExecuteReaderAsync())
                {
                    while (await lectorArticulos.ReadAsync())
                    {
                        factura.Articulos.Add(new Articulo
                        {
                            Nombre = lectorArticulos.GetString(0),
                            Precio = lectorArticulos.GetInt32(1)
                        });
                    }
                }
                facturas.Add(factura);
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
                Factura.Parameters.AddWithValue("@fecha", factura.Fecha_emision);
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

        public async Task ActualizarFactura(Factura factura) 
        {
            using (var conexion = new SqliteConnection(_connectionString))
            {
                await conexion.OpenAsync();

                var comando = conexion.CreateCommand();
                comando.CommandText =
                    "UPDATE facturas " +
                    "SET Fecha_emision = @fecha, Nombre_Cliente = @cliente, Precio_Total = @total " +
                    "WHERE Identificador = @identificador";
                comando.Parameters.AddWithValue("@fecha", factura.Fecha_emision);
                comando.Parameters.AddWithValue("@cliente", factura.Nombre_Cliente);
                comando.Parameters.AddWithValue("@total", factura.Precio_Total);
                comando.Parameters.AddWithValue("@identificador", factura.Identificador);

                await comando.ExecuteNonQueryAsync();

                var facturaAActualizar = facturas.FirstOrDefault(f => f.Identificador == factura.Identificador);
                if (facturaAActualizar != null)
                {
                    facturaAActualizar.Fecha_emision = factura.Fecha_emision;
                    facturaAActualizar.Nombre_Cliente = factura.Nombre_Cliente;
                    facturaAActualizar.Precio_Total = factura.Precio_Total;
                    facturaAActualizar.Articulos = factura.Articulos;
                }
            }
        }
    }
}