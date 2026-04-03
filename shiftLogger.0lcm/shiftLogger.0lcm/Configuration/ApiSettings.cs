using System.Text.Json;

namespace shiftLogger._0lcm.Configuration;

internal class ApiSettings
{
    internal static string BaseUrl { get; } = GetBaseUrl();

    //------- Employee Urls -------
    internal static string GetAllEmployeesUrl { get; } = $"{BaseUrl}Employees";
    internal static string GetEmployeeByIdIncompleteUrl { get; } = $"{BaseUrl}Employees/";
    internal static string GetEmployeeByNameIncompleteUrl { get; } = $"{BaseUrl}Employees/";
    internal static string PostEmployeeUrl { get; } = $"{BaseUrl}Employees";
    internal static string PutEmployeeIncompleteUrl { get; } = $"{BaseUrl}Employees/";
    internal static string DeleteEmployeeIncompleteUrl { get; } = $"{BaseUrl}Employees/";

    //------- Shift Urls -------
    internal static string GetUpcomingShiftsUrl { get; } = $"{BaseUrl}Shifts";
    internal static string GetAvailableShiftsUrl { get; } = $"{BaseUrl}Shifts/available";
    internal static string GetShiftsByDateIncompleteUrl { get; } = $"{BaseUrl}Shifts/";
    internal static string GetShiftByIdIncompleteUrl { get; } = $"{BaseUrl}Shifts/";
    internal static string GetShiftsForEmployeeIncompleteUrl { get; } = $"{BaseUrl}Shifts/Employee/";
    internal static string PostShiftUrl { get; } = $"{BaseUrl}Shifts";
    internal static string PutAssignShiftIncompleteUrl { get; } = $"{BaseUrl}Shifts/assign-shift/shift";
    internal static string PutRemoveShiftIncompleteUrl { get; } = $"{BaseUrl}Shifts/remove-shift/shift";

    internal static string PutRemoveAllShiftsFromEmployeeIncompleteUrl { get; } =
        $"{BaseUrl}Shifts/remove-shift/employee";

    internal static string PutCompleteShiftIncompleteUrl { get; } = $"{BaseUrl}Shifts/complete-shift/shift";
    internal static string PutEditShiftIncompleteUrl { get; } = $"{BaseUrl}Shifts/";
    internal static string DeleteShiftIncompleteUrl { get; } = $"{BaseUrl}Shifts/";

    //------- Helper Methods -------
    private static string GetBaseUrl()
    {
        var solutionRoot = GetSolutionDirectory();
        var launchSettingsPath = Path.Combine(
            solutionRoot,
            "shiftLogger.Api.0lcm",
            "Properties",
            "launchSettings.json");

        if (File.Exists(launchSettingsPath))
        {
            var json = File.ReadAllText(launchSettingsPath);
            using var doc = JsonDocument.Parse(json);
            var url = doc.RootElement
                .GetProperty("profiles")
                .GetProperty("http")
                .GetProperty("applicationUrl")
                .GetString();

            if (string.IsNullOrEmpty(url))
                throw new Exception("Could not find the application url in the Api's launchSettings.json");

            var baseUrl = url.Split(';').First();
            return $"{baseUrl}/api/";
        }

        throw new Exception("Could not find the Api's launchSettings.json");
    }

    private static string GetSolutionDirectory()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory != null)
        {
            if (directory.GetFiles("*.sln").Any())
                return directory.FullName;

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException("Could not find the solution directory");
    }
}