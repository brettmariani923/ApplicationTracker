using ApplicationTracker.Data.Interfaces;
using ApplicationTracker.Data.Implementation;
using ApplicationTracker.Application.Interfaces;
using ApplicationTracker.Application.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// your DI registrations...
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddSingleton<IDbConnectionFactory>(sp =>
    new SqlConnectionFactory(connectionString));
builder.Services.AddScoped<IDataAccess, DataAccess>();
builder.Services.AddScoped<ITrackerService, TrackerService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();   // ✅ serve css/js if you have them

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

// (optional) keep this if you also have pure API controllers with [Route] attributes
// app.MapControllers();

app.Run();
