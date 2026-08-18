using CandleCore.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace CandleCore.Api.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Candle> Candles => Set<Candle>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Candle>()
            .HasIndex(x => new
            {
                x.Symbol,
                x.Interval,
                x.OpenTime,
            })
            .IsUnique();
    }
}
