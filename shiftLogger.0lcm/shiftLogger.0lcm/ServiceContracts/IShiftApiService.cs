using shiftLogger.Shared;

namespace shiftLogger._0lcm.ServiceContracts;

public interface IShiftApiService
{
    public Task<string> GetUpcomingShifts();
    public Task<string> GetAvailableShifts();
    public Task<string> GetShiftsByDate(DateOnly date);
    public Task<string> GetShiftById(int shiftId);
    public Task<string> GetShiftsForEmployee(int employeeId);
    public Task PostShift(CreateShiftDto shiftDto);
    public Task AssignShift(int shiftId, int employeeId);
    public Task RemoveShift(int shiftId);
    public Task RemoveAllShiftsFromEmployee(int employeeId);
    public Task CompleteShift(int shiftId);
    public Task EditShift(int shiftId, CreateShiftDto shiftDto);
    public Task DeleteShift(int shiftId);
}