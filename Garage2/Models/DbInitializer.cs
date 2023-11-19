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
using NuGet.Packaging;

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
		//	await db.SaveChangesAsync(); // Save changes to make sure members are tracked

		var vehicleTypes = GenerateVehicleTypes(10);
		await db.AddRangeAsync(vehicleTypes);

		var vehicles = GenerateVehicles(vehicleTypes, members);
		await db.AddRangeAsync(vehicles);
		await db.SaveChangesAsync();

		// Add the generated vehicles to the corresponding members
		foreach (var member in members)
		{
			var correspondingVehicle = vehicles.FirstOrDefault(v => v.Member == member);

			if (correspondingVehicle != null)
			{
				member.ParkedVehicleId = correspondingVehicle.Id;
			}
		}

		await db.SaveChangesAsync();

		// Add the generated vehicleType to the corresponding vehicle
		foreach (var type in vehicleTypes)
		{
			var correspondingVehicle = vehicles.FirstOrDefault(v => v.VehicleTypeId == type.Id);

			if (correspondingVehicle != null)
			{
				type.ParkedVehicleId = correspondingVehicle.Id;
			}
		}

		await db.SaveChangesAsync();
	}

	//Generate Random Members
	private static IEnumerable<Member> GenerateMembers(int numberOfMembers)
	{
		var members = new List<Member>();

		for (int i = 0; i < numberOfMembers; i++)
		{
			var fName = faker.Name.FirstName();
			var lName = faker.Name.LastName();
			//var membership = Membership.Standard;
			var personNumber = faker.Date.Between(new DateTime(1965, 1, 2), new DateTime(2002, 1, 2));
			Array membershipValues = Enum.GetValues(typeof(Membership));

			var Random = new Random();
			var member = new Member()
			{
				FirstName = fName,
				LastName = lName,
				PersonNumber = personNumber.ToLongDateString(),
				Membership = (Membership)membershipValues.GetValue(rnd.Next(membershipValues.Length))!,
			};

			members.Add(member);
		}
		return members;
	}//Generate Random vehicles
	private static IEnumerable<VehicleType> GenerateVehicleTypes(int generateVehicleTypes)
	{
		var vehicleTypes = new List<VehicleType>();

		for (int i = 0; i < generateVehicleTypes; i++)
		{
			var name = GenerateRandomVehicleType();
			var size = GenerateSizeDependingOnName(name);

			var vehicleType = new VehicleType()
			{
				Name = name,
				Size = size
			};
			vehicleTypes.Add(vehicleType);
		}

		return vehicleTypes;
	}

	private static double GenerateSizeDependingOnName(string name)
	{
		switch (name)
		{
			case "Car":
				return 1;
			case "Truck":
			case "Bus":
				return 2;
		}
		return 0;
	}

	//Generate Random Vehicle Types
	private static string GenerateRandomVehicleType()
	{
		string[] vehicleTypeNames = new[] { "Car", "Bus", "Truck" };

		var typeName = vehicleTypeNames[rnd.Next(vehicleTypeNames.Length)];

		return typeName;
	}
	//Generate Random Vehicle
	private static IEnumerable<ParkedVehicle> GenerateVehicles(IEnumerable<VehicleType> vehicleTypes, IEnumerable<Member> members)
	{
		var parkedVehicle = new List<ParkedVehicle>();
		var startDate = new DateTime(2023, 1, 1);
		var endDate = new DateTime(2023, 12, 31);

		var random = new Random();
		var index = 0;
		foreach (var member in members)
		{

			var model = faker.Vehicle.Model();
			var regNr = faker.Vehicle.GbRegistrationPlate(new DateTime(2001, 09, 2), new DateTime(2023, 1, 2));
			var color = faker.Commerce.Color();
			var arrivalTime = faker.Date.Between(startDate, endDate);
			var brand = faker.Vehicle.Manufacturer();
			
			// Get The list from start to end
			var currentVehicleType = vehicleTypes.ElementAt(index);

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
			index++;
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
				vehicle.ParkingSpace = 1;
				break;
			case "Bus":
			case "Truck":
				vehicle.NumberOfWheels = 8;
				vehicle.ParkingSpace = 2;
				break;
		}
	}
}

