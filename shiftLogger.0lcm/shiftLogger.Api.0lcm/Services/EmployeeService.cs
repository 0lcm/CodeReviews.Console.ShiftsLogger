using shiftLogger.Shared;
using shiftLoggerApi._0lcm.Data;
using shiftLoggerApi._0lcm.Models;
using shiftLoggerApi._0lcm.ServiceContracts;

namespace shiftLoggerApi._0lcm.Services;

public class EmployeeService(ApiDbContext db) : IEmployeeService
{
    public List<EmployeeDto> GetAllEmployees()
    {
        return db.Employees
            .Select(e => new EmployeeDto
            {
                EmployeeId = e.EmployeeId,
                Name = e.Name,
                ShiftsCompleted = e.ShiftsCompleted,
                TotalLoggedHours = e.TotalLoggedHours
            })
            .ToList();
    }

    public EmployeeDto? GetEmployeeById(int id)
    {
        var employee = db.Employees.Find(id);

        if (employee is null) return null;

        return new EmployeeDto
        {
            EmployeeId = employee.EmployeeId,
            Name = employee.Name,
            ShiftsCompleted = employee.ShiftsCompleted,
            TotalLoggedHours = employee.TotalLoggedHours
        };
    }

    public List<EmployeeDto> GetEmployeeByName(string name)
    {
        return db.Employees
            .Where(e => e.Name.ToLower() == name.ToLower())
            .Select(e => new EmployeeDto
            {
                EmployeeId = e.EmployeeId,
                Name = e.Name,
                ShiftsCompleted = e.ShiftsCompleted,
                TotalLoggedHours = e.TotalLoggedHours
            })
            .ToList();
    }

    public async Task<bool> AddEmployee(CreateEmployeeDto createEmployeeDto)
    {
        var employee = new Employee
        {
            Name = createEmployeeDto.Name
        };
        db.Employees.Add(employee);
        await db.SaveChangesAsync();

        return GetEmployeeById(employee.EmployeeId) is not null;
    }

    public async Task EditEmployee(int employeeId, CreateEmployeeDto employeeDto)
    {
        var oldEmployee = await db.Employees.FindAsync(employeeId);

        if (oldEmployee is null) throw new ArgumentNullException(nameof(oldEmployee));

        var employee = GetEmployeeById(employeeId);
        db.Entry(oldEmployee).CurrentValues.SetValues(employeeDto);

        await db.SaveChangesAsync();
    }

    public async Task DeleteEmployee(int employeeId)
    {
        var employee = db.Employees.Find(employeeId);

        if (employee is null)
            return;

        db.Employees.Remove(employee);
        await db.SaveChangesAsync();
    }
}