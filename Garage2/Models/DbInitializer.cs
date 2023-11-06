using Garage2.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace Garage2.Models;

public static class DbInitializer
{
    public static void Seed(WebApplication applicationBuilder)
    {
        using (var scope = applicationBuilder.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<Garage2Context>();

			Garage2Context _context = applicationBuilder.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<Garage2Context>();

			if (!_context.ParkedVehicle.Any())
			{
				_context.AddRange(
					new ParkedVehicle
					{
						RegistrationNumber = "XTV 213",
						VehicleType = VehicleType.Car,
						Color = "Silver",
						Brand = "Volkswagen",
						Model = "Golf",
						NumberOfWheels = 4,
						ArrivalTime = new DateTime(2023, 11, 01, 12, 40, 13),
					},
					new ParkedVehicle
					{
						RegistrationNumber = "ABC 123",
						VehicleType = VehicleType.Car,
						Color = "Blue",
						Brand = "Ford",
						Model = "Mustang",
						NumberOfWheels = 4,
						ArrivalTime = new DateTime(2023, 11, 02, 12, 40, 13)
					},
					new ParkedVehicle
					{
						RegistrationNumber = "XYZ 789",
						VehicleType = VehicleType.Car,
						Color = "Red",
						Brand = "Toyota",
						Model = "Camry",
						NumberOfWheels = 4,
						ArrivalTime = new DateTime(2023, 11, 03, 12, 40, 13)
					},
					new ParkedVehicle
					{
						RegistrationNumber = "MOT 456",
						VehicleType = VehicleType.Motorcycle,
						Color = "Black",
						Brand = "Honda",
						Model = "CBR600RR",
						NumberOfWheels = 2,
						ArrivalTime = new DateTime(2023, 11, 01, 12, 40, 13)
					},
					new ParkedVehicle
					{
						RegistrationNumber = "TRK 001",
						VehicleType = VehicleType.Truck,
						Color = "White",
						Brand = "Ford",
						Model = "F-150",
						NumberOfWheels = 6, // Trucks often have multiple wheels
						ArrivalTime = new DateTime(2023, 11, 01, 12, 40, 13)
					},
					new ParkedVehicle
					{
						RegistrationNumber = "TRK 002",
						VehicleType = VehicleType.Bus,
						Color = "Blue",
						Brand = "Chevrolet",
						Model = "Silverado",
						NumberOfWheels = 8, // Larger trucks might have more wheels
						ArrivalTime = new DateTime(2023, 11, 02, 12, 40, 13)
					},
					new ParkedVehicle
					{
						RegistrationNumber = "BUS 001",
						VehicleType = VehicleType.Truck,
						Color = "Red",
						Brand = "Volvo",
						Model = "XC90",
						NumberOfWheels = 6, // Adjust this based on the specific bus type
						ArrivalTime = new DateTime(2023, 11, 03, 12, 40, 13)
					}
				);
			}

            context.SaveChanges();
        }
    }
}