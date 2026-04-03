namespace shiftLogger._0lcm.Enums;

internal enum MainMenuOption
{
    EmployeeMenu,
    ShiftsMenu,
    Exit
}

internal enum EmployeeMenuOption
{
    ShowAllEmployees,
    SearchEmployeeById,
    SearchEmployeeByName,
    AddNewEmployee,
    EditExistingEmployee,
    DeleteEmployee,
    Back
}

internal enum ShiftMenuOption
{
    ViewShifts,
    AddNewShift,
    AssignToOrRemoveShiftsFromEmployee,
    MarkShiftAsCompleted,
    EditShift,
    DeleteShift,
    Back
}

internal enum ShiftViewShiftsSubMenuOption
{
    SeeAllUpcomingShifts,
    SeeAvailableShifts,
    SearchShiftsByDate,
    SearchShiftById,
    SeeShiftsForEmployee,
    Back
}

internal enum ShiftAssignAndRemoveShiftsSubMenuOption
{
    AssignShiftToAnEmployee,
    RemoveShiftFromAnEmployee,
    RemoveAllShiftsFromAnEmployee,
    Back
}