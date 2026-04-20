namespace shiftLoggerApi._0lcm.Models;

public class Shift
{
    public int ShiftId { get; set; }
    public required DateOnly Date { get; set; }
    public required TimeOnly StartTime { get; set; }
    public required TimeOnly EndTime { get; set; }
    public required TimeSpan Duration { get; set; }
    public bool Completed { get; set; } = false;

    public int? EmployeeId { get; set; } = null;
                                                       public Employee? Employee { get; set; } = null;
}