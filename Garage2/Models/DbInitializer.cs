using Garage2.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Bogus;
using Bogus.DataSets;
using Bogus.Extensions.UnitedKingdom;
using Garage2.Migrations;
using Garage2.Models.Entities;
namespace Garage2.Models;

public class DbInitializer
{
	private static Faker faker = null!;
	private static Random rnd = new Random();
	public static async Task InitAsync(Garage2Context db)
	{
		//If there are members in database return
		if (await db.Member.AnyAsync()) return;

		faker = new Faker("sv");

		var members = GenerateMembers(10);
		await db.AddRangeAsync(members);
		await db.SaveChangesAsync(); // Save changes to make sure members are tracked

		var vehicleTypes = GenerateVehicleTypes(10);
		await db.AddRangeAsync(vehicleTypes);

		var vehicles = GenerateVehicles(vehicleTypes);
		await db.AddRangeAsync(vehicles);

		await db.SaveChangesAsync();
	}
	private static IEnumerable<Member> GenerateMembers(int numberOfMembers)
	{
		var members = new List<Member>();

		for (int i = 0; i < numberOfMembers; i++)
		{
			var fName = faker.Name.FirstName();
			var lName = faker.Name.LastName();
			var membership = Membership.Standard;
			var personNumber = faker.Date.Between(new DateTime(1965,1,2), new DateTime(2002,1,2));


			var member = new Member()
			{
				FirstName = fName,
				LastName = lName,
				PersonNumber = personNumber.ToLongDateString(),
				Membership = membership
			};

			members.Add(member);
		}
		return members;
	}
	private static IEnumerable<VehicleType> GenerateVehicleTypes(int generateVehicleTypes)
	{
		var vehicleTypes = new List<VehicleType>();

		for (int i = 0; i < generateVehicleTypes; i++)
		{
			var vehicleType = new VehicleType()
			{
				Name = GenerateRandomVehicleType()
			};
			vehicleTypes.Add(vehicleType);
		}

		return vehicleTypes;
	}

	private static string GenerateRandomVehicleType()
	{
		string[] vehicleTypeNames = new[] { "Car", "Bus", "Truck" };

		var typeName = vehicleTypeNames[rnd.Next(vehicleTypeNames.Length)];

		return typeName;
	}

	private static IEnumerable<ParkedVehicle> GenerateVehicles(IEnumerable<VehicleType> vehicleTypes)
	{
		var vehicles = new List<ParkedVehicle>();
		var startDate = new DateTime(2022, 1, 1);
		var endDate = new DateTime(2022, 12, 31);

			foreach (var vehicleType in vehicleTypes)
			{
				var model = faker.Vehicle.Model();
				var regNr = faker.Vehicle.GbRegistrationPlate(new DateTime(2001, 09, 2), new DateTime(2023, 1, 2));
				var color = faker.Commerce.Color();
				var arrivalTime = faker.Date.Between(startDate, endDate);
				var brand = faker.Vehicle.Manufacturer();
				//var parkingSpace = GenerateParkingSpace(vehicleTypes);

				var vehicle = new ParkedVehicle()
				{
					
					Brand = brand,
					Model = model,
					RegistrationNumber = regNr,
					Color = color,
					ArrivalTime = arrivalTime,
					VehicleType = new VehicleType()
					{
						Name = vehicleType.Name
					},
					NumberOfWheels = 4
				};
				//if (vehicle.VehicleType.Name == "Car")
				//{
				//	vehicle.NumberOfWheels = 4;
				//}
				vehicles.Add(vehicle);
			}
		
		return vehicles;
	}
}

