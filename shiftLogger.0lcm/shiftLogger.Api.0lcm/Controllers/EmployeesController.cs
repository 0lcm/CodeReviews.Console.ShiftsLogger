using Microsoft.AspNetCore.Mvc;
using shiftLogger.Shared;
using shiftLoggerApi._0lcm.ServiceContracts;

namespace shiftLoggerApi._0lcm.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController(IEmployeeService employeeService) : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllEmployees()
    {
        var employees = employeeService.GetAllEmployees();
        return Ok(employees);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetEmployeeById(int id)
    {
        var employee = employeeService.GetEmployeeById(id);

        if (employee is null) return NotFound();

        return Ok(employee);
    }

    [HttpGet("{name}")]
    public IActionResult GetEmployeeByName(string name)
    {
        var employee = employeeService.GetEmployeeByName(name);

        return Ok(employee);
    }

    [HttpPost]
    public IActionResult AddEmployee(CreateEmployeeDto employeeDto)
    {
        employeeService.AddEmployee(employeeDto);
        return Created();
    }

    [HttpPut("{id:int}")]
    public IActionResult EditEmployee(int id, [FromBody] CreateEmployeeDto employeeDto)
    {
        employeeService.EditEmployee(id, employeeDto);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteEmployee(int id)
    {
        employeeService.DeleteEmployee(id);
        return NoContent();
    }
}