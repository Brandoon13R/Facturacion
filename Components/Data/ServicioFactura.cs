using Microsoft.Data.Sqlite;

namespace Facturación.Components.Data
{
    public class ServicioFactura
    {
        public List<Factura> Facturas = new List<Factura>();
        public async Task GenerarFactura(Factura factura)
        {
            String ruta = "FacturacionBase.db";
            using var conexion = new SqliteConnection($"DataSource={ruta}");
            await conexion.OpenAsync();

            var comando = conexion.CreateCommand();
            comando.CommandText = "INSERT INTO facturas (Identificador, nombre_Cliente, Articulo) VALUES (@identificador, @nombre, @jugado)";
            comando.Parameters.AddWithValue("@Identificador", factura.Identificador);
            comando.Parameters.AddWithValue("@nombre_Cliente", factura.Nombre_Cliente);
            comando.Parameters.AddWithValue("@Articulo", factura.Articulo);
            comando.Parameters.AddWithValue("Fecha_emision", factura.Fecha_emision);

            await comando.ExecuteNonQueryAsync();

            Facturas.Add(factura);
        }
    }
}
