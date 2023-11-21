using Microsoft.AspNetCore.Mvc.Rendering;

namespace Garage2.Services;

public interface IAddNewVehicleTypes
{
    IEnumerable<SelectListItem> GetSelectListOfNewTypes();
}