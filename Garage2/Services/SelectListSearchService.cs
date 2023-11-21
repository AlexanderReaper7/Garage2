using Garage2.Data;
using Garage2.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Garage2.Services
{
	public class SelectListSearchService : ISelectListSearchService
	{
		private readonly Garage2Context context;
        private List<SelectListItem> selectList;
        public SelectListSearchService(Garage2Context context)
		{
			this.context = context;
            // Initialize selectList once at the beginning
            InitializeSelectList();
        }
        private void InitializeSelectList()
        {
            selectList = context.ParkedVehicle
                .Include(p => p.VehicleType)
                .Select(n => new SelectListItem
                {
                    Value = n.VehicleTypeName,
                    Text = n.VehicleTypeName
                }).ToList();
        }
        public IEnumerable<SelectListItem> GetSelectList()
		{
			selectList = context.VehicleType
                .Select(n => new SelectListItem
                {
                    Value = n.Name,
                    Text = n.Name
                }).Distinct().ToList();


            return selectList;
		}

        public void AddNewType(string newType)
        {
            var additionalType = new SelectListItem
            {
                Value = newType,
                Text = newType
            };

			selectList = selectList.Concat(new[]{additionalType}).ToList();
        }
    }
}
