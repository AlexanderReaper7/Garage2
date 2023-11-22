using Garage2.Data;
using Garage2.Models;

namespace Garage2.Services;

public class VehicleTypesService : IVehicleTypesService
{
    private readonly Garage2Context _context;

    public VehicleTypesService(Garage2Context context)
    {
        _context = context;
    }

    public IEnumerable<VehicleType> AllTypes => _context.VehicleType;
}