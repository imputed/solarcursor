using Microsoft.EntityFrameworkCore;
using SolarTracker.Application.Analysis;
using SolarTracker.Application.Interfaces.Repositories;
using SolarTracker.Domain.Entities;
using SolarTracker.Infrastructure.Analysis;
using SolarTracker.Infrastructure.Persistence;
using SolarTracker.Infrastructure.Persistence.Entities;
using SolarTracker.Infrastructure.Persistence.Mapping;
using SolarTracker.Infrastructure.Repositories.Helper;

namespace SolarTracker.Infrastructure.Repositories.Concrete;

public sealed class InstallationSiteRepository(SolarTrackerDbContext dbContext) : IInstallationSiteRepository
{
    private readonly EfEntityCrudStore<InstallationSiteDb> _store = new(dbContext);

    public async ValueTask<InstallationSite?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var row = await dbContext.Set<InstallationSiteDb>()
            .AsNoTracking()
            .Include(s => s.SolarPanels)
            .Include(s => s.LinearMotors)
            .AsSplitQuery()
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        return row is null ? null : PersistenceMapper.ToDomainInstallationSite(row, loadChildren: true);
    }

    public async ValueTask<IReadOnlyList<InstallationSite>> AnalyzeAsync(
        InstallationSiteAnalyzeRequest request,
        CancellationToken cancellationToken = default)
    {
        var queryable = dbContext.Set<InstallationSiteDb>().AsNoTracking().AsQueryable();
        queryable = queryable.ApplyAnalyze(request);
        List<InstallationSiteDb> rows = await queryable.ToListAsync(cancellationToken);
        return rows.Select(r => PersistenceMapper.ToDomainInstallationSite(r, loadChildren: false)).ToList();
    }

    public async ValueTask<IReadOnlyList<InstallationSite>> ListAsync(CancellationToken cancellationToken = default)
    {
        var rows = await _store.ListAsync(cancellationToken);
        return rows.Select(r => PersistenceMapper.ToDomainInstallationSite(r, loadChildren: false)).ToList();
    }

    public async ValueTask AddAsync(InstallationSite entity, CancellationToken cancellationToken = default)
    {
        var row = PersistenceMapper.ToDbInstallationSite(entity);
        await _store.AddAsync(row, cancellationToken);
        entity.Id = row.Id;
    }

    public async ValueTask UpdateAsync(InstallationSite entity, CancellationToken cancellationToken = default)
    {
        var row = await dbContext.Set<InstallationSiteDb>().FindAsync([entity.Id], cancellationToken);
        if (row is null)
        {
            return;
        }

        PersistenceMapper.CopyInstallationSiteScalars(row, entity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async ValueTask DeleteAsync(InstallationSite entity, CancellationToken cancellationToken = default)
    {
        var row = await _store.GetByIdAsync(entity.Id, cancellationToken);
        if (row is not null)
        {
            await _store.DeleteAsync(row, cancellationToken);
        }
    }
}
