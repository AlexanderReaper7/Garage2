using System.ComponentModel.DataAnnotations;

namespace Garage2.Models.ViewModels
{
	public class VehicleStatics
	{
		[Display(Name = "Total Vehicle Types")]
		public VehicleType VehicleType { get; set; }
		[Display(Name = "Total Number Of Wheels")]
		[Range(0, 99)]
		public int NumberOfWheels { get; set; }
		[DisplayFormat(DataFormatString = "{0:N2}")]
		[Display(Name ="Total Price")]
		public decimal Price { get; set; }

		public Dictionary<VehicleType, int> VehicleTypeCounts { get; set; }
	}
}
