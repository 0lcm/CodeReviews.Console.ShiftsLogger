using shiftLogger.Shared;

namespace shiftLogger._0lcm.ServiceContracts;

public interface IEmployeeApiService
{
    public Task<string> RequestAllEmployees();
    public Task<string> RequestEmployeeById(int employeeId);
    public Task<string> RequestEmployeeByName(string name);
    public Task PostEmployee(CreateEmployeeDto employeeDto);
    public Task PutEmployee(int employeeId, CreateEmployeeDto employeeDto);
    public Task DeleteEmployee(int employeeId);
}