using System.Text.Json;

namespace CandleCore.Api.Infrastructure.Binance;

public class BinanceClient : IBinanceClient
{
    private readonly HttpClient _httpClient;

    public BinanceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<BinanceKlineDto>> GetKlinesAsync(
        string symbol,
        string interval,
        int limit = 1000,
        long? endTime = null,
        CancellationToken cancellationToken = default
    )
    {
        var url = $"/api/v3/klines?symbol={symbol}&interval={interval}&limit={limit}";
        if (endTime is long endTimeMs)
        {
            url += $"&endTime={endTimeMs}";
        }

        var response = await _httpClient.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        var rawKlines = JsonSerializer.Deserialize<List<List<JsonElement>>>(json);

        if (rawKlines is null)
        {
            return [];
        }

        return rawKlines
            .Select(k => new BinanceKlineDto
            {
                OpenTime = k[0].GetInt64(),
                Open = k[1].GetString()!,
                High = k[2].GetString()!,
                Low = k[3].GetString()!,
                Close = k[4].GetString()!,
                Volume = k[5].GetString()!,
                CloseTime = k[6].GetInt64(),
            })
            .ToList();
    }
}
