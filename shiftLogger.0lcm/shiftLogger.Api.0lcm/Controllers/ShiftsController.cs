using Microsoft.AspNetCore.Mvc;
using shiftLogger.Shared;
using shiftLoggerApi._0lcm.ServiceContracts;

namespace shiftLoggerApi._0lcm.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShiftsController(IShiftService shiftService) : ControllerBase
{
    [HttpGet]
    public IActionResult GetUpcomingShifts()
    {
        var shifts = shiftService.GetAllUpcomingShifts();
        return Ok(shifts);
    }

    [HttpGet("available")]
    public IActionResult GetAllAvailableShifts()
    {
        var shifts = shiftService.GetAllAvailableShifts();
        return Ok(shifts);
    }

    [HttpGet("{date}")]
    public IActionResult GetShiftsByDate(DateOnly date)
    {
        ;
        var shifts = shiftService.GetShiftsForDate(date);
        return Ok(shifts);
    }

    [HttpGet("{shiftId:int}")]
    public IActionResult GetShiftById(int shiftId)
    {
        var shift = shiftService.GetShiftById(shiftId);
        if (shift is null)
            return NotFound();

        return Ok(shift);
    }

    [HttpGet("employee/{employeeId:int}")]
    public IActionResult GetShiftsForEmployee(int employeeId)
    {
        var shifts = shiftService.GetShiftsUpcomingForEmployee(employeeId);
        return Ok(shifts);
    }

    [HttpPost]
    public async Task<IActionResult> PublishNewShift(CreateShiftDto createShiftDto)
    {
        var success = await shiftService.PublishNewShift(createShiftDto);
        return success switch
        {
            true => Created(),
            false => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }

    [HttpPut("assign-shift/shift{shiftId:int}/employee{employeeId:int}")]
    public async Task<IActionResult> AssignShiftToEmployee(int shiftId, int employeeId)
    {
        var success = await shiftService.AssignShiftToEmployee(shiftId, employeeId);
        if (success is null)
            return NotFound();
        if (success == false)
            return StatusCode(StatusCodes.Status500InternalServerError);

        return NoContent();
    }

    [HttpPut("remove-shift/shift{shiftId:int}")]
    public async Task<IActionResult> RemoveShiftFromEmployee(int shiftId)
    {
        var success = await shiftService.RemoveShiftFromEmployee(shiftId);

        if (success is null)
            return NotFound();
        if (success == false)
            return StatusCode(StatusCodes.Status500InternalServerError);

        return NoContent();
    }

    [HttpPut("remove-shift/employee{employeeId:int}")]
    public IActionResult RemoveAllUpcomingShiftsFromEmployee(int employeeId)
    {
        shiftService.RemoveAllUpcomingShiftsFromEmployee(employeeId);
        return NoContent();
    }

    [HttpPut("complete-shift/shift{shiftId:int}")]
    public IActionResult CompleteShift(int shiftId)
    {
        shiftService.AssignShiftAsCompleted(shiftId);
        return NoContent();
    }

    [HttpPut("{shiftId:int}")]
    public async Task<IActionResult> EditShift(int shiftId, [FromBody] CreateShiftDto createShiftDto)
    {
        var success = await shiftService.EditShift(shiftId, createShiftDto);
        if (success is null)
            return NotFound();
        if (success == false)
            return StatusCode(StatusCodes.Status500InternalServerError);

        return NoContent();
    }

    [HttpDelete("{shiftId:int}")]
    public async Task<IActionResult> DeleteShift(int shiftId)
    {
        var success = await shiftService.DeleteShift(shiftId);
        if (success is null)
            return NotFound();
        if (success == false)
            return StatusCode(StatusCodes.Status500InternalServerError);

        return NoContent();
    }
}