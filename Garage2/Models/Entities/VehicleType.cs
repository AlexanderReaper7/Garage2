using System.ComponentModel.DataAnnotations;
using Garage2.Models.Entities;

namespace Garage2.Models;

#nullable disable
public class VehicleType
{
    /// <summary>
    /// ex. Car, Motorcycle, Bus, Truck, Boat, Airplane
    /// </summary>
    [Key]
    public string Name { get; set; }

    /// <summary>
    /// How large this vehicle is in terms of parking slots.
    /// </summary>
    public int Size { get; set; }

    //Navigation Property
    public ICollection<ParkedVehicle> ParkedVehicle { get; set; }
}