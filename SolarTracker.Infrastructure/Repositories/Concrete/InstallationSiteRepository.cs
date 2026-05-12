using Microsoft.EntityFrameworkCore;
using SolarTracker.Application.Interfaces.Repositories;
using SolarTracker.Domain.Entities;
using SolarTracker.Infrastructure.Persistence;
using SolarTracker.Infrastructure.Persistence.Entities;
using SolarTracker.Infrastructure.Persistence.Mapping;

namespace SolarTracker.Infrastructure.Repositories.Concrete;

public sealed class InstallationSiteRepository(SolarTrackerDbContext dbContext) : IInstallationSiteRepository
{
    public async ValueTask AddAsync(InstallationSite entity, CancellationToken cancellationToken)
    {
        var row = InstallationSitePersistenceMapping.ToDb(entity);
        await dbContext.InstallationSites.AddAsync(row, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        entity.Id = row.Id;
    }

    public async ValueTask UpdateAsync(InstallationSite entity, CancellationToken cancellationToken)
    {
        var row = await dbContext.InstallationSites.FindAsync([entity.Id], cancellationToken);
        if (row is null)
            return;

        InstallationSitePersistenceMapping.CopyScalars(row, entity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async ValueTask DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var row = await dbContext.InstallationSites.FindAsync([id], cancellationToken);
        if (row is not null)
        {
            dbContext.InstallationSites.Remove(row);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
