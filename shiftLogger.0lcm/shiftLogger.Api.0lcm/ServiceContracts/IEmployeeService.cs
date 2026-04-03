using shiftLogger.Shared;

namespace shiftLoggerApi._0lcm.ServiceContracts;

public interface IEmployeeService
{
    public List<EmployeeDto> GetAllEmployees();
    public EmployeeDto? GetEmployeeById(int id);
    public List<EmployeeDto> GetEmployeeByName(string name);
    public Task AddEmployee(CreateEmployeeDto createEmployeeDto);
    public Task EditEmployee(int employeeId, CreateEmployeeDto createEmployeeDto);
    public Task DeleteEmployee(int employeeId);
}