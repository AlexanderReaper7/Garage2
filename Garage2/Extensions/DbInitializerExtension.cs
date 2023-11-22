using Garage2.Data;
using Garage2.Models;
using Microsoft.EntityFrameworkCore;

namespace Garage2.Extensions
{
    public static class DbInitializerExtension
    {
        public static async Task SeedDataAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            var serviceProvider = scope.ServiceProvider;
            var db = serviceProvider.GetRequiredService<Garage2Context>();

            //If there isn't any parkedVehicles in the database erase it and instantiate defaults
             //await db.Database.EnsureDeletedAsync();
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

    }
}
