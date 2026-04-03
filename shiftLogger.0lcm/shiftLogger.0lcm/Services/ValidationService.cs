using System.Globalization;
using shiftLogger._0lcm.Configuration;
using shiftLogger._0lcm.ServiceContracts;

namespace shiftLogger._0lcm.Services;

public class ValidationService(DateFormatSettings dateFormatSettings) : IValidationService
{
    private readonly string[] _dateFormats = dateFormatSettings.DateFormats;
    private readonly string[] _hourFormats = dateFormatSettings.HourFormats;

    /// <summary>
    ///     Tries to parse a DateTime object following Iso 8601
    /// </summary>
    /// <param name="dateInput">the string containing the date</param>
    /// <param name="date">parsed date only</param>
    /// <param name="errorMessage">an error message if validation returned false, else is null</param>
    /// <returns>true upon parsing a valid date, else false</returns>
    public bool TryValidateDateTime(string dateInput, out DateOnly date, out string? errorMessage)
    {
        date = default;
        errorMessage = null;

        var validDate = DateTime.TryParseExact(
            dateInput.Trim(), _dateFormats,
            CultureInfo.InvariantCulture,
            DateTimeStyles.None, out var parsedDate);

        if (!validDate)
        {
            errorMessage = "Invalid date format, please use yyyy-MM-dd formatting instead.";
            return false;
        }

        date = DateOnly.FromDateTime(parsedDate);
        return true;
    }

    public bool TryValidateTimeOnly(string timeInput, out TimeOnly time, out string? errorMessage)
    {
        time = default;
        errorMessage = null;

        var validTime = TimeOnly.TryParseExact(
            timeInput.Trim(), _hourFormats,
            CultureInfo.InvariantCulture,
            DateTimeStyles.None, out var parsedTime);

        if (!validTime)
        {
            errorMessage = "Invalid time format, please use HH:mm formatting instead.";
            return false;
        }

        time = parsedTime;
        return true;
    }
}