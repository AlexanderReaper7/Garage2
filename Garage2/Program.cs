using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Garage2.Data;
using Garage2.Models;
using Garage2.Services;

namespace Garage2;
using Garage2.Models;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddDbContext<Garage2Context>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("Garage2Context") ?? throw new InvalidOperationException("Connection string 'Garage2Context' not found.")));

        // Add services to the container.
        builder.Services.AddControllersWithViews();
      //  builder.Services.AddSingleton<ParkingLotManager>();
       
        builder.Services.AddScoped<IListOfAvailableLotsService, ListOfAvailableLotsService>();
        builder.Services.AddScoped<IParkingLotManager, ParkingLotManager>();

        var app = builder.Build();

		var serviceProvider = app.Services;
		DbInitializer.Seed(app, serviceProvider);

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
            pattern: "{controller=ParkedVehicles}/{action=Index}/{id?}");

        app.Run();
    }
}
