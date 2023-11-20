namespace Garage2.Models.Entities;

public enum Membership
{
    Standard,
    Pro,
}

public static class MembershipExtensions
{
    public static double CalculateCost(this Membership membership, DateTime from, DateTime to)
    {
        throw new NotImplementedException();
    }
}