using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SolarTracker.Infrastructure.Persistence;

namespace SolarTracker.Tests.Unit.TestInfrastructure;

internal sealed class SqliteTestDatabase : IAsyncDisposable
{
    private readonly SqliteConnection connection;

    internal SolarTrackerDbContext DbContext { get; }

    private SqliteTestDatabase(SqliteConnection connection, SolarTrackerDbContext dbContext)
    {
        this.connection = connection;
        DbContext = dbContext;
    }

    internal static async ValueTask<SqliteTestDatabase> CreateAsync()
    {
        SqliteConnection connection = new("Data Source=:memory:");
        await connection.OpenAsync();

        DbContextOptions<SolarTrackerDbContext> options = new DbContextOptionsBuilder<SolarTrackerDbContext>()
            .UseSqlite(connection)
            .Options;

        SolarTrackerDbContext dbContext = new(options);
        await dbContext.Database.EnsureCreatedAsync();
        return new SqliteTestDatabase(connection, dbContext);
    }

    public async ValueTask DisposeAsync()
    {
        await DbContext.DisposeAsync();
        await connection.DisposeAsync();
    }
}
