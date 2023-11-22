using Garage2.Models;
using System.ComponentModel.DataAnnotations;

namespace Garage2.Validators;

public class RegistryNumberValidator : ValidationAttribute
{
    /// <inheritdoc />
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult("value should not be null");
        }
        string inputStr = value.ToString()!;
        string[] parts = inputStr.ToUpperInvariant().Split(' ');
        if (parts.Length != 2)
        {
            return new ValidationResult("Invalid registry number string format");
        }
        try
        {
            NationCode _ = Enum.Parse<NationCode>(parts[0]);
        }
        catch (ArgumentException)
        {
            return new ValidationResult($"{parts[0]} is not a valid nation code");
        }
        string serialNumber = parts[1];
        // ensure serial number contains only letters, digits and hyphens
        if (serialNumber.Any(c => !(char.IsLetterOrDigit(c) || c == '-')))
        {
            return new ValidationResult("Serial number must contain only letters, digits and hyphens");
        }

        return ValidationResult.Success;
    }
}
