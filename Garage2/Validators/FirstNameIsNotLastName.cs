using System.ComponentModel.DataAnnotations;
using Garage2.Models.Entities;

namespace Garage2.Validators;

[AttributeUsage(AttributeTargets.Property)]
public class FirstNameIsNotLastName : ValidationAttribute
{
    protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
    {
        var member = (Member)validationContext.ObjectInstance;

        if (member.FirstName == member.LastName)
        {
            return new ValidationResult(ErrorMessage ?? "First name and last name cannot be the same.");
        }

        return ValidationResult.Success;
    }
}