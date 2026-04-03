using shiftLogger.Shared;

namespace shiftLoggerApi._0lcm.ServiceContracts;

public interface IShiftService
{
    public List<ShiftDto> GetAllUpcomingShifts();
    public List<ShiftDto> GetAllAvailableShifts();
    public List<ShiftDto> GetShiftsForDate(DateOnly date);
    public ShiftDto? GetShiftById(int shiftId);
    public List<ShiftDto> GetShiftsUpcomingForEmployee(int employeeId);
    public Task PublishNewShift(CreateShiftDto shiftDto);
    public Task<bool?> AssignShiftToEmployee(int shiftId, int employeeId);
    public Task<bool?> RemoveShiftFromEmployee(int shiftId);
    public Task RemoveAllUpcomingShiftsFromEmployee(int employeeId);
    public Task AssignShiftAsCompleted(int shiftId);
    public Task<bool?> EditShift(int shiftId, CreateShiftDto shiftDto);
    public Task<bool?> DeleteShift(int shiftId);
}