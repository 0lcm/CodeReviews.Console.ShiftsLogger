using shiftLogger.Shared;

namespace shiftLoggerApi._0lcm.ServiceContracts;

public interface IEmployeeService
{
    public List<EmployeeDto> GetAllEmployees();
    public EmployeeDto? GetEmployeeById(int id);
    public List<EmployeeDto> GetEmployeeByName(string name);
    /// <summary>
    /// Adds a new employee to the database
    /// </summary>
    /// <param name="createEmployeeDto"></param>
    /// <returns>true on a succesful creation, else false</returns>
    public Task<bool> AddEmployee(CreateEmployeeDto createEmployeeDto);
    public Task EditEmployee(int employeeId, CreateEmployeeDto createEmployeeDto);
    public Task DeleteEmployee(int employeeId);
}