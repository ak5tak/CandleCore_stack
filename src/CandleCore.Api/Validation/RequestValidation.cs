using CandleCore.Api.Constants;
using CandleCore.Api.Extensions;

namespace CandleCore.Api.Validation;

public static class RequestValidation
{
    public static string? ValidateSymbol(string? symbol)
    {
        if (string.IsNullOrWhiteSpace(symbol))
        {
            return "symbol is required.";
        }

        return null;
    }

    public static string? ValidateQueryTimeframe(string? timeframe)
    {
        if (!SupportedIntervals.IsSupportedQueryTimeframe(timeframe ?? string.Empty))
        {
            return "timeframe is not supported.";
        }

        return null;
    }

    public static string? ValidateDateRange(DateTime startDate, DateTime endDate)
    {
        if (startDate >= endDate)
        {
            return "startDate must be before endDate.";
        }

        return null;
    }

    public static string? ValidateLookback(int lookback)
    {
        if (lookback < 1)
        {
            return "lookback must be at least 1.";
        }

        return null;
    }

    public static (DateTime From, DateTime To) ResolveDateRange(
        DateTime? startDate,
        DateTime? endDate
    )
    {
        var from = (startDate ?? DateTime.UtcNow.AddYears(-1)).AsUtc();
        var to = (endDate ?? DateTime.UtcNow).AsUtc();
        return (from, to);
    }

    public static string? ValidateSymbolAndTimeframe(string? symbol, string? timeframe)
    {
        if (ValidateSymbol(symbol) is { } symbolError)
        {
            return symbolError;
        }

        if (ValidateQueryTimeframe(timeframe) is { } timeframeError)
        {
            return timeframeError;
        }

        return null;
    }

    public static string? ValidateSymbolTimeframeAndDateRange(
        string? symbol,
        string? timeframe,
        DateTime? startDate,
        DateTime? endDate,
        out DateTime from,
        out DateTime to
    )
    {
        from = default;
        to = default;

        if (ValidateSymbolAndTimeframe(symbol, timeframe) is { } error)
        {
            return error;
        }

        (from, to) = ResolveDateRange(startDate, endDate);

        if (ValidateDateRange(from, to) is { } dateError)
        {
            return dateError;
        }

        return null;
    }
}
