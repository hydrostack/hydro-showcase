using Hydro.Configuration;
using HydroShowcase.Domain;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddHydro(options =>
{
    options.AntiforgeryTokenEnabled = true;
});

builder.Services.AddScoped<IProductsStorage, ProductsStorage>();

builder.Services.Configure<RouteOptions>(option =>
{
    option.LowercaseUrls = true;
    option.LowercaseQueryStrings = true;
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

app.UseHydro(builder.Environment);

app.Run();
