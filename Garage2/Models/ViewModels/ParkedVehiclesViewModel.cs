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
    [Display(Name = "Parking Lot Nr")]
    public int ParkingSpace { get; set; }
    [Display(Name = "Parking Sub Space")]
    public int ParkingSubSpace { get; set; }
    public Member Member { get; set; }

}