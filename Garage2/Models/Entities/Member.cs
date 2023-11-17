using System.ComponentModel.DataAnnotations;
using Garage2.Validators;

namespace Garage2.Models.Entities;

#nullable disable
public class Member
{
    [Key]
    [PersonNumberValidator]
    public string PersonNumber { get; set; }

    [Required]
    [MaxLength(50)]
    [MinLength(1)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(50)]
    [MinLength(1)]
    public string LastName { get; set; }

    public Membership Membership { get; set; }
    //Navigation Property
    public ICollection<ParkedVehicle> Vehicles { get; set; }

}
