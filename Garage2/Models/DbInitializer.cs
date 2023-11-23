using Garage2.Data;
using Microsoft.EntityFrameworkCore;
using Bogus;
using Bogus.Extensions.UnitedKingdom;
using Garage2.Models.Entities;
using System;
using System.Text;
using Bogus.DataSets;

namespace Garage2.Models;

public class DbInitializer
{
    private static readonly Faker Faker = new("sv");
    private static readonly Random Rnd = new();
    private const int MembersCount = 10;

    public static async Task InitAsync(Garage2Context db)
    {
        // If there are members in database return
        if (await db.Member.AnyAsync()) return;


        var members = GenerateMembers(MembersCount);
        await db.AddRangeAsync(members);

        var vehicleTypes = GenerateVehicleTypes();
        await db.AddRangeAsync(vehicleTypes);

        var vehicles = GenerateVehicles(vehicleTypes, members);
        await db.AddRangeAsync(vehicles);
        await db.SaveChangesAsync();
    }
    // Generate Random Members
    private static List<Member> GenerateMembers(int numberOfMembers)
    {
        var members = new List<Member>();
        var membershipValues = Enum.GetValues(typeof(Membership));

        for (var i = 0; i < numberOfMembers; i++)
        {
            var isFemale = Rnd.Next(2) == 1;
            var fName = Faker.Name.FirstName(isFemale ? Name.Gender.Female : Name.Gender.Male);
            var lName = Faker.Name.LastName(isFemale ? Name.Gender.Female : Name.Gender.Male);
            var dob = Faker.Date.Between(DateTime.Now.AddYears(-100), DateTime.Now.AddYears(-18));
            
            var member = new Member
            {
                FirstName = fName,
                LastName = lName,
                PersonNumber = PersonNumber.GeneratePersonNumber(dob, isFemale),
                Membership = (Membership)Rnd.Next(membershipValues.Length),
            };

            members.Add(member);
        }

        return members;
    }

    // Generate Random vehicles
    private static List<VehicleType> GenerateVehicleTypes()
    {
        var vehicleTypes = new List<VehicleType>
        {
            new()
            {
                Name = "Car",
                Size = 3,
            },
            new()
            {
                Name = "Motorcycle",
                Size = 1,
            },
            new()
            {
                Name = "Truck",
                Size = 6,
            },
            new()
            {
                Name = "Bus",
                Size = 6,
            },
            new()
            {
                Name = "Boat",
                Size = 9,
            },
            new()
            {
                Name = "Airplane",
                Size = 9,
            }
        };
        return vehicleTypes;
    }

    // Generate Random Vehicle
    private static List<ParkedVehicle> GenerateVehicles(IEnumerable<VehicleType> vehicleTypes, IEnumerable<Member> members)
    {
        var parkedVehicles = new List<ParkedVehicle>();

        foreach (var member in members)
        {
            var numberOfVehicles = Rnd.Next(1, 4); // Randomly choose 1 to 3 vehicles per member

            for (var i = 0; i < numberOfVehicles; i++)
            {
                var model = Faker.Vehicle.Model();
                var regNr = $"SE {Faker.Random.String2(3, "ABCDEFGHIJKLMNOPQRSTUVWXYZ")}{Faker.Random.Number(100, 999)}";
                var color = Faker.Commerce.Color();
                var brand = Faker.Vehicle.Manufacturer();

                var currentVehicleType = vehicleTypes.ElementAt(Rnd.Next(vehicleTypes.Count()));

                var vehicle = new ParkedVehicle
                {
                    Brand = brand,
                    Model = model,
                    RegistrationNumber = regNr,
                    Color = color,
                    ArrivalTime = DateTime.MinValue,
                    VehicleType = currentVehicleType,
                    Member = member,
                };

                SetNumberOfWheels(vehicle);
                parkedVehicles.Add(vehicle);
            }
        }

        return parkedVehicles;
    }

    /// <summary>
    /// Sets the number of wheels based on the vehicle type
    /// </summary>
    private static void SetNumberOfWheels(ParkedVehicle vehicle)
    {
        vehicle.NumberOfWheels = vehicle.VehicleType.Name switch
        {
            "Car" => 4,
            "Bus" => 6,
            "Truck" => 6,
            "Motorcycle" => 2,
            "Boat" => 0,
            "Airplane" => 8,
            _ => vehicle.NumberOfWheels
        };
    }

}

