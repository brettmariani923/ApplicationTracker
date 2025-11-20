using ApplicationTracker.Data.Interfaces;
using ApplicationTracker.Data.Implementation;
using ApplicationTracker.Application.Interfaces;
using ApplicationTracker.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// MVC controllers + Razor views
builder.Services.AddControllersWithViews();

// Database + data access
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddSingleton<IDbConnectionFactory>(_ =>
    new SqlConnectionFactory(connectionString));

builder.Services.AddScoped<IDataAccess, DataAccess>();
builder.Services.AddScoped<IApplications, Applications>();
builder.Services.AddScoped<IEvents, Events>();
builder.Services.AddScoped<IFunnel, Funnel>();
builder.Services.AddScoped<ISankeyLinks, SankeyLinks>();
builder.Services.AddScoped<IStages, Stages>();
builder.Services.AddScoped<ITimelines, Timelines>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();   // serve css/js

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.Run();

