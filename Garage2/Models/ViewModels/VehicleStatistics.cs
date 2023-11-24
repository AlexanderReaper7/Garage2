using System.ComponentModel.DataAnnotations;
using Garage2.Models.Entities;

namespace Garage2.Models.ViewModels;

public class VehicleStatistics
{
    [Display(Name = "Total Number Of Wheels")]
    [Range(0, 30)]
    public int NumberOfWheels { get; set; }
    [Display(Name ="Total Earnings")]
    [DataType(DataType.Currency)]
    public decimal Price { get; set; }
    public Dictionary<string, int> VehicleCounts { get; set; } = new();
    public Member Member { get; set; }
    [Display(Name = "Total Members")]
    public int NrOfMembers { get; set; }
    public Dictionary<Membership, int> Memberships { get; set; } = new();
}