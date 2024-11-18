using Lab4.Data;
using Lab4.Middleware;
using Lab4.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Проверка строки подключения
var connectionString = builder.Configuration.GetConnectionString("DBConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'DBConnection' is not configured.");
}

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddMemoryCache();
builder.Services.AddScoped<CachedDataService>();

builder.Services.AddSession();

builder.Services.AddDbContext<TelecomContext>(options =>
    options.UseSqlServer(connectionString)); // Используем проверенную строку подключения

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseDbInitializer();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
