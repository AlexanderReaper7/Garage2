using System.ComponentModel.DataAnnotations;

namespace Garage2.Models.ViewModels;

public class VehicleStatistics
{
    [Display(Name = "Total Vehicle Types")]
    public VehicleType VehicleType { get; set; }
    [Display(Name = "Total Number Of Wheels")]
    [Range(0, 30)]
    public int NumberOfWheels { get; set; }
    [Display(Name ="Total Earnings")]
    [DisplayFormat(DataFormatString = "{0:N2}")]
    public decimal Price { get; set; }
    public Dictionary<string, int> VehicleCounts { get; set; } = new Dictionary<string, int>();
}