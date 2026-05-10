using Microsoft.EntityFrameworkCore;
using SolarTracker.Infrastructure.Persistence;

namespace SolarTracker.Infrastructure.Repositories.Helper;

internal sealed class EfEntityCrudStore<TEntity>(SolarTrackerDbContext dbContext)
    where TEntity : class
{
    public async ValueTask<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<TEntity>().FindAsync([id], cancellationToken);
    }

    public async ValueTask<IReadOnlyList<TEntity>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<TEntity>().AsNoTracking().ToListAsync(cancellationToken);
    }

    public async ValueTask AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await dbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async ValueTask DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        dbContext.Set<TEntity>().Remove(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
