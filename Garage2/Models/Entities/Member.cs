using System.ComponentModel.DataAnnotations;
using Garage2.Validators;

namespace Garage2.Models.Entities;

#nullable disable
public class Member
{
    [Key]
    [Required]
    [PersonNumberValidator]
    [Display(Name = "Person Number")]
    public string PersonNumber { get; set; }

    [Required]
    [MaxLength(50)]
    [MinLength(1)]
    [FirstNameIsNotLastName(ErrorMessage = "Cannot be the same as Last Name.")]
    [Display(Name = "First name")]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(50)]
    [MinLength(1)]
    [FirstNameIsNotLastName(ErrorMessage = "Cannot be the same as First Name.")]
    [Display(Name = "Last name")]
    public string LastName { get; set; }

    [Display(Name = "Owner")]
    public string FullName => $"{FirstName} {LastName}";
    [Required]
    public Membership Membership { get; set; }

    //Navigation Property
    public ICollection<ParkedVehicle> ParkedVehicle { get; set; }

}
