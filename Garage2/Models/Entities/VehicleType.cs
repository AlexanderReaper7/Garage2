using Garage2.Models.Entities;

namespace Garage2.Models;

#nullable disable
public class VehicleType
{
    public int Id { get; set; }
    /// <summary>
    /// ex. Car, Motorcycle, Bus, Truck, Boat, Airplane
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// How large this vehicle is in terms of parking slots.
    /// </summary>
    public double Size { get; set; }
    //Foreign Key
    public int ParkedVehicleId { get; set; }
	//Navigation Property
	public ICollection<ParkedVehicle> ParkedVehicle { get; set; }
}