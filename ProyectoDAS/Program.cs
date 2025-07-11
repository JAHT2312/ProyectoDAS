// Imports necesarios:
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using ProyectoDAS.Datos;
using ProyectoDAS.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Registramos ApplicationDbContext para inyección de dependencias
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        // Detecta automáticamente la versión del servidor MySQL
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    )
);

//Prueba
builder.Services.AddControllersWithViews();

builder.Services.Configure<OpenAISettings>(builder.Configuration.GetSection("OpenAI"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
