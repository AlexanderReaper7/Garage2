using Garage2.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Garage2.Migrations;


namespace Garage2.Models;

public static class DbInitializer
{
    public static void Seed(WebApplication applicationBuilder, IServiceProvider serviceProvider)
    {
        using (var scope = applicationBuilder.Services.CreateScope())
        {
           // var parkingLotManager = serviceProvider.GetRequiredService<ParkingLotManager>();

            var context = scope.ServiceProvider.GetRequiredService<Garage2Context>();

            if (!context.ParkedVehicle.Any())
            {
                context.AddRange(
                    new ParkedVehicle
                    {
                        ParkingSpace = 1,
                        ParkingSubSpace = 0,
                        RegistrationNumber = "SE XTV213",
                        VehicleType = VehicleType.Car,
                        Color = "Silver",
                        Brand = "Volkswagen",
                        Model = "Golf",
                        NumberOfWheels = 4,
                        ArrivalTime = new DateTime(2023, 11, 01, 15, 46, 25),

                    },
                    new ParkedVehicle
                    {
                        ParkingSpace = 2,
                        ParkingSubSpace = 0,
                        RegistrationNumber = "SE ABC123",
                        VehicleType = VehicleType.Car,
                        Color = "Blue",
                        Brand = "Ford",
                        Model = "Mustang",
                        NumberOfWheels = 4,
                        ArrivalTime = new DateTime(2023, 11, 02, 01, 56, 07)
                    },
                    new ParkedVehicle
                    {
                        ParkingSpace = 3,
                        ParkingSubSpace = 0,
                        RegistrationNumber = "SE XYZ789",
                        VehicleType = VehicleType.Car,
                        Color = "Red",
                        Brand = "Toyota",
                        Model = "Camry",
                        NumberOfWheels = 4,
                        ArrivalTime = new DateTime(2023, 11, 03, 10, 55, 19)
                    },
                    new ParkedVehicle
                    {
                        ParkingSpace = 4,
                        ParkingSubSpace = 0,
                        RegistrationNumber = "SE MOT456",
                        VehicleType = VehicleType.Motorcycle,
                        Color = "Black",
                        Brand = "Honda",
                        Model = "CBR600RR",
                        NumberOfWheels = 2,
                        ArrivalTime = new DateTime(2023, 11, 04, 12, 40, 13)
                    },
                    new ParkedVehicle
                    {
                        ParkingSpace = 4,
                        ParkingSubSpace = 1,
                        RegistrationNumber = "SE MOV468",
                        VehicleType = VehicleType.Motorcycle,
                        Color = "Purple",
                        Brand = "Yamaha",
                        Model = "CBR600RR",
                        NumberOfWheels = 2,
                        ArrivalTime = new DateTime(2023, 11, 05, 23, 12, 25)
                    },
                    new ParkedVehicle
                    {
                        ParkingSpace = 5,
                        ParkingSubSpace = 0,
                        RegistrationNumber = "SE TRK001",
                        VehicleType = VehicleType.Truck,
                        Color = "White",
                        Brand = "Ford",
                        Model = "F-150",
                        NumberOfWheels = 6, // Trucks often have multiple wheels
                        ArrivalTime = new DateTime(2023, 11, 05, 13, 10, 45)
                    },
                    new ParkedVehicle
                    {
                        ParkingSpace = 7,
                        ParkingSubSpace = 0,
                        RegistrationNumber = "SE TRK002",
                        VehicleType = VehicleType.Bus,
                        Color = "Blue",
                        Brand = "Chevrolet",
                        Model = "Silverado",
                        NumberOfWheels = 8, // Larger trucks might have more wheels
                        ArrivalTime = new DateTime(2023, 11, 06, 22, 12, 17)
                    },
                    new ParkedVehicle
                    {
                        ParkingSpace = 9,
                        ParkingSubSpace = 0,
                        RegistrationNumber = "SE BUS001",
                        VehicleType = VehicleType.Truck,
                        Color = "Red",
                        Brand = "Volvo",
                        Model = "XC90",
                        NumberOfWheels = 6, // Adjust this based on the specific bus type
                        ArrivalTime = new DateTime(2023, 11, 07, 18, 10, 13)
                    },
                    new ParkedVehicle
                    {
                        ParkingSpace = 11,
                        ParkingSubSpace = 0,
                        RegistrationNumber = "SE AIR001",
                        VehicleType = VehicleType.Airplane,
                        Color = "Red",
                        Brand = "Airbus",
                        Model = "787",
                        NumberOfWheels = 24,
                        ArrivalTime = new DateTime(2023, 11, 07, 02, 11, 16)
                    },
                    new ParkedVehicle
                    {
                        ParkingSpace = 14,
                        ParkingSubSpace = 0,
                        RegistrationNumber = "SE WAT001",
                        VehicleType = VehicleType.Boat,
                        Color = "Blue",
                        Brand = "Maxi",
                        Model = "77",
                        NumberOfWheels = 0,
                        ArrivalTime = new DateTime(2023, 11, 07, 17, 01, 03)
                    }
                );
            }
           
            context.SaveChanges();
        }
    }
}