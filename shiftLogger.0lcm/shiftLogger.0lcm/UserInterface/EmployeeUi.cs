using shiftLogger._0lcm.Enums;
using shiftLogger._0lcm.ServiceContracts;
using shiftLogger.Shared;
using Spectre.Console;
using Spectre.Console.Rendering;
using static shiftLogger._0lcm.UserInterface.DisplayHelper;

namespace shiftLogger._0lcm.UserInterface;

internal class EmployeeUi(IEmployeeService employeeService)
{
    private const string BackOption = "Back";

    //------- Menu Methods -------
    internal async Task EmployeeMenu()
    {
        while (true)
        {
            Console.Clear();

            var option = DisplayMenu<EmployeeMenuOption>();
            var shouldReturn = await HandleMenuOption(option);

            if (!shouldReturn)
                continue;

            return;
        }
    }

    /// <summary>
    ///     Handles menu option.
    /// </summary>
    /// <param name="option"></param>
    /// <returns>True if the menu should return, false if not.</returns>
    private async Task<bool> HandleMenuOption(EmployeeMenuOption option)
    {
        switch (option)
        {
            case EmployeeMenuOption.ShowAllEmployees:
                await ShowAllEmployees();
                break;
            case EmployeeMenuOption.SearchEmployeeById:
                await ShowEmployeeById();
                break;
            case EmployeeMenuOption.SearchEmployeeByName:
                await ShowEmployeeByName();
                break;
            case EmployeeMenuOption.AddNewEmployee:
                await CreateEmployee();
                break;
            case EmployeeMenuOption.EditExistingEmployee:
                await UpdateEmployee();
                break;
            case EmployeeMenuOption.DeleteEmployee:
                await DeleteEmployee();
                break;
            case EmployeeMenuOption.Back:
                return true;
        }

        return false;
    }

    //------- Ui Methods -------
    private async Task ShowAllEmployees()
    {
        Console.Clear();

        var employees = await employeeService.GetAllEmployees();

        var iRenderable = BuildRenderableEmployees(employees);
        DisplayRows(iRenderable);

        DisplayPrompt(new List<string> { BackOption });
    }

    private async Task ShowEmployeeById()
    {
        int parsedId;
        while (true)
        {
            Console.Clear();

            DisplayInfo("Enter 'Back' to return to the menu.");
            var id = DisplayQuestion("Please enter an Employee ID: ");
            if (id.ToLower() == BackOption.ToLower()) return;
            if (int.TryParse(id, out parsedId)) break;

            DisplayWarning("Please enter a valid Employee ID containing only numbers: ");
        }

        EmployeeDto employee;
        try
        {
            employee = await employeeService.GetEmployeeById(parsedId);
        }
        catch (HttpRequestException ex)
        {
            if ((int)ex.StatusCode! == 404)
                DisplayWarning($"Employee with ID {parsedId} not found.");
            else
                DisplayWarning(ex.Message);

            WaitForUser();
            return;
        }

        var iRenderable = BuildRenderableEmployees(new List<EmployeeDto> { employee });

        DisplayRows(iRenderable);
        DisplayPrompt(new List<string> { BackOption }, "\n Press back to return:");
    }

    private async Task ShowEmployeeByName()
    {
        DisplayInfo("Enter 'Back' to return.");
        var name = DisplayQuestion("Please enter an Employee Name: ");
        if (name.ToLower() == BackOption.ToLower()) return;

        List<EmployeeDto> employees;
        try
        {
            employees = await employeeService.GetEmployeeByName(name);
        }
        catch (HttpRequestException ex)
        {
            DisplayWarning(ex.Message);

            WaitForUser();
            return;
        }

        if (employees.Any())
        {
            var iRenderable = BuildRenderableEmployees(employees);

            DisplayRows(iRenderable);
        }
        else
        {
            DisplayMessage("No Employees found.");
        }

        DisplayPrompt(new List<string> { BackOption }, "\nPress back to return:");
    }

    private async Task CreateEmployee()
    {
        while (true)
        {
            DisplayInfo("Enter 'Back' to return.");
            var name = DisplayQuestion("Please enter an Employee Name: ");
            if (name.ToLower() == BackOption.ToLower()) return;

            try
            {
                await employeeService.PostEmployee(name);
                DisplaySuccess("Employee was added to the database.");
                WaitForUser();
                return;
            }
            catch (ArgumentNullException)
            {
                DisplayWarning("The name you entered was null or empty. Please try again.");
            }
            catch (HttpRequestException ex)
            {
                DisplayWarning(ex.Message);
                WaitForUser();
                return;
            }
        }
    }

    private async Task UpdateEmployee()
    {
        int parsedId;
        while (true)
        {
            Console.Clear();
            DisplayInfo("Enter 'Back' to return.");
            var id = DisplayQuestion("Please enter an Employee ID: ");
            if (id.ToLower() == BackOption.ToLower()) return;
            if (!int.TryParse(id, out parsedId))
            {
                DisplayWarning("Please enter a valid Employee ID containing only numbers.");
                WaitForUser();
                continue;
            }

            try
            {
                var name = DisplayQuestion("Please enter a new name for the employee: ");

                await employeeService.PutEmployee(parsedId, name);
                DisplaySuccess("Employee was updated.");
                WaitForUser();
                return;
            }
            catch (ArgumentNullException)
            {
                DisplayWarning("The name you entered was null or empty. Please try again.");
            }
            catch (HttpRequestException ex)
            {
                DisplayWarning(ex.Message);
            }
        }
    }

    private async Task DeleteEmployee()
    {
        int parsedInt;
        while (true)
        {
            DisplayInfo("Enter 'Back' to return.");
            var id = DisplayQuestion("Please enter an Employee ID: ");
            if (!int.TryParse(id, out parsedInt))
                DisplayWarning("Please enter a valid Employee ID containing only numbers.");

            try
            {
                await employeeService.GetEmployeeById(parsedInt);
            }
            catch (HttpRequestException ex)
            {
                if ((int)ex.StatusCode! == 404)
                {
                    DisplayWarning("No employee exists with this ID.");
                    WaitForUser();
                    continue;
                }

                DisplayWarning(ex.Message);
                WaitForUser();
            }

            break;
        }

        if (AnsiConsole.Confirm("Are you sure you want to permanently delete this employee?"))
        {
            await employeeService.DeleteEmployee(parsedInt);
            DisplaySuccess("Successfully deleted employee.");
        }
        else
        {
            DisplaySuccess("Deletion was cancelled.");
        }

        WaitForUser();
    }

    //------- Helper Methods -------
    private List<IRenderable> BuildRenderableEmployees(List<EmployeeDto> employees)
    {
        List<IRenderable> iRenderable = [];

        foreach (var employee in employees)
        {
            var shiftsCompleted = employee.ShiftsCompleted is null ? "N/A" : employee.ShiftsCompleted.ToString()!;
            var loggedHours = employee.TotalLoggedHours is null ? "N/A" : employee.TotalLoggedHours.ToString()!;

            iRenderable.Add(new Markup($"[{White}]\nName: [/][{Green}]{employee.Name}[/]"));
            iRenderable.Add(new Markup($"[{White}]Employee ID: [/][{Grey}]{employee.EmployeeId}[/]"));
            iRenderable.Add(new Markup($"[{White}]Total Shift Count: [/][{Grey}]{shiftsCompleted}[/]"));
            iRenderable.Add(new Markup($"[{White}]Total Logged Hours: [/][{Grey}]{loggedHours}[/]"));
        }

        return iRenderable;
    }

    private void WaitForUser()
    {
        DisplayInfo("Press enter to continue.");
        Console.ReadLine();
    }
}