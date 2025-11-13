using ApplicationTracker.Application.Interfaces;
using ApplicationTracker.Application.Services;
using ApplicationTracker.Data.Implementation;
using ApplicationTracker.Data.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// MVC: API controllers + Razor views
builder.Services.AddControllersWithViews();

// Swagger (optional but nice)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DataAccess implementation
builder.Services.AddScoped<IDataAccess, DataAccess>();

// TrackerService from Application layer
builder.Services.AddScoped<ITrackerService, TrackerService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// For CSS/JS/images in wwwroot (optional but usually needed for styling)
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Attribute-routed API controllers (e.g., /api/...)
app.MapControllers();

// Conventional routing for MVC + Razor views
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.Run();
