using CandleCore.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CandleCore.Api.Tests.Helpers;

public static class TestDb
{
    public static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }
}
