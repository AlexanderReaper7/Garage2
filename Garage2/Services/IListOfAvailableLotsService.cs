using Garage2.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Garage2.Services;

public interface IListOfAvailableLotsService
{
    IEnumerable<SelectListItem> GetVehicleTypesForList();
    //MembershipModel GetMembershipTypesForList();
    List<SelectListItem> GetMembershipTypesForList();
}
