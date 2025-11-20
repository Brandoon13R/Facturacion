using Facturación.Components.Data;

namespace Facturación.Components.Controlador
{
    public class ServicioControlador
    {
        private readonly ServicioFactura _servicioFactura;

        public ServicioControlador(ServicioFactura servicioFactura)
        {
            _servicioFactura = servicioFactura;
        }
        public async Task<List<Factura>> ObtenerFacturas()
        {
            return await _servicioFactura.ObtenerFacturas();
        }

        public async Task AgregarFactura(Factura factura)
        {
            factura.Identificador = await GenerarNuevoID();
            factura.Precio_Total = factura.Articulos.Sum(a => a.Precio);
            await _servicioFactura.AgregarFactura(factura);
        }

        public async Task EliminarFactura(int Identificador)
        {
            await _servicioFactura.EliminarFactura(Identificador);
        }
        
        public async Task ActualizarFactura(Factura factura)
        {
            factura.Precio_Total = factura.Articulos.Sum(a => a.Precio);
            await _servicioFactura.ActualizarFactura(factura);
        }

        public async Task<List<string>> FacturasCaras()
        {
            return await _servicioFactura.FacturasCaras();
        }

        public async Task<List<string>> PrecioIntermedio()
        {
            return await _servicioFactura.PreciosIntermedios();
        }

        public async Task<List<string>> FechasFacturas()
        {
            return await _servicioFactura.FechasFacturas();
        }

        public async Task<List<string>> MesMayorVentas()
        {
            return await _servicioFactura.MesMayorVentas();
        }

        public async Task<List<string>> ArticuloMasVendido()
        {
            return await _servicioFactura.ArticuloMasVendido();
        }

        private async Task<int> GenerarNuevoID()
        {
            var facturas = await _servicioFactura.ObtenerFacturas();
            return facturas.Any() ? facturas.Max(f => f.Identificador) + 1 : 1;
        }
    }
}