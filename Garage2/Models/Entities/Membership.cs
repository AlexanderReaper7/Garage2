//using System.Web.Mvc;

using Microsoft.AspNetCore.Mvc.Rendering;

namespace Garage2.Models.Entities;

public enum Membership
{
    Standard,
    Pro,
}

public class MembershipModel
{
    public Membership SelectedMembership { get; set; }
    public List<SelectListItem> Memberships { get; set; }
}
public static class MembershipExtensions
{
    public static double CalculateCost(this Membership membership, DateTime from, DateTime to)
    {
        throw new NotImplementedException();
    }
}
