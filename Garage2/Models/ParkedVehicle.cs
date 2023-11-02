using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Garage2.Models;
public enum VehicleType
{
    A,
    A1,
    B,
    B1,
    BE,
    C,
    C1,
    C1E,
    CE,
    D,
    D1,
    D1E,
    DE,
}

public static class RegistryNumber
{
    //public RegistryNumber(NationCode Nationality, string SerialNumber)
    //{
    //    this.Nationality = Nationality;
    //    // ensure serial number contains only letters, digits and hyphens
    //    if (SerialNumber.Any(c => !(char.IsLetterOrDigit(c) || c == '-')))
    //    {
    //        throw new ArgumentException("Serial number must contain only letters, digits and hyphens");
    //    }
    //    this.SerialNumber = SerialNumber.ToUpperInvariant();
    //}

    //public RegistryNumber()
    //{
        
    //}
    //public NationCode Nationality { get; }
    //public string SerialNumber { get; }

    //[Key]
    //public string RegNum => ToString();

    //public static ToRegistryNumber(string registryNumberString)
    //{
    //    string[] parts = registryNumberString.ToUpperInvariant().Split(' ');
    //    if (parts.Length != 2)
    //    {
    //        throw new ArgumentException("Invalid registry number string format");
    //    }
    //    NationCode nationality = Enum.Parse<NationCode>(parts[0]);
    //    string serialNumber = parts[1];
    //    return $"{nationality} {serialNumber}";
    //}

    //public override string ToString()
    //{
    //    return $"{Nationality} {SerialNumber}";
    //}

}


public class ParkedVehicle
{
    [Key]
    public int Id { get; set; }
    //[Remote()]
    public string RegistrationNumber { get; set; }
    public VehicleType VehicleType { get; set; }
    public string Color { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public int NumberOfWheels { get; set; }
    public DateTime ArrivalTime { get; set; }

}


public class CustomValidator : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        //
        return true;
    }
}

