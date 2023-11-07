using Garage2.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace Garage2.Models;

public static class DbInitializer
{

	public static void Seed(WebApplication applicationBuilder, IServiceProvider serviceProvider)
    {
	    var parkingSlotManager = serviceProvider.GetRequiredService<ParkingSlotManager>();
		using (var scope = applicationBuilder.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<Garage2Context>();
            //ParkingSlotManager parkingSlotManager = new ParkingSlotManager();
          
			if (!context.ParkedVehicle.Any())
			{
				context.AddRange(
					new ParkedVehicle
					{
						ParkingslotNr = 1,
						RegistrationNumber = "XTV213",
						VehicleType = VehicleType.Car,
						Color = "Silver",
						Brand = "Volkswagen",
						Model = "Golf",
						NumberOfWheels = 4,
						ArrivalTime = new DateTime(2023, 11, 01, 12, 40, 13),

					},
					new ParkedVehicle
					{
						ParkingslotNr = 2,
						RegistrationNumber = "ABC123",
						VehicleType = VehicleType.Car,
						Color = "Blue",
						Brand = "Ford",
						Model = "Mustang",
						NumberOfWheels = 4,
						ArrivalTime = new DateTime(2023, 11, 02, 12, 40, 13)
					},
					new ParkedVehicle
					{
						ParkingslotNr = 3,
						RegistrationNumber = "XYZ789",
						VehicleType = VehicleType.Car,
						Color = "Red",
						Brand = "Toyota",
						Model = "Camry",
						NumberOfWheels = 4,
						ArrivalTime = new DateTime(2023, 11, 03, 12, 40, 13)
					},
					new ParkedVehicle
					{
						ParkingslotNr = 4,
						RegistrationNumber = "MOT456",
						VehicleType = VehicleType.Motorcycle,
						Color = "Black",
						Brand = "Honda",
						Model = "CBR600RR",
						NumberOfWheels = 2,
						ArrivalTime = new DateTime(2023, 11, 01, 12, 40, 13)
					},
					new ParkedVehicle
					{
						ParkingslotNr = 5,
						RegistrationNumber = "TRK001",
						VehicleType = VehicleType.Truck,
						Color = "White",
						Brand = "Ford",
						Model = "F-150",
						NumberOfWheels = 6, // Trucks often have multiple wheels
						ArrivalTime = new DateTime(2023, 11, 01, 12, 40, 13)
					},
					new ParkedVehicle
					{
						ParkingslotNr = 6,
						RegistrationNumber = "TRK002",
						VehicleType = VehicleType.Bus,
						Color = "Blue",
						Brand = "Chevrolet",
						Model = "Silverado",
						NumberOfWheels = 8, // Larger trucks might have more wheels
						ArrivalTime = new DateTime(2023, 11, 02, 12, 40, 13)
					},
					new ParkedVehicle
					{
						ParkingslotNr = 7,
						RegistrationNumber = "BUS001",
						VehicleType = VehicleType.Truck,
						Color = "Red",
						Brand = "Volvo",
						Model = "XC90",
						NumberOfWheels = 6, // Adjust this based on the specific bus type
						ArrivalTime = new DateTime(2023, 11, 03, 12, 40, 13)
					}
				);
			}


			var parkingSlotNumbers = context.ParkedVehicle.Select(p => p.ParkingslotNr).ToList();

			for (int i = 0; i < parkingSlotNumbers.Count; i++)
			{
				parkingSlotManager.AddVehicleToSlot(parkingSlotNumbers[i]);
			}

			context.SaveChanges();
        }
    }
}