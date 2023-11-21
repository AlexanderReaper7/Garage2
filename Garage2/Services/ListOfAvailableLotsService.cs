using Garage2.Controllers;
using Garage2.Data;
using Garage2.Models;
using Garage2.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Garage2.Services;

public class ListOfAvailableLotsService : IListOfAvailableLotsService
{
	private readonly Garage2Context context;

	public ListOfAvailableLotsService(Garage2Context context)
	{
		this.context = context;
	}
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

	public ListOfAvailableLotsService(IParkingLotManager parkingLotManager, Garage2Context context)
	{
		this.parkingLotManager = parkingLotManager;
		this.context = context;
	}

	public IEnumerable<SelectListItem> GetVehicleTypesForList()
	{
		// Assuming you want to convert AvailableLotsList to a list of SelectListItem
		//var selectList = AvailableTypes.Select(v => new SelectListItem { Value = v.ToString(), Text = v.ToString() });
		var selectList = context.ParkedVehicle.Select(v => new SelectListItem { Value = v.VehicleType.Name, Text = v.VehicleType.Name }).Distinct();

		return selectList;
	}
}