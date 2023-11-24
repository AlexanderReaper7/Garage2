using System.Text;

namespace Garage2.Models;

public static class PersonNumber
{
    /// <summary>
    /// Formats a person number in the range 000101-0000 to 991231-9999 into
    /// a string in the format YYYYMMDDXXXX
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    public static void Normalize(ref string personNumber)
    {
        // YYMMDD-XXXX or YYMMDD+XXXX or YYMMDDXXXX
        if (personNumber.Length is 11 or 10)
        {
            AddCenturyBorn(ref personNumber);
        }

        if (personNumber.Length is 13)
        {
            personNumber = personNumber.ElementAt(8) switch
            {
                '+' => personNumber.Replace("+", ""),
                '-' => personNumber.Replace("-", ""),
                _ => throw new ArgumentException("Invalid format", nameof(personNumber))
            };
        }

        if (personNumber.Length is not 12) throw new ArgumentException("Invalid format", nameof(personNumber));
    }

    /// <summary>
    /// Formats a person number in the range 000101-0000 to 991231-9999 into
    /// a string in the format YYYYMMDDXXXX
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    public static string Normalize(string personNumber)
    {
        var temp = personNumber;
        Normalize(ref temp);
        return temp;
    }
    /// <summary>
    /// Formats a person number in the range 000101-0000 to 991231-9999 into
    /// a string in the format YYYYMMDDXXXX
    /// </summary>
    public static string? TryNormalize(string personNumber)
    {
        try
        {
            return Normalize(personNumber);
        }
        catch
        {
            return null;
        }
    }


    /// <summary>
    /// Adds the century to a person number of the format YYMMDD[+|-]XXXX
    /// </summary>
    /// <param name="personNumber"></param>
    private static void AddCenturyBorn(ref string personNumber)
    {
        var currentYearStr = DateTime.Now.Year.ToString();
        var currentCentury = int.Parse(currentYearStr[..2]);
        if (int.Parse(personNumber[..2]) <= int.Parse(currentYearStr[^2..]))
        {
            // the person was born this century
            if (personNumber.Contains('+')) currentCentury -= 2; // the person was born at least 2 centuries ago, could be more but there's not enough info
            personNumber = currentCentury + personNumber;
        }
        else personNumber = currentCentury - 1 + personNumber; // the person was born last century
    }

    /// <summary>
    /// Generates a valid person number for a person born on the specified date 
    /// </summary>
    /// <param name="dob">Date of Birth</param>
    /// <param name="isFemale">Is the person female?</param>
    /// <param name="withSeparator">should the format include the separator between date and serial?</param>
    /// <returns>A Person number with the format YYYYMMDD[-|+]XXXX</returns>
    public static string GeneratePersonNumber(DateTime dob, bool isFemale, bool withSeparator = false)
    {
        var strb = new StringBuilder();
        var year = dob.Year.ToString().PadLeft(4, '0');
        strb.Append(year);
        var month = dob.Month.ToString().PadLeft(2, '0');
        strb.Append(month);
        var day = dob.Day.ToString().PadLeft(2, '0');
        strb.Append(day);
        var two = Random.Shared.Next(100).ToString().PadLeft(2, '0');
        strb.Append(two);
        var last = Random.Shared.Next(1, 6);
        if (isFemale) last = last * 2 % 10;
        strb.Append(last);
        var checkNum = CalculateCheckNumber(strb.ToString()[2..]);
        strb.Append(checkNum);
        if (withSeparator) strb.Insert(8, dob.Year <= DateTime.Now.Year - 100 ? '+' : '-');
        return strb.ToString();
    }

    public static string InsertSeparator(this string personNumber)
    {
        if (personNumber.Length is not 12) throw new ArgumentException("Invalid format", nameof(personNumber));
        var year = int.Parse(personNumber[..4]);
        var separator = year <= DateTime.Now.Year - 100 ? '+' : '-';
        return personNumber.Insert(8, separator.ToString());
    }

    /// <summary>
    /// Calculates mod10 checkNumber for a person number in the format YYMMDDXXX, note that the check number is not included
    /// </summary>
    /// <param name="nr">The person number in the format YYMMDDXXX</param>
    /// <returns>The check number</returns>
    public static int CalculateCheckNumber(string nr)
    {
        var sum = Mod10Sum(nr);
        return (10 - sum % 10) % 10;
    }

    /// <summary>
    /// Uses the Mod10 check number algorithm to validate a number
    /// </summary>
    /// <param name="nr">The person number in format YYMMDDXXXX</param>
    /// <returns>Is valid</returns>
    public static bool ValidateCheckNumber(string nr)
    {
        if (nr.Length is 12) nr = nr[2..];
        else if (nr.Length is not 10) return false;
        var sum = Mod10Sum(nr);
        return sum % 10 == 0;
    }

    private static int Mod10Sum(string nr)
    {
        var sum = 0;
        var alternate = true;
        var digits = nr.Select(digit => int.Parse(digit.ToString())).ToArray();
        for (var i = 0; i < digits.Length; i++)
        {
            if (alternate)
            {
                digits[i] *= 2;
                if (digits[i] > 9)
                {
                    digits[i] = digits[i] % 10 + 1;
                }
            }

            sum += digits[i];
            alternate = !alternate;
        }

        return sum;
    }
}
