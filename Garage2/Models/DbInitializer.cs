using Garage2.Data;
using Microsoft.EntityFrameworkCore;
using Bogus;
using Bogus.Extensions.UnitedKingdom;
using Garage2.Models.Entities;
using System;

namespace Garage2.Models;

public class DbInitializer
{
    private static Faker faker = null!;
    private static readonly Random rnd = new Random();
    private static readonly int dbInitializerAmount = 10;
    public static async Task InitAsync(Garage2Context db)
    {
        //If there are members in database return
        if (await db.Member.AnyAsync()) return;

        faker = new Faker("sv");

        var members = GenerateMembers(dbInitializerAmount);
        await db.AddRangeAsync(members);

        var vehicleTypes = GenerateVehicleTypes(dbInitializerAmount);
        await db.AddRangeAsync(vehicleTypes);

        var vehicles = GenerateVehicles(vehicleTypes, members);
        await db.AddRangeAsync(vehicles);
        await db.SaveChangesAsync();
    }
    //Generate Random Members
    private static IEnumerable<Member> GenerateMembers(int numberOfMembers)
    {
        var members = new List<Member>();
        var membershipValues = Enum.GetValues(typeof(Membership));

        for (int i = 0; i < numberOfMembers; i++)
        {
            var gender = faker.Person.Gender;
            var fName = faker.Person.FirstName;
            var lName = faker.Person.LastName;
            var dob = faker.Date.Between(new DateTime(1965, 1, 2), new DateTime(2002, 1, 2));

            var member = new Member()
            {
                FirstName = fName,
                LastName = lName,
                PersonNumber = dob.ToString("d"),
                Membership = (Membership)rnd.Next(membershipValues.Length),
            };

            members.Add(member);
        }
        return members;
    }
    //Generate Random vehicles
    private static IEnumerable<VehicleType> GenerateVehicleTypes(int generateVehicleTypes)
    {
        var vehicleTypes = new List<VehicleType>()
        {
            new VehicleType()
            {
                Name = "Car",
                Size = 3,
            },
            new VehicleType()
            {
                Name = "Motorcycle",
                Size = 1,
            },
            new VehicleType()
            {
                Name = "Truck",
                Size = 6,
            },
            new VehicleType()
            {
                Name = "Bus",
                Size = 6,
            },
            new VehicleType()
            {
                Name = "Boat",
                Size = 9,
            },
            new VehicleType()
            {
                Name = "Airplane",
                Size = 9,
            }
        };
        return vehicleTypes;
    }

    //Generate Random Vehicle
    private static IEnumerable<ParkedVehicle> GenerateVehicles(IEnumerable<VehicleType> vehicleTypes, IEnumerable<Member> members)
    {
        var parkedVehicle = new List<ParkedVehicle>();
        var startDate = new DateTime(2023, 1, 1);
        var endDate = new DateTime(2023, 11, 19);

        foreach (var member in members)
        {
            var model = faker.Vehicle.Model();
            var regNr = faker.Vehicle.GbRegistrationPlate(new DateTime(2001, 09, 2), new DateTime(2023, 1, 2));
            var color = faker.Commerce.Color();
            var arrivalTime = faker.Date.Between(startDate, endDate);
            var brand = faker.Vehicle.Manufacturer();

            var currentVehicleType = vehicleTypes.ElementAt(rnd.Next(vehicleTypes.Count()));

            var vehicle = new ParkedVehicle()
            {
                Brand = brand,
                Model = model,
                RegistrationNumber = regNr,
                Color = color,
                ArrivalTime = arrivalTime,
                VehicleType = currentVehicleType,
                Member = member,
            };

            FetchNrWheelsAndParkingSpace(vehicle);
            parkedVehicle.Add(vehicle);
        }

        return parkedVehicle;
    }
    //Fetch Number Of WHeels And Parking Space Depending On Type
    private static void FetchNrWheelsAndParkingSpace(ParkedVehicle vehicle)
    {
        switch (vehicle.VehicleType.Name)
        {
            case "Car":
                vehicle.NumberOfWheels = 4;
                break;
            case "Bus":
                vehicle.NumberOfWheels = 6;
                break;
            case "Truck":
                vehicle.NumberOfWheels = 6;
                break;
            case "Motorcycle":
                vehicle.NumberOfWheels = 2;
                break;
            case "Boat":
                vehicle.NumberOfWheels = 0;
                break;
            case "Airplane":
                vehicle.NumberOfWheels = 8;
                break;
        }
    }
}

