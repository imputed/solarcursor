using Microsoft.EntityFrameworkCore;
using SolarTracker.Application.Interfaces.Repositories;
using SolarTracker.Domain.Entities;
using SolarTracker.Infrastructure.Persistence;
using SolarTracker.Infrastructure.Persistence.Entities;
using SolarTracker.Infrastructure.Persistence.Mapping;

namespace SolarTracker.Infrastructure.Repositories.Concrete;

public sealed class CurrentMeasuringUnitRepository(SolarTrackerDbContext dbContext) : ICurrentMeasuringUnitRepository
{
    public async ValueTask AddAsync(CurrentMeasuringUnit entity, CancellationToken cancellationToken)
    {
        var row = CurrentMeasuringUnitPersistenceMapping.ToDb(entity);
        await dbContext.CurrentMeasuringUnits.AddAsync(row, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        entity.Id = row.Id;
    }

    public async ValueTask UpdateAsync(CurrentMeasuringUnit entity, CancellationToken cancellationToken)
    {
        var row = await dbContext.CurrentMeasuringUnits.FindAsync([entity.Id], cancellationToken);
        if (row is null)
            return;

        CurrentMeasuringUnitPersistenceMapping.CopyScalars(row, entity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async ValueTask DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var row = await dbContext.CurrentMeasuringUnits.FindAsync([id], cancellationToken);
        if (row is not null)
        {
            dbContext.CurrentMeasuringUnits.Remove(row);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
