using Microsoft.EntityFrameworkCore;
using SolarTracker.Application.Interfaces.Repositories;
using SolarTracker.Domain.Entities;
using SolarTracker.Infrastructure.Persistence;
using SolarTracker.Infrastructure.Persistence.Mapping;

namespace SolarTracker.Infrastructure.Repositories.Concrete;

public sealed class SolarOptimizationScheduleConfigurationRepository(
    SolarTrackerDbContext dbContext) : ISolarOptimizationScheduleConfigurationRepository
{
    public async ValueTask<SolarOptimizationScheduleConfiguration> GetAsync(CancellationToken cancellationToken)
    {
        var row = await dbContext.SolarOptimizationScheduleConfigurations
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == SolarOptimizationScheduleConfiguration.SingletonId,
                cancellationToken);

        return row is null
            ? SolarOptimizationScheduleConfiguration.CreateDefault()
            : SolarOptimizationScheduleConfigurationPersistenceMapping.ToDomain(row);
    }

    public async ValueTask<SolarOptimizationScheduleConfiguration> UpsertAsync(
        SolarOptimizationScheduleConfiguration entity,
        CancellationToken cancellationToken)
    {
        var row = await dbContext.SolarOptimizationScheduleConfigurations
            .FirstOrDefaultAsync(
                x => x.Id == SolarOptimizationScheduleConfiguration.SingletonId,
                cancellationToken);

        if (row is null)
        {
            row = SolarOptimizationScheduleConfigurationPersistenceMapping.ToDb(entity);
            await dbContext.SolarOptimizationScheduleConfigurations.AddAsync(row, cancellationToken);
        }
        else
        {
            SolarOptimizationScheduleConfigurationPersistenceMapping.CopyScalars(row, entity);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return SolarOptimizationScheduleConfigurationPersistenceMapping.ToDomain(row);
    }
}
