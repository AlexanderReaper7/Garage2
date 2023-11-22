using Garage2.Models;

namespace Garage2.Services;

public interface IVehicleTypesService
{
    IEnumerable<VehicleType> AllTypes { get; }
}