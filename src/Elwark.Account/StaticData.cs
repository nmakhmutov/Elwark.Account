namespace Elwark.Account;

internal static class StaticData
{
    internal static readonly string[] DateFormats =
    {
        "MM/dd/yyyy",
        "MM.dd.yyyy",
        "MM-dd-yyyy",
        "dd/MM/yyyy",
        "dd.MM.yyyy",
        "dd-MM-yyyy",
        "yyyy-MM-dd"
    };

    internal static readonly string[] TimeFormats =
    {
        "H:mm",
        "HH:mm",
        "HH:mm:ss",
        "h:mm tt",
        "hh:mm tt"
    };

    internal static readonly DayOfWeek[] DayOfWeeks =
    {
        DayOfWeek.Monday,
        DayOfWeek.Tuesday,
        DayOfWeek.Wednesday,
        DayOfWeek.Thursday,
        DayOfWeek.Friday,
        DayOfWeek.Saturday,
        DayOfWeek.Sunday
    };

    internal static readonly Dictionary<string, string> Languages = new()
    {
        ["en"] = "English",
        ["ru"] = "Русский"
    };
}
