using Microsoft.EntityFrameworkCore;
using SolarTracker.Application.Interfaces.Repositories;
using SolarTracker.Domain.Entities;
using SolarTracker.Infrastructure.Persistence;
using SolarTracker.Infrastructure.Persistence.Entities;
using SolarTracker.Infrastructure.Persistence.Mapping;

namespace SolarTracker.Infrastructure.Repositories.Concrete;

public sealed class TiltMeasuringUnitRepository(SolarTrackerDbContext dbContext) : ITiltMeasuringUnitRepository
{
    public async ValueTask AddAsync(TiltMeasuringUnit entity, CancellationToken cancellationToken)
    {
        var row = TiltMeasuringUnitPersistenceMapping.ToDb(entity);
        await dbContext.TiltMeasuringUnits.AddAsync(row, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        entity.Id = row.Id;
    }

    public async ValueTask UpdateAsync(TiltMeasuringUnit entity, CancellationToken cancellationToken)
    {
        var row = await dbContext.TiltMeasuringUnits.FindAsync([entity.Id], cancellationToken);
        if (row is null)
            return;

        TiltMeasuringUnitPersistenceMapping.CopyScalars(row, entity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async ValueTask DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var row = await dbContext.TiltMeasuringUnits.FindAsync([id], cancellationToken);
        if (row is not null)
        {
            dbContext.TiltMeasuringUnits.Remove(row);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
