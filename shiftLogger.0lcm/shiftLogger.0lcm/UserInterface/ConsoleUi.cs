using shiftLogger._0lcm.Enums;
using static shiftLogger._0lcm.UserInterface.DisplayHelper;

namespace shiftLogger._0lcm.UserInterface;

internal class ConsoleUi(EmployeeUi employeeUi, ShiftUi shiftUi)
{
    internal async Task MainMenu()
    {
        while (true)
        {
            Console.Clear();

            var option = DisplayMenu<MainMenuOption>();
            var shouldExit = await HandleMainMenu(option);

            if (!shouldExit)
                continue;

            await ExitApplication();

            return;
        }
    }

    /// <summary>
    ///     Handles the main menu option.
    /// </summary>
    /// <param name="option">Main menu option.</param>
    /// <returns>True if the menu should exit, else false.</returns>
    private async Task<bool> HandleMainMenu(MainMenuOption option)
    {
        switch (option)
        {
            case MainMenuOption.EmployeeMenu:
                await employeeUi.EmployeeMenu();
                break;
            case MainMenuOption.ShiftsMenu:
                await shiftUi.Menu();
                break;
            case MainMenuOption.Exit:
                return true;
        }

        return false;
    }

    private async Task ExitApplication()
    {
        await DisplaySpinner("Exiting application...", 2250);
        Environment.Exit(0);
    }
}