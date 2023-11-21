using Garage2.Data;
using Garage2.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Garage2.Services
{
    public class AddNewVehicleTypes : IAddNewVehicleTypes
    {
        private readonly Garage2Context context;

        public AddNewVehicleTypes(Garage2Context context)
        {
            this.context = context;
        }

        public IEnumerable<SelectListItem> GetSelectListOfNewTypes()
        {
            var addNewTypes = Enum.GetValues(typeof(VehicleTypesEnum));

            var selectList = new List<SelectListItem>();

            foreach (var type in addNewTypes)
            {
                selectList.Add(new SelectListItem
                {
                    Value = type.ToString(),
                    Text = type.ToString()
                });
            }

            return selectList;
        }
    }
}
