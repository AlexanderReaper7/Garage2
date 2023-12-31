﻿using Garage2.AutoMapperConfig;
using Microsoft.EntityFrameworkCore;
using Garage2.Data;
using Garage2.Models;
using Garage2.Services;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using Garage2.Extensions;

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

        builder.Services.AddScoped<IAvailableLotsService, AvailableLotsService>();
        builder.Services.AddScoped<IMessageToView, MessageToView>();
        builder.Services.AddScoped<IParkingLotManager, ParkingLotManager>();
        builder.Services.AddScoped<IVehicleTypesService, VehicleTypesService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();

            
        }
        else
        {
            await app.SeedDataAsync();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}
