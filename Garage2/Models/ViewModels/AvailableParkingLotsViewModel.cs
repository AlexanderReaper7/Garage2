using System.ComponentModel.DataAnnotations;

namespace Garage2.Models.ViewModels
{
	public class AvailableParkingLotsViewModel
	{
		[Display(Name = "Regular Size")]
		public int AvailableParkingLotsRegularSize { get; set; }
		[Display(Name = "Medium Size")]
		public int AvailableParkingLotsMediumSize { get; set; }
		[Display(Name = "Large Size")]
		public int AvailableParkingLotsLargeSize { get; set; }
	}
}
