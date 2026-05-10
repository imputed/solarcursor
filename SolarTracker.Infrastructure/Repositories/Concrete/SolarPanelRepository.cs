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

public sealed class SolarPanelRepository(SolarTrackerDbContext dbContext) : ISolarPanelRepository
{
    private readonly EfEntityCrudStore<SolarPanelDb> _store = new(dbContext);

    public async ValueTask<SolarPanel?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var row = await _store.GetByIdAsync(id, cancellationToken);
        return row is null ? null : PersistenceMapper.ToDomainSolarPanel(row);
    }

    public async ValueTask<IReadOnlyList<SolarPanel>> AnalyzeAsync(
        SolarPanelAnalyzeRequest request,
        CancellationToken cancellationToken = default)
    {
        var queryable = dbContext.Set<SolarPanelDb>().AsNoTracking().AsQueryable();
        queryable = queryable.ApplyAnalyze(request);
        List<SolarPanelDb> rows = await queryable.ToListAsync(cancellationToken);
        return rows.Select(PersistenceMapper.ToDomainSolarPanel).ToList();
    }

    public async ValueTask<IReadOnlyList<SolarPanel>> ListAsync(CancellationToken cancellationToken = default)
    {
        var rows = await _store.ListAsync(cancellationToken);
        return rows.Select(PersistenceMapper.ToDomainSolarPanel).ToList();
    }

    public async ValueTask AddAsync(SolarPanel entity, CancellationToken cancellationToken = default)
    {
        var row = PersistenceMapper.ToDbSolarPanel(entity);
        await _store.AddAsync(row, cancellationToken);
        entity.Id = row.Id;
    }

    public async ValueTask UpdateAsync(SolarPanel entity, CancellationToken cancellationToken = default)
    {
        var row = await dbContext.Set<SolarPanelDb>().FindAsync([entity.Id], cancellationToken);
        if (row is null)
        {
            return;
        }

        PersistenceMapper.CopySolarPanelScalars(row, entity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async ValueTask DeleteAsync(SolarPanel entity, CancellationToken cancellationToken = default)
    {
        var row = await _store.GetByIdAsync(entity.Id, cancellationToken);
        if (row is not null)
        {
            await _store.DeleteAsync(row, cancellationToken);
        }
    }
}
