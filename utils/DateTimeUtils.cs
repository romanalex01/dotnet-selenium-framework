using System.Globalization;

namespace dotnet_selenium_framework.utils;

public static class DateTimeUtils
{
    private const string ErrorMsg = "Input string is not in the correct format (e.g., 'Jan 01, 2000').";

    public static DateTime ConvertToDate(string input)
    {
        string[] formats = ["MMM dd, yyyy", "M/d/yyyy"];
        return DateTime.TryParseExact(input, formats,
            CultureInfo.InvariantCulture, DateTimeStyles.None, out var date)
            ? date
            : throw new FormatException(ErrorMsg);
    }
}