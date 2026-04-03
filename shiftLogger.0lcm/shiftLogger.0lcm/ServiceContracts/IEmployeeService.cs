using shiftLogger.Shared;

namespace shiftLogger._0lcm.ServiceContracts;

public interface IEmployeeService
{
    public Task<List<EmployeeDto>> GetAllEmployees();
    public Task<EmployeeDto> GetEmployeeById(int employeeId);
    public Task<List<EmployeeDto>> GetEmployeeByName(string name);
    public Task PostEmployee(string employeeName);
    public Task PutEmployee(int employeeId, string employeeName);
    public Task DeleteEmployee(int employeeId);
}