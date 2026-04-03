using shiftLogger.Shared;

namespace shiftLogger._0lcm.ServiceContracts;

public interface IShiftService
{
    public Task<List<ShiftDto>> GetUpcomingShifts();
    public Task<List<ShiftDto>> GetAvailableShifts();
    public Task<List<ShiftDto>> GetShiftsByDate(DateOnly date);
    public Task<ShiftDto> GetShiftById(int shiftId);
    public Task<List<ShiftDto>> GetShiftsForEmployee(int employeeId);
    public Task PostShift(DateOnly date, TimeOnly startTime, TimeOnly endTime);
    public Task PutAssignShift(int shiftId, int employeeId);
    public Task PutRemoveShift(int shiftId);
    public Task PutRemoveAllShiftsFromEmployee(int employeeId);
    public Task PutCompleteShift(int shiftId);
    public Task PutEditShift(int shiftId, DateOnly date, TimeOnly start, TimeOnly end);
    public Task DeleteShift(int shiftId);
}