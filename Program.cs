using Facturación.Components;
using Microsoft.Data.Sqlite;
using Facturación.Components.Data;
using Facturación.Components.Controlador;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<ServicioFactura>();
builder.Services.AddSingleton<ServicioControlador>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

String ruta = "FacturacionBase.db"; 
using var conexion = new SqliteConnection($"DataSource={ruta}");
conexion.Open();
var comando = conexion.CreateCommand();

comando.CommandText = @"
    CREATE TABLE IF NOT EXISTS facturas(
        Identificador INTEGER PRIMARY KEY AUTOINCREMENT,
        Fecha_emision TEXT,
        Nombre_Cliente TEXT,
        Precio_Total INTEGER
    );
";
comando.ExecuteNonQuery();

comando.CommandText = @"
    CREATE TABLE IF NOT EXISTS articulos(
        Nombre TEXT,
        Precio INTEGER,
        FacturaId INTEGER
    );
";
comando.ExecuteNonQuery();

app.Run();