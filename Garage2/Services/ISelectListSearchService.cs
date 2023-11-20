using Microsoft.AspNetCore.Mvc.Rendering;

namespace Garage2.Services;

public interface ISelectListSearchService
{
	IEnumerable<SelectListItem> GetSelectList();
}