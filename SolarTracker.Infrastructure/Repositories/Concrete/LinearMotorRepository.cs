using Microsoft.EntityFrameworkCore;
using SolarTracker.Application.Interfaces.Repositories;
using SolarTracker.Domain.Entities;
using SolarTracker.Infrastructure.Persistence;
using SolarTracker.Infrastructure.Persistence.Entities;
using SolarTracker.Infrastructure.Persistence.Mapping;

namespace SolarTracker.Infrastructure.Repositories.Concrete;

public sealed class LinearMotorRepository(SolarTrackerDbContext dbContext) : ILinearMotorRepository
{
    public async ValueTask AddAsync(LinearMotor entity, CancellationToken cancellationToken)
    {
        var row = LinearMotorPersistenceMapping.ToDb(entity);
        await dbContext.LinearMotors.AddAsync(row, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        entity.Id = row.Id;
    }

    public async ValueTask UpdateAsync(LinearMotor entity, CancellationToken cancellationToken)
    {
        var row = await dbContext.LinearMotors.FindAsync([entity.Id], cancellationToken);
        if (row is null)
        {
            return;
        }

        LinearMotorPersistenceMapping.CopyScalars(row, entity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async ValueTask DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var row = await dbContext.LinearMotors.FindAsync([id], cancellationToken);
        if (row is not null)
        {
            dbContext.LinearMotors.Remove(row);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
