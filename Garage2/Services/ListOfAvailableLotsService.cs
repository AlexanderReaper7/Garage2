using Garage2.Controllers;
using Garage2.Data;
using Garage2.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Garage2.Services;

public class ListOfAvailableLotsService : IListOfAvailableLotsService
{
    private readonly IParkingLotManager _parkingLotManager;
    private readonly Garage2Context _context;
    /// <summary>
    /// The VehicleTypes that are available to park
    /// </summary>
    public IQueryable<VehicleType> AvailableTypes
    {
        get
        {
            var largest = _parkingLotManager.LargestParkingSpaceAvailable;
            return _context.VehicleType.Where(t => t.Size <= largest);
        }
    }

    public ListOfAvailableLotsService(IParkingLotManager parkingLotManager, Garage2Context context)
    {
        _parkingLotManager = parkingLotManager;
        _context = context;
    }

    /// <summary>
    /// Returns a list of SelectListItem of the AvailableTypes
    /// </summary>
    /// <returns></returns>
    public IEnumerable<SelectListItem> AvailableTypesAsSelectList()
    {
        return AvailableTypes.Select(v => new SelectListItem(v.Name, v.Name));
    }
}