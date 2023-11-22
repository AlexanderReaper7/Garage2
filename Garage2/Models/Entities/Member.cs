using System.ComponentModel.DataAnnotations;
using Garage2.Validators;

namespace Garage2.Models.Entities;

#nullable disable
public class Member
{
    [Key]
    [PersonNumberValidator]
    [Display(Name = "Person Number")]
    public string PersonNumber { get; set; }

    [Required]
    [MaxLength(50)]
    [MinLength(1)]
    [Display(Name = "First name")]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(50)]
    [MinLength(1)]
    [Display(Name = "Last name")]
    public string LastName { get; set; }
    [Display(Name = "Owner")]
    public string FullName => $"{FirstName} {LastName}";
    public Membership Membership { get; set; }

    //Navigation Property
    public ICollection<ParkedVehicle> ParkedVehicle { get; set; }

}
