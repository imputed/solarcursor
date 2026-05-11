using Microsoft.EntityFrameworkCore;
using SolarTracker.Domain.Entities;
using SolarTracker.Infrastructure.Persistence.Entities;
using SolarTracker.Infrastructure.Repositories.Concrete;
using SolarTracker.Tests.Unit.TestInfrastructure;

namespace SolarTracker.Tests.Unit.Repositories;

public sealed class SolarPanelOptimizationStateRepositoryTests
{
    [Fact]
    public async Task GetBySolarPanelIdAsync_returns_disabled_default_state_when_missing()
    {
        await using SqliteTestDatabase database = await SqliteTestDatabase.CreateAsync();
        SolarPanelOptimizationStateRepository repository = new(database.DbContext);

        SolarPanelOptimizationState result = await repository.GetBySolarPanelIdAsync(15, CancellationToken.None);

        Assert.Equal(15, result.SolarPanelId);
        Assert.False(result.IsEnabled);
    }

    [Fact]
    public async Task UpsertAsync_and_ListEnabledSolarPanelIdsAsync_persist_and_filter_states()
    {
        await using SqliteTestDatabase database = await SqliteTestDatabase.CreateAsync();
        int firstSolarPanelId = await SeedSolarPanelAsync(database, "panel-1");
        int secondSolarPanelId = await SeedSolarPanelAsync(database, "panel-2");
        SolarPanelOptimizationStateRepository repository = new(database.DbContext);

        SolarPanelOptimizationState saved = await repository.UpsertAsync(
            new SolarPanelOptimizationState { SolarPanelId = secondSolarPanelId, IsEnabled = true },
            CancellationToken.None);
        await repository.UpsertAsync(
            new SolarPanelOptimizationState { SolarPanelId = firstSolarPanelId, IsEnabled = false },
            CancellationToken.None);

        IReadOnlyList<int> enabledIds = await repository.ListEnabledSolarPanelIdsAsync(CancellationToken.None);
        SolarPanelOptimizationStateDb row = await database.DbContext.SolarPanelOptimizationStates
            .SingleAsync(x => x.SolarPanelId == secondSolarPanelId);

        Assert.True(saved.Id > 0);
        Assert.Equal(secondSolarPanelId, row.SolarPanelId);
        Assert.Equal([secondSolarPanelId], enabledIds);
    }

    private static async Task<int> SeedSolarPanelAsync(SqliteTestDatabase database, string serialNumber)
    {
        InstallationSiteDb site = new() { Name = $"{serialNumber}-site", Latitude = 50.1m, Longitude = 8.6m };
        await database.DbContext.InstallationSites.AddAsync(site);
        await database.DbContext.SaveChangesAsync();

        SolarPanelDb panel = new() { InstallationSiteId = site.Id, SerialNumber = serialNumber };
        await database.DbContext.SolarPanels.AddAsync(panel);
        await database.DbContext.SaveChangesAsync();
        return panel.Id;
    }
}
