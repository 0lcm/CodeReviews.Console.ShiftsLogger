using System.Text.Json;
using shiftLogger._0lcm.ServiceContracts;
using shiftLogger.Shared;

namespace shiftLogger._0lcm.Services;

public class EmployeeService(IEmployeeApiService apiService) : IEmployeeService
{
    public async Task<List<EmployeeDto>> GetAllEmployees()
    {
        var rawJson = await apiService.RequestAllEmployees();

        return JsonSerializer.Deserialize<List<EmployeeDto>>(rawJson, GetJsonOptions())!;
    }

    public async Task<EmployeeDto> GetEmployeeById(int employeeId)
    {
        var rawJson = await apiService.RequestEmployeeById(employeeId);

        return JsonSerializer.Deserialize<EmployeeDto>(rawJson, GetJsonOptions())!;
    }

    public async Task<List<EmployeeDto>> GetEmployeeByName(string name)
    {
        var rawJson = await apiService.RequestEmployeeByName(name);

        return JsonSerializer.Deserialize<List<EmployeeDto>>(rawJson, GetJsonOptions())!;
    }

    public async Task PostEmployee(string employeeName)
    {
        if (string.IsNullOrWhiteSpace(employeeName))
            throw new ArgumentNullException(nameof(employeeName));

        var dto = new CreateEmployeeDto
        {
            Name = employeeName
        };

        await apiService.PostEmployee(dto);
    }

    public async Task PutEmployee(int employeeId, string employeeName)
    {
        if (string.IsNullOrWhiteSpace(employeeName))
            throw new ArgumentNullException(nameof(employeeName));

        var dto = new CreateEmployeeDto
        {
            Name = employeeName
        };

        await apiService.PutEmployee(employeeId, dto);
    }

    public async Task DeleteEmployee(int employeeId)
    {
        await apiService.DeleteEmployee(employeeId);
    }

    //------- Helper Methods -------
    private JsonSerializerOptions GetJsonOptions()
    {
        return new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }
}