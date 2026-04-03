using Microsoft.Extensions.Configuration;

namespace shiftLogger._0lcm.Configuration;

public class DateFormatSettings(IConfiguration config)
{
    internal readonly string[] DateFormats =
        config.GetSection("TimeFormats:DateFormats").Get<string[]>()
        ?? ["yyyy-MM-dd", "yyyy-MM-d", "yyyy-M-dd", "yyyy-M-d"];

    internal readonly string DateIso =
        config.GetSection("TimeFormats:DateIso").Get<string>()
        ?? "yyyy-MM-dd";

    internal readonly string[] HourFormats =
        config.GetSection("TimeFormats:HourFormats").Get<string[]>()
        ?? ["HH:mm", "HH:m", "H:mm", "H:m"];
}