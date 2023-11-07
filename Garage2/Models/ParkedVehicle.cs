using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Garage2.Models;
public enum VehicleType
{
    Car = 1,
    Motorcycle,
    Bus,
    Truck,
    Boat,
    Airplane
}
public static class VehicleTypeExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <returns>The number of sub-parking slots requierd to park this vehicle</returns>
    public static int GetVehicleSize(this VehicleType type)
    {
        switch (type)
        {
            case VehicleType.Car:
                return 3;
            case VehicleType.Motorcycle:
                return 1;
            case VehicleType.Bus:
                return 6;
            case VehicleType.Truck:
                return 6;
            case VehicleType.Boat:
                return 9;
            case VehicleType.Airplane:
                return 9;
            default:
                throw new ArgumentException($"{nameof(type)} is not a valid {nameof(VehicleType)}");
        }
    }

    public static List<VehicleType> GetAvailableTypesForParkingSpace(int spaceSize)
    {
        var types = Enum.GetValues(typeof(VehicleType)).Cast<VehicleType>().ToList();
        var output = new List<VehicleType>();
        foreach (var type in types)
        {
            if (type.GetVehicleSize() >= spaceSize)
            {
                output.Add(type);
            }
        }
        return output;
    }
}

public class ParkedVehicle
{
    [Key]
    public int Id { get; set; }
    [Display(Name = "Registration Number")]
    [RegistryNumberValidator]
    [StringLength(20)]

    public string RegistrationNumber { get; set; }
    [Display(Name = "Vehicle Type")]
    public VehicleType VehicleType { get; set; } // also store how many sub-slots this vehicle takes to park
    [StringLength(50)]
    public string Color { get; set; }
    [StringLength(50)]
    public string Brand { get; set; }
    [StringLength(50)]
    public string Model { get; set; }
    [Display(Name = "Number Of Wheels")]
    [Range(0, 99)]
    public int NumberOfWheels { get; set; }
    [Display(Name = "Arrival Time")]
    public DateTime ArrivalTime { get; set; }
    /// <summary>
    /// Where this vehicle is parked with the first int being index of whole parking slots and second the sub-slot.
    /// If the vehicle takes atleast 1 whole slot then the second int is always 0.
    /// </summary>
    public int ParkingSpace { get; set; }
    public int ParkingSubSpace { get; set; }

}

public class RegistryNumberValidator : ValidationAttribute
{
    /// <inheritdoc />
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult("value should not be null");
        }
        string inputStr = value.ToString()!;
        string[] parts = inputStr.ToUpperInvariant().Split(' ');
        if (parts.Length != 2)
        {
            return new ValidationResult("Invalid registry number string format");
        }
        try
        {
            NationCode _ = Enum.Parse<NationCode>(parts[0]);
        }
        catch (ArgumentException _)
        {
            return new ValidationResult($"{parts[0]} is not a valid nation code");
        }
        string serialNumber = parts[1];
        // ensure serial number contains only letters, digits and hyphens
        if (serialNumber.Any(c => !(char.IsLetterOrDigit(c) || c == '-')))
        {
            return new ValidationResult("Serial number must contain only letters, digits and hyphens");
        }

        return ValidationResult.Success;
    }
}

