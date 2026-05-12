using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SolarTracker.Infrastructure.Persistence;
using SolarTracker.Infrastructure.Services;

namespace SolarTracker.Tests.IntegrationTests.Common;

public sealed class SolarTrackerWebApplicationFactory(string connectionString) : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        builder.ConfigureAppConfiguration((_, configurationBuilder) =>
        {
            configurationBuilder.AddInMemoryCollection(
            [
                new KeyValuePair<string, string?>("ConnectionStrings:SolarTracker", connectionString),
                new KeyValuePair<string, string?>("RaspberryPi:UseFakeGpio", bool.TrueString),
            ]);
        });

        builder.ConfigureServices(services =>
        {
            ServiceDescriptor? backgroundService = services.SingleOrDefault(descriptor =>
                descriptor.ServiceType == typeof(IHostedService) &&
                descriptor.ImplementationType == typeof(SolarPanelOptimizationBackgroundService));

            if (backgroundService is not null)
                services.Remove(backgroundService);
        });
    }

    public async ValueTask InitializeDatabaseAsync()
    {
        using IServiceScope scope = Services.CreateScope();
        SolarTrackerDbContext dbContext = scope.ServiceProvider.GetRequiredService<SolarTrackerDbContext>();
        await dbContext.Database.MigrateAsync();
    }
}
