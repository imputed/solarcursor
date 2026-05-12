using Microsoft.EntityFrameworkCore;
using SolarTracker.Application.Interfaces.Repositories;
using SolarTracker.Domain.Entities;
using SolarTracker.Infrastructure.Persistence;
using SolarTracker.Infrastructure.Persistence.Entities;
using SolarTracker.Infrastructure.Persistence.Mapping;

namespace SolarTracker.Infrastructure.Repositories.Concrete;

public sealed class SolarPanelRepository(SolarTrackerDbContext dbContext) : ISolarPanelRepository
{
    public async ValueTask AddAsync(SolarPanel entity, CancellationToken cancellationToken)
    {
        var row = SolarPanelPersistenceMapping.ToDb(entity);
        await dbContext.SolarPanels.AddAsync(row, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        entity.Id = row.Id;
    }

    public async ValueTask UpdateAsync(SolarPanel entity, CancellationToken cancellationToken)
    {
        var row = await dbContext.SolarPanels.FindAsync([entity.Id], cancellationToken);
        if (row is null)
            return;

        SolarPanelPersistenceMapping.CopyScalars(row, entity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async ValueTask DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var row = await dbContext.SolarPanels.FindAsync([id], cancellationToken);
        if (row is not null)
        {
            dbContext.SolarPanels.Remove(row);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
