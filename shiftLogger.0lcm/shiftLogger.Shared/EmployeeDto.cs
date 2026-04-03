namespace shiftLogger.Shared;

public class EmployeeDto
{
    public string Name { get; set; }
    public int EmployeeId { get; set; }
    public int? ShiftsCompleted { get; set; } = null;
    public int? TotalLoggedHours { get; set; } = null;
}