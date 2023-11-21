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
            selectList = new List<SelectListItem>();
        }
        //Temporary solution?, depending if we want to show vehicle types without the need of it being connected to a vehicle.
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
            //Adds the new created Type to the existing seleclist
            selectList = selectList.Concat(new[] { additionalType }).ToList();
        }
    }
}
