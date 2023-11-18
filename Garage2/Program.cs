using Garage2.AutoMapperConfig;
using Microsoft.EntityFrameworkCore;
using Garage2.Data;
using Garage2.Models;
using Garage2.Services;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;

namespace Garage2;
using Garage2.Models;
public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddDbContext<Garage2Context>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("Garage2Context") ?? throw new InvalidOperationException("Connection string 'Garage2Context' not found.")));

        // Add services to the container.
        builder.Services.AddControllersWithViews();
		builder.Services.AddAutoMapper(typeof(GarageMapping));

		builder.Services.AddScoped<IListOfAvailableLotsService, ListOfAvailableLotsService>();
        builder.Services.AddScoped<IParkingLotManager, ParkingLotManager>();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
	        var serviceProvider = scope.ServiceProvider;
	        var db = serviceProvider.GetRequiredService<Garage2Context>();

	        //erases the database everytime i run the application
	        await db.Database.EnsureDeletedAsync();
	        //run all the migrations, if the database doesnt exist create it, if it exist, just update the database
	        await db.Database.MigrateAsync();

	        try
	        {
		        await DbInitializer.InitAsync(db);
	        }
	        catch (Exception e)
	        {
		        Console.WriteLine(e.Message);
		        throw;
	        }
        }
		//DbInitializer.Seed(app, serviceProvider);

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
