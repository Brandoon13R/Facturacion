using Facturación.Components.Data;

namespace Facturación.Components.Controlador
{
    public class ServicioControlador
    {
        private readonly ServicioFactura servicioFactura;

        public ServicioControlador(ServicioFactura servicioFactura)
        {
            this.servicioFactura = servicioFactura;
        }
        public async Task GenerarFactura(Factura factura)
        {
            await servicioFactura.GenerarFactura(factura);
        }
    }
}
