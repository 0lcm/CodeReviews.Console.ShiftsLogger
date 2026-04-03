using shiftLogger.Shared;
using shiftLoggerApi._0lcm.Data;
using shiftLoggerApi._0lcm.Models;
using shiftLoggerApi._0lcm.ServiceContracts;

namespace shiftLoggerApi._0lcm.Services;

public class ShiftService(ApiDbContext db) : IShiftService
{
    public List<ShiftDto> GetAllUpcomingShifts()
    {
        return db.Shifts
            .Where(s => s.Date >= DateOnly.FromDateTime(DateTime.Today))
            .Select(s => new ShiftDto
            {
                ShiftId = s.ShiftId,
                Date = s.Date,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                Duration = s.Duration,
                Completed = s.Completed,
                EmployeeId = s.EmployeeId
            })
            .ToList();
    }

    public List<ShiftDto> GetAllAvailableShifts()
    {
        return db.Shifts
            .Where(s => s.EmployeeId == null && s.Completed == false && s.Date >= DateOnly.FromDateTime(DateTime.Today))
            .Select(s => new ShiftDto
            {
                ShiftId = s.ShiftId,
                Date = s.Date,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                Duration = s.Duration,
                Completed = s.Completed,
                EmployeeId = s.EmployeeId
            })
            .ToList();
    }

    public List<ShiftDto> GetShiftsForDate(DateOnly date)
    {
        return db.Shifts
            .Where(s => s.Date == date)
            .Select(s => new ShiftDto
            {
                ShiftId = s.ShiftId,
                Date = s.Date,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                Duration = s.Duration,
                Completed = s.Completed,
                EmployeeId = s.EmployeeId
            })
            .ToList();
    }

    public ShiftDto? GetShiftById(int shiftId)
    {
        var shift = db.Shifts.Find(shiftId);
        if (shift is null)
            return null;

        return new ShiftDto
        {
            ShiftId = shift.ShiftId,
            Date = shift.Date,
            StartTime = shift.StartTime,
            EndTime = shift.EndTime,
            Duration = shift.Duration,
            Completed = shift.Completed,
            EmployeeId = shift.EmployeeId
        };
    }

    public List<ShiftDto> GetShiftsUpcomingForEmployee(int employeeId)
    {
        return db.Shifts
            .Where(s => s.EmployeeId == employeeId && s.Completed == false)
            .Select(s => new ShiftDto
            {
                ShiftId = s.ShiftId,
                Date = s.Date,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                Duration = s.Duration,
                Completed = s.Completed,
                EmployeeId = s.EmployeeId
            })
            .ToList();
    }

    public async Task PublishNewShift(CreateShiftDto shiftDto)
    {
        var shift = new Shift
        {
            Date = shiftDto.Date,
            StartTime = shiftDto.StartTime,
            EndTime = shiftDto.EndTime,
            Duration = shiftDto.EndTime - shiftDto.StartTime
        };

        db.Shifts.Add(shift);
        await db.SaveChangesAsync();
    }

    /// <summary>
    ///     assigns a shift to an employee
    /// </summary>
    /// <param name="shiftId">id of the shift</param>
    /// <param name="employeeId">if of the employee</param>
    /// <returns>true on success, null on a null argument, and false on an unexpected failure</returns>
    public async Task<bool?> AssignShiftToEmployee(int shiftId, int employeeId)
    {
        var shift = db.Shifts.Find(shiftId);
        var employee = db.Employees.Find(employeeId);

        if (shift is null)
            return null;
        if (employee is null)
            return null;

        try
        {
            shift.Employee = employee;
            shift.EmployeeId = employee.EmployeeId;

            await db.SaveChangesAsync();
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    ///     removes a shift from an employee
    /// </summary>
    /// <param name="shiftId">id of the shift</param>
    /// <returns>true upon success, null upon a null argument, and false upon an unexpected failure</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task<bool?> RemoveShiftFromEmployee(int shiftId)
    {
        var shift = db.Shifts.Find(shiftId);

        if (shift is null)
            return null;

        try
        {
            shift.Employee = null;
            shift.EmployeeId = null;

            await db.SaveChangesAsync();
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }

    public async Task RemoveAllUpcomingShiftsFromEmployee(int employeeId)
    {
        var shifts = db.Shifts
            .Where(s => s.EmployeeId == employeeId && s.Completed == false &&
                        s.Date == DateOnly.FromDateTime(DateTime.Today));

        foreach (var shift in shifts)
        {
            shift.EmployeeId = null;
            shift.Employee = null;
        }

        await db.SaveChangesAsync();
    }

    /// <summary>
    ///     Edits a shift
    /// </summary>
    /// <param name="shiftId">id of shift</param>
    /// <param name="shiftDto">shiftDto containing details to edit</param>
    /// <returns>true upon success, null upon a null argument, and false upon an unexpected failure</returns>
    public async Task<bool?> EditShift(int shiftId, CreateShiftDto shiftDto)
    {
        var shift = db.Shifts.Find(shiftId);
        if (shift is null)
            return null;

        var newShift = new Shift
        {
            ShiftId = shift.ShiftId,
            Date = shiftDto.Date,
            StartTime = shiftDto.StartTime,
            EndTime = shiftDto.EndTime,
            Duration = shiftDto.EndTime - shiftDto.StartTime,
            Completed = shift.Completed,
            Employee = shift.Employee,
            EmployeeId = shift.EmployeeId
        };

        try
        {
            db.Entry(shift).CurrentValues.SetValues(newShift);
            await db.SaveChangesAsync();
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    ///     deletes a shift from the database
    /// </summary>
    /// <param name="shiftId">id of shift</param>
    /// <returns>true upon success, null upon a null argument, and false upon an unexpected failure</returns>
    public async Task<bool?> DeleteShift(int shiftId)
    {
        var shift = db.Shifts.Find(shiftId);
        if (shift is null)
            return null;

        try
        {
            db.Shifts.Remove(shift);
            await db.SaveChangesAsync();
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }

    public Task AssignShiftAsCompleted(int shiftId)
    {
        var shift = db.Shifts.Find(shiftId);
        if (shift is null)
            throw new ArgumentNullException(nameof(shift));

        shift.Completed = true;
        return db.SaveChangesAsync();
    }
}