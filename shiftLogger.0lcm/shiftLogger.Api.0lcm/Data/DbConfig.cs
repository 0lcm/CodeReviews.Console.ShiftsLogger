namespace shiftLoggerApi._0lcm.Data;

public class DbConfig
{
    public static string GetConnectionString()
    {
        return $"Data Source={Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "shiftLogger.0lcm",
            "api.db")}";
    }
}