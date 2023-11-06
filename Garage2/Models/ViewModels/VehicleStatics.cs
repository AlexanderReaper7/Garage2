using System.ComponentModel.DataAnnotations;

namespace Garage2.Models.ViewModels
{
	public class VehicleStatics
	{
		[Display(Name = "Vehicle Type")]
		public VehicleType VehicleType { get; set; }
		[Display(Name = "Number Of Wheels")]
		[Range(0, 99)]
		public int NumberOfWheels { get; set; }
		[DisplayFormat(DataFormatString = "{0:N2}")]
		public decimal Price { get; set; }
	}
}
