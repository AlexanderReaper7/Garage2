using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Garage2.Models;

namespace Garage2.Validators;

public class PersonNumberValidator : ValidationAttribute
{
    private const string InvalidFormatMsg = "Invalid person number format, the valid formats are 10 or 12 digits with or without hyphen";
    /// <summary>
    /// Validates a person number,
    /// see <a href="https://en.wikipedia.org/wiki/Personal_identity_number_(Sweden)">Wikipedia</a>
    /// </summary>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not string str) return new ValidationResult("value is not a string");
        // try to convert into YYYYMMDDXXXX
        try
        {
            PersonNumber.Normalize(ref str);
        }
        catch (Exception)
        {
            return new ValidationResult(InvalidFormatMsg);
        }
        // All characters are digits
        if (!str.All(char.IsDigit)) return new ValidationResult("Invalid person number format, only digits are allowed");
        // The date is a valid date
        DateTime dob;
        if (!DateTime.TryParseExact(str[..8], "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dob)) return new ValidationResult($"{str[..8]} is not a valid date");
        // Is at least 18 years old
        if (dob > DateTime.Now.AddYears(-18)) return new ValidationResult("Must be at least 18 years old");
        // The sequence is a valid Luhn checksum
        if (!PersonNumber.ValidateCheckNumber(str)) return new ValidationResult("Invalid check number");
        return ValidationResult.Success;
    }
}