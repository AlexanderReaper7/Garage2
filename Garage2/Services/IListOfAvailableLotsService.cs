using Microsoft.AspNetCore.Mvc.Rendering;

namespace Garage2.Services;

public interface IListOfAvailableLotsService
{
    IEnumerable<SelectListItem> AvailableTypesAsSelectList();
}