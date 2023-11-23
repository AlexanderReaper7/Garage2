using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Garage2.Validators;

namespace Garage2.Models.Entities;

#nullable disable
public class ParkedVehicle
{
    [Key] public int Id { get; set; }

    [Display(Name = "Registration Number")]
    [RegistryNumberValidator]
    [StringLength(20)]

    public string RegistrationNumber { get; set; }

    [StringLength(50)] public string Color { get; set; }
    [StringLength(50)] public string Brand { get; set; }
    [StringLength(50)] public string Model { get; set; }

    [Display(Name = "Number Of Wheels")]
    [Range(0, 30)]
    public int NumberOfWheels { get; set; }

    [Display(Name = "Arrival Time")] public DateTime ArrivalTime { get; set; }

    /// <summary>
    /// Where this vehicle is parked with the first int being index of whole parking slots and second the sub-slot.
    /// If This value is 0, then the vehicle is not parked.
    /// If the vehicle takes atleast 1 whole slot then the second int is always 0.
    /// </summary>
    [Display(Name = "Parking Lot Nr")]
    public int ParkingSpace { get; set; }

    [Display(Name = "Parking Sub Space")]
    public int ParkingSubSpace { get; set; }

    /// <summary>
    /// A formatted string representing the parking space.
    /// </summary>
    /// <returns>
    /// The parking space as a string in the format "slot,sub-slot : slot,sub-slot" where the first slot is the starting position of the vehicle.
    /// If the vehicle only occupies 1 whole slot or sub slot, then the second position is omitted and the format becomes "slot,sub-slot"
    /// If the vehicle is not parked, then the string "Not Parked" is returned.
    /// </returns>
    [Display(Name = "Parking Space")]
    public string ParkingSpaceString
    {
        get
        {
            if (ParkingSpace == 0) return "Not Parked";
            var strb = new StringBuilder();
            strb.Append(ParkingSpace);
            if (ParkingSubSpace != 0 || VehicleType.Size == 1)
            {
                strb.Append(',');
                strb.Append(ParkingSubSpace);
            }

            // if the vehicle occupies more than 1 whole parking lot or sub lot, then append the end position
            if (VehicleType.Size != 1 && VehicleType.Size != IParkingLotManager.ParkingSubLotSize)
            {
                strb.Append(" : ");
                var end = ParkingSubSpace + VehicleType.Size;
                var whole = end / IParkingLotManager.ParkingSubLotSize;
                if (whole > 0) strb.Append(ParkingSpace + whole);
                var sub = end % IParkingLotManager.ParkingSubLotSize;
                if (sub > 0)
                {
                    strb.Append(',');
                    strb.Append(sub);
                }
            }
            return strb.ToString();
        }
    }

    // Foreign Key
    [Display(Name = "Owner")]
    public string MemberPersonNumber { get; set; }
    [Display(Name = "Vehicle Type")]
    public string VehicleTypeName { get; set; }

    // Navigation Property
    [Display(Name = "Owner")]
    public Member Member { get; set; }
    [Display(Name = "Vehicle Type")]
    public VehicleType VehicleType { get; set; }

}


