using Microsoft.AspNetCore.Mvc.Rendering;

namespace Garage2.Services;

public interface IAddNewVehicleTypesService
{
    IEnumerable<SelectListItem> GetSelectListOfNewTypes();
}