namespace Garage2.Models;

public enum Membership
{
    STANDARD,
    PRO,
}

public static class MembershipExtensions
{
    public static double CalculateCost(this Membership membership, DateTime from, DateTime to)
    {
        throw new NotImplementedException();
    }
}