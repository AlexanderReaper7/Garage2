using Garage2.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Garage2.Services;

public interface IAvailableLotsService
{
    /// <summary>
    /// The VehicleTypes that are available to park
    /// </summary>
    IQueryable<VehicleType> AvailableTypes { get; }
    /// <summary>
    /// Returns a list of SelectListItem of the AvailableTypes
    /// </summary>
    IEnumerable<SelectListItem> AvailableTypesAsSelectList();
    /// <summary>
    /// Returns true if the vehicle type is available to park
    /// </summary>
    Task<bool> IsAvailable(string vehicleTypeName);
    /// <summary>
    /// Returns true if the vehicle type is available to park
    /// </summary>
    public bool IsAvailable(int vehicleSize);

}