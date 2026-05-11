using Microsoft.EntityFrameworkCore;
using SolarTracker.Domain.Entities;
using SolarTracker.Infrastructure.Persistence.Entities;
using SolarTracker.Infrastructure.Repositories.Concrete;
using SolarTracker.Tests.Unit.TestInfrastructure;

namespace SolarTracker.Tests.Unit.Repositories;

public sealed class SolarTrackingConfigurationRepositoryTests
{
    [Fact]
    public async Task GetBySolarPanelIdAsync_returns_default_configuration_when_missing()
    {
        await using SqliteTestDatabase database = await SqliteTestDatabase.CreateAsync();
        SolarTrackingConfigurationRepository repository = new(database.DbContext);

        SolarTrackingConfiguration result = await repository.GetBySolarPanelIdAsync(42, CancellationToken.None);

        Assert.Equal(42, result.SolarPanelId);
        Assert.Equal(SolarTrackingConfiguration.DefaultPositionThresholdDegrees, result.PositionThresholdDegrees);
        Assert.Equal(SolarTrackingConfiguration.DefaultStepDurationMs, result.StepDurationMs);
        Assert.Equal(SolarTrackingConfiguration.DefaultMaxAdjustmentSteps, result.MaxAdjustmentSteps);
    }

    [Fact]
    public async Task UpsertAsync_inserts_then_updates_configuration_for_same_solar_panel()
    {
        await using SqliteTestDatabase database = await SqliteTestDatabase.CreateAsync();
        int solarPanelId = await SeedSolarPanelAsync(database);
        SolarTrackingConfigurationRepository repository = new(database.DbContext);

        SolarTrackingConfiguration inserted = await repository.UpsertAsync(
            new SolarTrackingConfiguration
            {
                SolarPanelId = solarPanelId,
                PositionThresholdDegrees = 2.5d,
                StepDurationMs = 750,
                MaxAdjustmentSteps = 30,
            },
            CancellationToken.None);

        SolarTrackingConfiguration updated = await repository.UpsertAsync(
            new SolarTrackingConfiguration
            {
                SolarPanelId = solarPanelId,
                PositionThresholdDegrees = 3.5d,
                StepDurationMs = 900,
                MaxAdjustmentSteps = 45,
            },
            CancellationToken.None);

        SolarTrackingConfigurationDb row = await database.DbContext.SolarTrackingConfigurations.SingleAsync();
        Assert.True(inserted.Id > 0);
        Assert.Equal(inserted.Id, updated.Id);
        Assert.Equal(3.5d, row.PositionThresholdDegrees);
        Assert.Equal(900, row.StepDurationMs);
        Assert.Equal(45, row.MaxAdjustmentSteps);
    }

    private static async Task<int> SeedSolarPanelAsync(SqliteTestDatabase database)
    {
        InstallationSiteDb site = new() { Name = "Site A", Latitude = 50.1m, Longitude = 8.6m };
        await database.DbContext.InstallationSites.AddAsync(site);
        await database.DbContext.SaveChangesAsync();

        SolarPanelDb panel = new() { InstallationSiteId = site.Id, SerialNumber = "panel-42" };
        await database.DbContext.SolarPanels.AddAsync(panel);
        await database.DbContext.SaveChangesAsync();
        return panel.Id;
    }
}
