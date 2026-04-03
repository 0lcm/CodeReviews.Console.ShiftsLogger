using System.Text.Json;
using shiftLogger._0lcm.ServiceContracts;
using shiftLogger.Shared;

namespace shiftLogger._0lcm.Services;

public class ShiftService(IShiftApiService apiService) : IShiftService
{
    public async Task<List<ShiftDto>> GetUpcomingShifts()
    {
        var rawJson = await apiService.GetUpcomingShifts();

        return JsonSerializer.Deserialize<List<ShiftDto>>(rawJson, GetJsonOptions())!;
    }

    public async Task<List<ShiftDto>> GetAvailableShifts()
    {
        var rawJson = await apiService.GetAvailableShifts();

        return JsonSerializer.Deserialize<List<ShiftDto>>(rawJson, GetJsonOptions())!;
    }

    public async Task<List<ShiftDto>> GetShiftsByDate(DateOnly date)
    {
        var rawJson = await apiService.GetShiftsByDate(date);

        return JsonSerializer.Deserialize<List<ShiftDto>>(rawJson, GetJsonOptions())!;
    }

    public async Task<ShiftDto> GetShiftById(int shiftId)
    {
        var rawJson = await apiService.GetShiftById(shiftId);

        return JsonSerializer.Deserialize<ShiftDto>(rawJson, GetJsonOptions())!;
    }

    public async Task<List<ShiftDto>> GetShiftsForEmployee(int employeeId)
    {
        var rawJson = await apiService.GetShiftsForEmployee(employeeId);

        return JsonSerializer.Deserialize<List<ShiftDto>>(rawJson, GetJsonOptions())!;
    }

    public async Task PostShift(DateOnly date, TimeOnly startTime, TimeOnly endTime)
    {
        var shiftDto = new CreateShiftDto
        {
            Date = date,
            StartTime = startTime,
            EndTime = endTime
        };

        await apiService.PostShift(shiftDto);
    }

    public async Task PutAssignShift(int shiftId, int employeeId)
    {
        await apiService.AssignShift(shiftId, employeeId);
    }

    public async Task PutRemoveShift(int shiftId)
    {
        await apiService.RemoveShift(shiftId);
    }

    public async Task PutRemoveAllShiftsFromEmployee(int employeeId)
    {
        await apiService.RemoveAllShiftsFromEmployee(employeeId);
    }

    public async Task PutCompleteShift(int shiftId)
    {
        await apiService.CompleteShift(shiftId);
    }

    public async Task PutEditShift(int shiftId, DateOnly date, TimeOnly start, TimeOnly end)
    {
        var shiftDto = new CreateShiftDto
        {
            Date = date,
            StartTime = start,
            EndTime = end
        };

        await apiService.EditShift(shiftId, shiftDto);
    }

    public async Task DeleteShift(int shiftid)
    {
        await apiService.DeleteShift(shiftid);
    }

    //------- Helper Methods -------
    private JsonSerializerOptions GetJsonOptions()
    {
        return new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }
}