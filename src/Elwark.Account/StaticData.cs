namespace Elwark.Account;

internal static class StaticData
{
    internal static readonly string[] DateFormats =
    [
        "dd.MM.yyyy",
        "dd.MM.yy",
        "d.M.yyyy",
        "d.M.yy",
        "dd-MM-yyyy",
        "dd-MM-yy",
        "d-M-yyyy",
        "d-M-yy",
        "dd/MM/yyyy",
        "dd/MM/yy",
        "d/M/yyyy",
        "d/M/yy",
        "yyyy-MM-dd",
        "MM.dd.yyyy",
        "MM-dd-yyyy",
        "MM/dd/yyyy"
    ];

    internal static readonly string[] TimeFormats =
    [
        "H:mm",
        "HH:mm",
        "HH:mm:ss",
        "h:mm tt",
        "hh:mm tt"
    ];

    internal static readonly DayOfWeek[] DayOfWeeks =
    [
        DayOfWeek.Monday,
        DayOfWeek.Tuesday,
        DayOfWeek.Wednesday,
        DayOfWeek.Thursday,
        DayOfWeek.Friday,
        DayOfWeek.Saturday,
        DayOfWeek.Sunday
    ];

    internal static readonly Dictionary<string, string> Languages = new()
    {
        ["en"] = "English",
        ["ru"] = "Русский"
    };
}
