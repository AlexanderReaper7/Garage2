namespace Garage2.Models.Entities;

public enum Membership
{
    Standard,
    Pro,

    //public int Id { get; set; }
    //public string Name { get; set; }
    //public double InitalCost { get; set; }
    //public double TimeCost { get; set; }
}

public static class MembershipExtensions {
    public static double CalculateCost(this Membership membership, DateTime from, DateTime to)
    {
        throw new NotImplementedException();
    }
}