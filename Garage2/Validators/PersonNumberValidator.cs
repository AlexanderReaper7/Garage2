using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Garage2.Validators;

public class PersonNumberValidator : ValidationAttribute
{
    private const string InvalidFormatMsg = "Invalid person number format, the valid format is YYMMDD-XXXX";
    /// <summary>
    /// Validates a person number
    ///
    /// A person number is a string of 10 digits, where the first 6 digits are the birth date in the format YYMMDD
    /// and the next 3 digits are a birth number.
    /// the last digit is a control digit and functions as a checksum.
    /// See "https://en.wikipedia.org/wiki/Personal_identity_number_(Sweden)"
    /// </summary>
    /// <param name="value"></param>
    /// <param name="validationContext"></param>
    /// <returns></returns>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null) return new ValidationResult("value should not be null");
        if (value is string str)
        {
            string dateStr = "";
            switch (str.Length)
            {
                // Determine if the input is in the format YYMMDD-XXXX or YYYYMMDD-XXXX, and if the - is missing or replaced with a +
                case 11: // YYMMDD-XXXX or YYMMDD+XXXX
                    // if the - has been replaced with +, then the person is going to be >= 100 years old this year
                    if (str.Contains("+"))
                    {
                        // Strip the + and add 100 to the year
                        str = str.Replace("+", "");
                        // get current year - 100
                        int currentYear = DateTime.Now.Year - 100;
                        dateStr = currentYear.ToString().Substring(0,2) + str.Substring(0, 6);
                    }
                    else if (str.Contains("-"))
                    {
                        // strip the '-' from the input
                        str = str.Replace("-", "");
                        dateStr = DateTime.Now.Year.ToString().Substring(0, 2) + str.Substring(0, 6);
                    }
                    break;
                case 10: // YYMMDDXXXX
                    dateStr = DateTime.Now.Year.ToString().Substring(0, 2) + str.Substring(0, 6);
                    break;
                case 13: // YYYYMMDD-XXXX or YYYYMMDD+XXXX
                    // strip the + or -
                    str = str.Replace("+", "").Replace("-", "");
                    dateStr = str.Substring(0, 8);
                    str = str.Substring(2, 10);
                    break;
                case 12: // YYYYMMDDXXXX
                    dateStr = str.Substring(0, 8);
                    str = str.Substring(2,10);
                    break;
                default:
                    return new ValidationResult(InvalidFormatMsg);
            }
            // the format is now YYMMDDXXXX
            if (str.Length != 10) return new ValidationResult(InvalidFormatMsg);
            // All characters are digits
            if (str.Any(c => !char.IsDigit(c))) return new ValidationResult(InvalidFormatMsg);
            // The date is a valid date
            if (!DateTime.TryParseExact(dateStr, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _)) return new ValidationResult($"{dateStr} is not a valid date");
            // The sequence is a valid Luhn checksum
            if (!ValidateLuhn(str)) return new ValidationResult($"{str} is not a valid person number");
            return ValidationResult.Success;
        }
        return new ValidationResult("value is not a string");
    }
    /// <summary>
    /// Uses the Luhn algorithm to validate a number
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    private static bool ValidateLuhn(string number)
    {
        int sum = 0;
        bool alternate = true;
        for (int i = 0; i < number.Length; i++)
        {
            int n = int.Parse(number.Substring(i, 1));
            if (alternate)
            {
                n *= 2;
                if (n > 9)
                {
                    n = (n % 10) + 1;
                }
            }
            sum += n;
            alternate = !alternate;
        }
        return (sum % 10 == 0);
    }
}