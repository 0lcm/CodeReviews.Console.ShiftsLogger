namespace shiftLogger._0lcm.ServiceContracts;

public interface IValidationService
{
    public bool TryValidateDateTime(string dateInput, out DateOnly date, out string? errorMessage);
    public bool TryValidateTimeOnly(string timeInput, out TimeOnly time, out string? errorMessage);
}