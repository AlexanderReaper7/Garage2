using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Garage2.Validators;

namespace Garage2.Models.Entities;

#nullable disable
public class ParkedVehicle
{
    [Key]
    public int Id { get; set; }
    [Display(Name = "Registration Number")]
    [RegistryNumberValidator]
    [StringLength(20)]

    public string RegistrationNumber { get; set; }
    [StringLength(50)]
    public string Color { get; set; }
    [StringLength(50)]
    public string Brand { get; set; }
    [StringLength(50)]
    public string Model { get; set; }
    [Display(Name = "Number Of Wheels")]
    [Range(0, 30)]
    public int NumberOfWheels { get; set; }
    [Display(Name = "Arrival Time")]
    public DateTime ArrivalTime { get; set; }
    /// <summary>
    /// Where this vehicle is parked with the first int being index of whole parking slots and second the sub-slot.
    /// If This value is 0, then the vehicle is not parked.
    /// If the vehicle takes atleast 1 whole slot then the second int is always 0.
    /// </summary>
    [Display(Name = "Parking Lot Nr")]
    public int ParkingSpace { get; set; }
    [Display(Name = "Parking Sub Space")]
    public int ParkingSubSpace { get; set; }
    // Foreign Key
    public int MemberId { get; set; }
    public int VehicleTypeId { get; set; }

    // Navigation Property
    [Display(Name = "Owner")]
    public Member Member { get; set; }
    [Display(Name = "Vehicle Type")]
    public VehicleType VehicleType { get; set; }

}


