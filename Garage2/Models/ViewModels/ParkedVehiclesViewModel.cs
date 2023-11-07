using System.ComponentModel.DataAnnotations;

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

    public int ParkingSpace { get; set; }
    public int ParkingSubSpace { get; set; }
}