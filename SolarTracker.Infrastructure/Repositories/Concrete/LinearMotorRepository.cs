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

public sealed class LinearMotorRepository(SolarTrackerDbContext dbContext) : ILinearMotorRepository
{
    private readonly EfEntityCrudStore<LinearMotorDb> _store = new(dbContext);

    public async ValueTask<LinearMotor?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var row = await _store.GetByIdAsync(id, cancellationToken);
        return row is null ? null : PersistenceMapper.ToDomainLinearMotor(row);
    }

    public async ValueTask<IReadOnlyList<LinearMotor>> AnalyzeAsync(
        LinearMotorAnalyzeRequest request,
        CancellationToken cancellationToken = default)
    {
        var queryable = dbContext.Set<LinearMotorDb>().AsNoTracking().AsQueryable();
        queryable = queryable.ApplyAnalyze(request);
        List<LinearMotorDb> rows = await queryable.ToListAsync(cancellationToken);
        return rows.Select(PersistenceMapper.ToDomainLinearMotor).ToList();
    }

    public async ValueTask<IReadOnlyList<LinearMotor>> ListAsync(CancellationToken cancellationToken = default)
    {
        var rows = await _store.ListAsync(cancellationToken);
        return rows.Select(PersistenceMapper.ToDomainLinearMotor).ToList();
    }

    public async ValueTask AddAsync(LinearMotor entity, CancellationToken cancellationToken = default)
    {
        var row = PersistenceMapper.ToDbLinearMotor(entity);
        await _store.AddAsync(row, cancellationToken);
        entity.Id = row.Id;
    }

    public async ValueTask UpdateAsync(LinearMotor entity, CancellationToken cancellationToken = default)
    {
        var row = await dbContext.Set<LinearMotorDb>().FindAsync([entity.Id], cancellationToken);
        if (row is null)
        {
            return;
        }

        PersistenceMapper.CopyLinearMotorScalars(row, entity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async ValueTask DeleteAsync(LinearMotor entity, CancellationToken cancellationToken = default)
    {
        var row = await _store.GetByIdAsync(entity.Id, cancellationToken);
        if (row is not null)
        {
            await _store.DeleteAsync(row, cancellationToken);
        }
    }
}
