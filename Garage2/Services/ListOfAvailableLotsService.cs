using Garage2.Controllers;
using Garage2.Data;
using Garage2.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Garage2.Services;

public class ListOfAvailableLotsService : IListOfAvailableLotsService
{
	readonly IParkingLotManager parkingLotManager;

	/// <summary>
	/// 
	/// </summary>
	public List<VehicleType> AvailableTypes
	{
		get
		{
			var largest = parkingLotManager.LargestParkingSpaceAvailable;
			//var availableTypes = context.VehicleType.Select(v => v.Size <= largest);
			//return VehicleTypeExtensions.GetAvailableTypesForParkingSpace(largest);
			throw new NotImplementedException();
		}
	}

	public ListOfAvailableLotsService(IParkingLotManager parkingLotManager)
	{
		this.parkingLotManager = parkingLotManager;
	}

	public IEnumerable<SelectListItem> GetVehicleTypesForList()
	{

		// Assuming you want to convert AvailableLotsList to a list of SelectListItem
		var selectList = AvailableTypes.Select(v => new SelectListItem { Value = v.ToString(), Text = v.ToString() });
		return selectList;
	}
}