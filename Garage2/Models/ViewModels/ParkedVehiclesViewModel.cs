using System.ComponentModel.DataAnnotations;
using Garage2.Models.Entities;

namespace Garage2.Models.ViewModels;

public class ParkedVehiclesViewModel
{
    public int Id { get; set; }
    [Display(Name = "Registration Number")]
    public string RegistrationNumber { get; set; }
    [Display(Name ="Vehicle Type")]
    public VehicleType VehicleType { get; set; }
    [Display(Name ="Arrival Time")]
    public DateTime ArrivalTime { get; set; }
    [Display(Name = "Parked at")]
    public string ParkingSpaceString { get; set; }
    [Display(Name = "Owner")]
    public Member Member { get; set; }

}