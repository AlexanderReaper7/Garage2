using Garage2.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Garage2.Services
{
	public class SelectListSearchService : ISelectListSearchService
	{
		private readonly Garage2Context context;

		public SelectListSearchService(Garage2Context context)
		{
			this.context = context;
		}
		public IEnumerable<SelectListItem> GetSelectList()
		{
			var selectList = context.ParkedVehicle
				.Select(t => t.VehicleType)
				.Distinct()
				.Select(n => new SelectListItem
				{
					Value = n.Name,
					Text = n.Name
				});
			
			return selectList;
		}
	}
}
