using CandleCore.Api.Infrastructure.Binance;
using CandleCore.Api.Infrastructure.Persistence;
using CandleCore.Api.Services.Analysis;
using CandleCore.Api.Services.Dashboard;
using CandleCore.Api.Services.MarketData;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Analysis
builder.Services.AddScoped<AnalysisCalculator>();
builder.Services.AddScoped<IAnalysisQueryService, AnalysisQueryService>();

// Dashboard
builder.Services.AddScoped<IDashboardQueryService, DashboardQueryService>();

// MarketData (reads)
builder.Services.AddScoped<CandleQueryService>();
builder.Services.AddScoped<CandleAggregationService>();
builder.Services.AddScoped<CandleProvider>();
builder.Services.AddScoped<IMarketDataQueryService, MarketDataQueryService>();

builder.Services.AddHttpClient<IBinanceClient, BinanceClient>(client =>
{
    client.BaseAddress = new Uri("https://api.binance.com");
});
builder.Services.AddScoped<CandleSeeder>();
builder.Services.AddHostedService<CandleSeedHostedService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var corsOrigins = builder.Configuration.GetSection("Cors:Origins").Get<string[]>() ?? [];

if (builder.Environment.IsDevelopment() && corsOrigins.Length > 0)
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("DevCors", policy =>
        {
            policy.WithOrigins(corsOrigins)
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
    });
}

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}

if (app.Environment.IsDevelopment())
{
    if (corsOrigins.Length > 0)
    {
        app.UseCors("DevCors");
    }

    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
