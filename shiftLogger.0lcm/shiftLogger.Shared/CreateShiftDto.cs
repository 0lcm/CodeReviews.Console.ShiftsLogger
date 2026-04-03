namespace shiftLogger.Shared;

public class CreateShiftDto
{
    public required DateOnly Date { get; set; }
    public required TimeOnly StartTime { get; set; }
    public required TimeOnly EndTime { get; set; }
}