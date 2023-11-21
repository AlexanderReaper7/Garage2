using System.ComponentModel.DataAnnotations;
using Garage2.Validators;

namespace Garage2.Models.Entities;

#nullable disable
public class Member
{
    //[Range(18, int.MaxValue, ErrorMessage = "You must be at least 18 years old to perform this action.")]
    [Key]
    [PersonNumberValidator]
    [Display(Name = "Birth Number")]
    public string PersonNumber { get; set; }

    
    [Required]
    [MaxLength(50)]
    [MinLength(1)]
    [FirstNameNotEqualToLastName(ErrorMessage = "First name and last name cannot be the same.")]
	public string FirstName { get; set; }

   
    [Required]
    [MaxLength(50)]
    [MinLength(1)]
	[FirstNameNotEqualToLastName(ErrorMessage = "First name and last name cannot be the same.")]
	public string LastName { get; set; }
    [Display(Name = "Owner")]
    public string FullName => $"{FirstName} {LastName}";
    public Membership Membership { get; set; }

    //Navigation Property
    public ICollection<ParkedVehicle> ParkedVehicle { get; set; }

}
