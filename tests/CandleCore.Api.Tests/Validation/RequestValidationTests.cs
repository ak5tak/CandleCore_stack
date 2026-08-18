using CandleCore.Api.Tests.Helpers;
using CandleCore.Api.Validation;
using FluentAssertions;

namespace CandleCore.Api.Tests.Validation;

public class RequestValidationTests
{
    private static readonly DateTime Jan1 = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    private static readonly DateTime Jan2 = new(2026, 1, 2, 0, 0, 0, DateTimeKind.Utc);

    [Fact]
    public void ValidateSymbolAndTimeframe_Should_Return_Error_For_Empty_Symbol()
    {
        RequestValidation
            .ValidateSymbolAndTimeframe("  ", CandleFixtures.Interval)
            .Should()
            .Be("symbol is required.");
    }

    [Fact]
    public void ValidateSymbolAndTimeframe_Should_Return_Error_For_Unsupported_Timeframe()
    {
        RequestValidation
            .ValidateSymbolAndTimeframe(CandleFixtures.Symbol, "1m")
            .Should()
            .Be("timeframe is not supported.");
    }

    [Fact]
    public void ValidateLookback_Should_Return_Error_When_Less_Than_One()
    {
        RequestValidation.ValidateLookback(0).Should().Be("lookback must be at least 1.");
    }

    [Fact]
    public void ValidateSymbolTimeframeAndDateRange_Should_Return_Error_When_Start_Not_Before_End()
    {
        var error = RequestValidation.ValidateSymbolTimeframeAndDateRange(
            CandleFixtures.Symbol,
            CandleFixtures.Interval,
            Jan2,
            Jan1,
            out _,
            out _
        );

        error.Should().Be("startDate must be before endDate.");
    }

    [Fact]
    public void ValidateSymbolTimeframeAndDateRange_Should_Resolve_Defaults_As_Utc_Ordered_Range()
    {
        var error = RequestValidation.ValidateSymbolTimeframeAndDateRange(
            CandleFixtures.Symbol,
            CandleFixtures.Interval,
            null,
            null,
            out var from,
            out var to
        );

        error.Should().BeNull();
        from.Kind.Should().Be(DateTimeKind.Utc);
        to.Kind.Should().Be(DateTimeKind.Utc);
        from.Should().BeBefore(to);
    }
}
