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
    public VehicleType VehicleType { get; set; }
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
    public int ParkingslotNr { get; set; }

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

