using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SolarTracker.Infrastructure.Persistence;

namespace SolarTracker.Tests.IntegrationTests.Common;

public sealed class SqliteApiIntegrationTestFixture : IAsyncLifetime
{
    private readonly TemporarySqliteDatabase database = new();

    public SolarTrackerWebApplicationFactory Factory { get; private set; } = null!;

    public HttpClient Client { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        Factory = new SolarTrackerWebApplicationFactory(database.ConnectionString);
        await Factory.InitializeDatabaseAsync();
        Client = Factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost"),
        });
    }

    public async Task DisposeAsync()
    {
        Client.Dispose();
        await Factory.DisposeAsync();
        await database.DisposeAsync();
    }

    public async Task ResetDatabaseAsync()
    {
        using IServiceScope scope = Factory.Services.CreateScope();
        SolarTrackerDbContext dbContext = scope.ServiceProvider.GetRequiredService<SolarTrackerDbContext>();
        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.MigrateAsync();
    }

    public async Task ExecuteDbContextAsync(Func<SolarTrackerDbContext, Task> action)
    {
        using IServiceScope scope = Factory.Services.CreateScope();
        SolarTrackerDbContext dbContext = scope.ServiceProvider.GetRequiredService<SolarTrackerDbContext>();
        await action(dbContext);
    }

    public async Task<T> ExecuteDbContextAsync<T>(Func<SolarTrackerDbContext, Task<T>> action)
    {
        using IServiceScope scope = Factory.Services.CreateScope();
        SolarTrackerDbContext dbContext = scope.ServiceProvider.GetRequiredService<SolarTrackerDbContext>();
        return await action(dbContext);
    }
}
