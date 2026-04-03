using System.Net;
using shiftLogger._0lcm.Enums;
using shiftLogger._0lcm.ServiceContracts;
using shiftLogger.Shared;
using Spectre.Console;
using Spectre.Console.Rendering;
using static shiftLogger._0lcm.UserInterface.DisplayHelper;

namespace shiftLogger._0lcm.UserInterface;

public class ShiftUi(IShiftService shiftService, IValidationService validationService)
{
    private const string BackOption = "back";

    //------- Menu Methods -------
    internal async Task Menu()
    {
        while (true)
        {
            var option = DisplayMenu<ShiftMenuOption>();
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
    private async Task<bool> HandleMenuOption(ShiftMenuOption option)
    {
        switch (option)
        {
            case ShiftMenuOption.ViewShifts:
                await ViewShifts();
                break;
            case ShiftMenuOption.AddNewShift:
                await AddNewShift();
                break;
            case ShiftMenuOption.AssignToOrRemoveShiftsFromEmployee:
                await AssignAndRemoveShifts();
                break;
            case ShiftMenuOption.MarkShiftAsCompleted:
                await CompleteShift();
                break;
            case ShiftMenuOption.EditShift:
                await EditShift();
                break;
            case ShiftMenuOption.DeleteShift:
                await DeleteShift();
                break;
            case ShiftMenuOption.Back:
                return true;
        }

        return false;
    }

    //------- Sub-Menus -------
    private async Task ViewShifts()
    {
        while (true)
        {
            Console.Clear();

            var option = DisplayMenu<ShiftViewShiftsSubMenuOption>();

            switch (option)
            {
                case ShiftViewShiftsSubMenuOption.SeeAllUpcomingShifts:
                    await SeeUpcomingShifts();
                    break;
                case ShiftViewShiftsSubMenuOption.SeeAvailableShifts:
                    await SeeAvailableShifts();
                    break;
                case ShiftViewShiftsSubMenuOption.SearchShiftsByDate:
                    await SeeShiftsForDate();
                    break;
                case ShiftViewShiftsSubMenuOption.SearchShiftById:
                    await SeeShiftForId();
                    break;
                case ShiftViewShiftsSubMenuOption.SeeShiftsForEmployee:
                    await SeeShiftsForEmployee();
                    break;
                case ShiftViewShiftsSubMenuOption.Back:
                    return;
            }
        }
    }

    private async Task AssignAndRemoveShifts()
    {
        while (true)
        {
            Console.Clear();
            var option = DisplayMenu<ShiftAssignAndRemoveShiftsSubMenuOption>();

            switch (option)
            {
                case ShiftAssignAndRemoveShiftsSubMenuOption.AssignShiftToAnEmployee:
                    await AssignShift();
                    break;
                case ShiftAssignAndRemoveShiftsSubMenuOption.RemoveShiftFromAnEmployee:
                    await RemoveShift();
                    break;
                case ShiftAssignAndRemoveShiftsSubMenuOption.RemoveAllShiftsFromAnEmployee:
                    await RemoveAllShifts();
                    break;
                case ShiftAssignAndRemoveShiftsSubMenuOption.Back:
                    return;
            }
        }
    }

    //------- CRUD Operations -------
    private async Task SeeUpcomingShifts()
    {
        try
        {
            var shifts = await shiftService.GetUpcomingShifts();
            DisplayShifts(shifts);
        }
        catch (HttpRequestException ex)
        {
            HandleException(ex);
        }
    }

    private async Task SeeAvailableShifts()
    {
        try
        {
            var shifts = await shiftService.GetAvailableShifts();
            DisplayShifts(shifts);
        }
        catch (HttpProtocolException ex)
        {
            HandleException(ex);
        }
    }

    private async Task SeeShiftsForDate()
    {
        DateOnly parsedDate;

        while (true)
        {
            var date = GetArgument("Please enter the search date:",
                "Please use yyyy-MM-dd formatting when entering dates.");
            if (date is null) return;

            if (validationService.TryValidateDateTime(date, out parsedDate, out var errorMessage))
                break;

            var message = errorMessage ?? "Invalid date, please make sure you follow a 'yyyy-MM-dd' format.";
            DisplayWarning(message);
            WaitForUser();
        }

        try
        {
            var shifts = await shiftService.GetShiftsByDate(parsedDate);
            DisplayShifts(shifts);
        }
        catch (HttpRequestException ex)
        {
            HandleException(ex);
        }
    }

    private async Task SeeShiftForId()
    {
        int parsedId;

        while (true)
        {
            var id = GetArgument("Please enter a shift ID:");
            if (id is null) return;

            if (!TryValidateId(id, out parsedId)) continue;

            break;
        }

        try
        {
            var shift = await shiftService.GetShiftById(parsedId);
            DisplayShifts(new List<ShiftDto> { shift });
        }
        catch (HttpRequestException ex)
        {
            HandleException(ex);
        }
    }

    private async Task SeeShiftsForEmployee()
    {
        int parsedId;

        while (true)
        {
            var id = GetArgument("Please enter an employee ID:");
            if (id is null) return;

            if (!TryValidateId(id, out parsedId)) continue;

            break;
        }

        try
        {
            var shifts = await shiftService.GetShiftsForEmployee(parsedId);
            DisplayShifts(shifts);
        }
        catch (HttpRequestException ex)
        {
            HandleException(ex);
        }
    }

    private async Task AddNewShift()
    {
        DateOnly parsedDate;
        TimeOnly parsedStart;
        TimeOnly parsedEnd;

        while (true)
        {
            var date = GetArgument("Please enter a date:", "Please use yyyy-MM-dd formatting when entering a date.");
            if (date is null) return;

            if (!validationService.TryValidateDateTime(date, out parsedDate, out var errorMessage))
            {
                var message = errorMessage ?? "The date you entered is not valid, please try again.";
                DisplayWarning(message);
                WaitForUser();
                continue;
            }

            break;
        }

        while (true)
        {
            var startTime = GetArgument("Please enter a start time:",
                "Please use HH:mm formatting when entering a time.");
            if (startTime is null) return;

            if (!validationService.TryValidateTimeOnly(startTime, out parsedStart, out var errorMessage))
            {
                var message = errorMessage ?? "The time you entered wasn't valid, please use HH:mm formatting.";
                DisplayWarning(message);
                WaitForUser();
                continue;
            }

            break;
        }

        while (true)
        {
            var endTime = GetArgument("Please enter an end time:", "Please use HH:mm formatting when entering a time.");
            if (endTime is null) return;

            if (!validationService.TryValidateTimeOnly(endTime, out parsedEnd, out var errorMessage))
            {
                var message = errorMessage ?? "The time you entered wasn't valid, please use HH:mm formatting.";
                DisplayWarning(message);
                WaitForUser();
                continue;
            }

            break;
        }

        try
        {
            await shiftService.PostShift(parsedDate, parsedStart, parsedEnd);
            DisplaySuccess("Successfully added new shift to the database.");
            WaitForUser();
        }
        catch (HttpRequestException ex)
        {
            HandleException(ex);
        }
    }

    private async Task AssignShift()
    {
        int parsedShiftId;
        int parsedEmployeeId;

        while (true)
        {
            var shiftId = GetArgument("Please enter the shift ID:");
            if (shiftId is null) return;

            if (!TryValidateId(shiftId, out parsedShiftId)) continue;

            break;
        }

        while (true)
        {
            var employeeId = GetArgument("Please enter the employee ID to assign to:");
            if (employeeId is null) return;

            if (!TryValidateId(employeeId, out parsedEmployeeId)) continue;

            break;
        }

        try
        {
            await shiftService.PutAssignShift(parsedShiftId, parsedEmployeeId);
            DisplaySuccess("Successfully assigned shift to employee.");
            WaitForUser();
        }
        catch (HttpRequestException ex)
        {
            HandleException(ex);
        }
    }

    private async Task RemoveShift()
    {
        int parsedShiftId;

        while (true)
        {
            var id = GetArgument("Please enter the shift ID:");
            if (id is null) return;

            if (!TryValidateId(id, out parsedShiftId)) continue;

            break;
        }

        try
        {
            await shiftService.PutRemoveShift(parsedShiftId);
            DisplaySuccess("Successfully removed shift from employee.");
            WaitForUser();
        }
        catch (HttpRequestException ex)
        {
            HandleException(ex);
        }
    }

    private async Task RemoveAllShifts()
    {
        int parsedEmployeeId;

        while (true)
        {
            var id = GetArgument("Please enter the employee ID:");
            if (id is null) return;

            if (!TryValidateId(id, out parsedEmployeeId)) continue;

            break;
        }

        try
        {
            await shiftService.PutRemoveAllShiftsFromEmployee(parsedEmployeeId);
            DisplaySuccess("Successfully removed all shifts from employee.");
            WaitForUser();
        }
        catch (HttpRequestException ex)
        {
            HandleException(ex);
        }
    }

    private async Task CompleteShift()
    {
        int parsedId;

        while (true)
        {
            var id = GetArgument("Please enter the shift ID:");
            if (id is null) return;

            if (!TryValidateId(id, out parsedId)) continue;

            break;
        }

        try
        {
            await shiftService.PutCompleteShift(parsedId);
            DisplaySuccess("Successfully completed shift to employee.");
            WaitForUser();
        }
        catch (HttpRequestException ex)
        {
            HandleException(ex);
        }
    }

    private async Task EditShift()
    {
        int parsedId;
        DateOnly parsedDate;
        TimeOnly parsedTime;
        TimeOnly parsedEnd;

        while (true)
        {
            var id = GetArgument("Please enter the shift ID:");
            if (id is null) return;

            if (!TryValidateId(id, out parsedId)) continue;

            break;
        }

        while (true)
        {
            var date = GetArgument("Please enter a new date:",
                "Please use yyyy-MM-dd formatting when entering a date.");
            if (date is null) return;

            if (!validationService.TryValidateDateTime(date, out parsedDate, out var errorMessage))
            {
                var message = errorMessage ?? "Please enter a valid date.";
                DisplayWarning(message);
                WaitForUser();
                continue;
            }

            break;
        }

        while (true)
        {
            var start = GetArgument("Please enter a new start time:",
                "Please use HH:mm formatting when entering a time.");
            if (start is null) return;

            if (!validationService.TryValidateTimeOnly(start, out parsedTime, out var errorMessage))
            {
                var message = errorMessage ?? "Please enter a valid time.";
                DisplayWarning(message);
                WaitForUser();
                continue;
            }

            break;
        }

        while (true)
        {
            var end = GetArgument("Please enter a new end time:", "Please use HH:mm formatting when entering a time.");
            if (end is null) return;

            if (!validationService.TryValidateTimeOnly(end, out parsedEnd, out var errorMessage))
            {
                var message = errorMessage ?? "Please enter a valid time.";
                DisplayWarning(message);
                WaitForUser();
                continue;
            }

            break;
        }

        try
        {
            await shiftService.PutEditShift(parsedId, parsedDate, parsedTime, parsedEnd);
            DisplaySuccess("Successfully edited the shift.");
            WaitForUser();
        }
        catch (HttpRequestException ex)
        {
            HandleException(ex);
        }
    }

    private async Task DeleteShift()
    {
        int parsedId;

        while (true)
        {
            var id = GetArgument("Please enter a shift ID to delete:");
            if (id is null) return;

            if (!TryValidateId(id, out parsedId)) continue;

            break;
        }

        if (AnsiConsole.Confirm("Are you sure you want to permanently delete this shift from the database?"))
        {
            await shiftService.DeleteShift(parsedId);
            DisplaySuccess("Successfully deleted the shift.");
        }
        else
        {
            DisplaySuccess("Deletion was cancelled.");
        }

        WaitForUser();
    }

    //------- Helper Methods -------
    private List<IRenderable> BuildRenderableShifts(List<ShiftDto> shifts)
    {
        List<IRenderable> iRenderable = [];

        foreach (var shift in shifts)
        {
            var employeeId = shift.EmployeeId is null ? "Available for pickup" : $"Employee ID: {shift.EmployeeId}";
            iRenderable.Add(new Markup($"[{White}]\nShift ID: [/][{Green}]{shift.ShiftId}[/]"));
            iRenderable.Add(new Markup($"[{White}]Date: [/][{Grey}]{shift.Date}[/]"));
            iRenderable.Add(new Markup($"[{White}]Starting at: [/][{Grey}]{shift.StartTime}[/]"));
            iRenderable.Add(new Markup($"[{White}]Ending at: [/][{Grey}]{shift.EndTime}[/]"));
            iRenderable.Add(new Markup($"[{White}]Lasting: [/][{Grey}]{shift.Duration}[/]"));
            iRenderable.Add(new Markup($"[{White}]Completed? [/][{Grey}]{shift.Completed}[/]"));
            iRenderable.Add(new Markup($"[{White}]Assigned to: [/][{Grey}]{employeeId}[/]"));
        }

        return iRenderable;
    }

    private void DisplayShifts(List<ShiftDto> shifts)
    {
        var iRenderable = BuildRenderableShifts(shifts);
        if (iRenderable.Count == 0)
            DisplayWarning("There are no current shifts to display.");
        else
            DisplayRows(iRenderable);

        DisplayPrompt(new List<string> { BackOption }, "\nPress back to return:");
    }

    private void WaitForUser()
    {
        DisplayMessage("Press enter to continue");
        Console.ReadLine();
    }

    private void HandleException(Exception ex)
    {
        Console.Clear();

        if (ex is HttpRequestException { StatusCode: not null } exception)
        {
            var statusCode = exception.StatusCode;

            if (statusCode == HttpStatusCode.NotFound)
                DisplayWarning(
                    "The content could not be found, please check that you entered proper details, or that the content exists.");
            else if ((int)statusCode! >= 400 && (int)statusCode <= 499)
                DisplayWarning(
                    "A client side error has occurred while processing your request, please check that you are sending a valid request to the API.");
            else if ((int)statusCode >= 500 && (int)statusCode <= 599)
                DisplayWarning("A server side error has occurred while processing your request.");
            else
                DisplayWarning(
                    "An unknown error has occurred either client side or server side while attempting to process a request to the API.");
        }
        else if (ex is ArgumentException argException)
        {
            if (argException is ArgumentNullException)
                DisplayWarning(
                    "One or more of the arguments you have entered was null, please try again valid and non null details.");
            else
                DisplayWarning(
                    "An error has occurred with one or more of the arguments you have entered, please check that all entered arguments are correct and valid.");
        }
        else
        {
            DisplayWarning(
                "An unexpected error has occurred during runtime, please retry later or report the problem if it persists.");
        }

        WaitForUser();
    }

    private bool TryValidateId(string id, out int parsedId)
    {
        if (!int.TryParse(id, out parsedId))
        {
            DisplayWarning("Please enter a valid ID containing only numbers.");
            WaitForUser();
            return false;
        }

        return true;
    }

    /// <summary>
    ///     Gets an argument from the user
    /// </summary>
    /// <param name="prompt"></param>
    /// <returns>the value entered or null if the method should return.</returns>
    private string? GetArgument(string prompt, string? extraInstructions = null)
    {
        Console.Clear();
        DisplayInfo("Enter 'Back' to return.");
        if (extraInstructions != null) DisplayInfo(extraInstructions);
        var value = DisplayQuestion(prompt);

        if (value.ToLower() == BackOption) return null;
        return value;
    }
}