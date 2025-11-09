using Facturación.Components;
using Microsoft.Data.Sqlite;
using Facturación.Components.Data;
using Facturación.Components.Controlador;

var builder = WebApplication.CreateBuilder(args);

String ruta = "FacturacionBase.db";
string connectionString = $"DataSource={ruta}";

builder.Services.AddScoped<ServicioFactura>(sp => new ServicioFactura(connectionString));
builder.Services.AddScoped<ServicioControlador>();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();


using (var conexion = new SqliteConnection(connectionString))
{
    conexion.Open();
    var comando = conexion.CreateCommand();
    comando.CommandText = @"
    create table if not exists facturas(
        Identificador INTEGER PRIMARY KEY AUTOINCREMENT,
        Fecha_emision Date,
        Nombre_Cliente TEXT,
        Articulos TEXT,
        Precio_Total INTEGER
    );

    create table if not exists articulos(
        Codigo INTEGER PRIMARY KEY AUTOINCREMENT,
        Nombre TEXT,
        Precio INTEGER
    )
    ";
    comando.ExecuteNonQuery();
}

app.Run();
