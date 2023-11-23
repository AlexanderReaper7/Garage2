using Garage2.Controllers;
using Garage2.Data;
using Garage2.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Garage2.Services;

public class AvailableLotsService : IAvailableLotsService
{
    private readonly IParkingLotManager _parkingLotManager;
    private readonly Garage2Context _context;

    public IQueryable<VehicleType> AvailableTypes
    {
        get
        {
            var largest = _parkingLotManager.LargestParkingSpaceAvailable;
            return _context.VehicleType.Where(t => t.Size <= largest);
        }
    }

    public AvailableLotsService(IParkingLotManager parkingLotManager, Garage2Context context)
    {
        _parkingLotManager = parkingLotManager;
        _context = context;
    }

    public IEnumerable<SelectListItem> AvailableTypesAsSelectList()
    {
        return AvailableTypes.Select(v => new SelectListItem(v.Name, v.Name));
    }

    public async Task<bool> IsAvailable(string vehicleTypeName)
    {
        var largest = _parkingLotManager.LargestParkingSpaceAvailable;
        var type = await _context.VehicleType.FindAsync(vehicleTypeName);
        return type != null && type.Size <= largest;
    }

    public bool IsAvailable(int vehicleSize)
    {
        var largest = _parkingLotManager.LargestParkingSpaceAvailable;
        return vehicleSize <= largest;
    }
}