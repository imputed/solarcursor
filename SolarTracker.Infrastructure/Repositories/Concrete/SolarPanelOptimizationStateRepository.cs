using Microsoft.EntityFrameworkCore;
using SolarTracker.Application.Interfaces.Repositories;
using SolarTracker.Domain.Entities;
using SolarTracker.Infrastructure.Persistence;
using SolarTracker.Infrastructure.Persistence.Mapping;

namespace SolarTracker.Infrastructure.Repositories.Concrete;

public sealed class SolarPanelOptimizationStateRepository(
    SolarTrackerDbContext dbContext) : ISolarPanelOptimizationStateRepository
{
    public async ValueTask<SolarPanelOptimizationState> GetBySolarPanelIdAsync(
        int solarPanelId,
        CancellationToken cancellationToken)
    {
        var row = await dbContext.SolarPanelOptimizationStates
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.SolarPanelId == solarPanelId, cancellationToken);

        return row is null
            ? SolarPanelOptimizationState.CreateDefault(solarPanelId)
            : SolarPanelOptimizationStatePersistenceMapping.ToDomain(row);
    }

    public async ValueTask<SolarPanelOptimizationState> UpsertAsync(
        SolarPanelOptimizationState entity,
        CancellationToken cancellationToken)
    {
        var row = await dbContext.SolarPanelOptimizationStates
            .FirstOrDefaultAsync(x => x.SolarPanelId == entity.SolarPanelId, cancellationToken);
        if (row is null)
        {
            row = SolarPanelOptimizationStatePersistenceMapping.ToDb(entity);
            await dbContext.SolarPanelOptimizationStates.AddAsync(row, cancellationToken);
        }
        else
        {
            SolarPanelOptimizationStatePersistenceMapping.CopyScalars(row, entity);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return SolarPanelOptimizationStatePersistenceMapping.ToDomain(row);
    }

    public async ValueTask<IReadOnlyList<int>> ListEnabledSolarPanelIdsAsync(CancellationToken cancellationToken) =>
        await dbContext.SolarPanelOptimizationStates
            .AsNoTracking()
            .Where(x => x.IsEnabled)
            .OrderBy(x => x.SolarPanelId)
            .Select(x => x.SolarPanelId)
            .ToListAsync(cancellationToken);
}
