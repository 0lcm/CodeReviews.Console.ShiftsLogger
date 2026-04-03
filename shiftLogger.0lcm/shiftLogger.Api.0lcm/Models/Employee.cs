namespace shiftLoggerApi._0lcm.Models;

public class Employee
{
    public required string Name { get; set; }
    public int EmployeeId { get; set; }
    public int? ShiftsCompleted { get; set; } = null;
    public int? TotalLoggedHours { get; set; } = null;
}