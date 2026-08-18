namespace CandleCore.Api.Constants;

public static class SupportedIntervals
{
    public const string OneHour = "1h";
    public const string FourHour = "4h";
    public const string OneDay = "1d";
    public const string OneWeek = "1w";
    public const string OneMonth = "1M";

    public const string BaseInterval = OneHour;

    public static readonly IReadOnlyList<string> QueryTimeframes =
    [
        OneHour,
        FourHour,
        OneDay,
        OneWeek,
        OneMonth,
    ];

    public static bool IsSupportedQueryTimeframe(string timeframe) =>
        QueryTimeframes.Contains(timeframe);
}
