using Microsoft.EntityFrameworkCore;
using SolarTracker.Application.Interfaces.Repositories;
using SolarTracker.Domain.Entities;
using SolarTracker.Infrastructure.Persistence;
using SolarTracker.Infrastructure.Persistence.Mapping;

namespace SolarTracker.Infrastructure.Repositories.Concrete;

public sealed class SolarTrackingConfigurationRepository(
    SolarTrackerDbContext dbContext) : ISolarTrackingConfigurationRepository
{
    public async ValueTask<SolarTrackingConfiguration> GetBySolarPanelIdAsync(
        int solarPanelId,
        CancellationToken cancellationToken)
    {
        var row = await dbContext.SolarTrackingConfigurations
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.SolarPanelId == solarPanelId, cancellationToken);

        return row is null
            ? SolarTrackingConfiguration.CreateDefault(solarPanelId)
            : SolarTrackingConfigurationPersistenceMapping.ToDomain(row);
    }

    public async ValueTask<SolarTrackingConfiguration> UpsertAsync(
        SolarTrackingConfiguration entity,
        CancellationToken cancellationToken)
    {
        var row = await dbContext.SolarTrackingConfigurations
            .FirstOrDefaultAsync(x => x.SolarPanelId == entity.SolarPanelId, cancellationToken);
        if (row is null)
        {
            row = SolarTrackingConfigurationPersistenceMapping.ToDb(entity);
            await dbContext.SolarTrackingConfigurations.AddAsync(row, cancellationToken);
        }
        else
        {
            SolarTrackingConfigurationPersistenceMapping.CopyScalars(row, entity);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return SolarTrackingConfigurationPersistenceMapping.ToDomain(row);
    }
}
