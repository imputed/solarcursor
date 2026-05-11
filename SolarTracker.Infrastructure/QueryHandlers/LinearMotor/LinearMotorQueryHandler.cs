using Microsoft.EntityFrameworkCore;
using SolarTracker.Application.Analysis;
using SolarTracker.Application.Interfaces.QueryHandlers;
using SolarTracker.Infrastructure.Analysis.LinearMotor;
using SolarTracker.Infrastructure.Persistence;
using SolarTracker.Infrastructure.Persistence.Entities;
using SolarTracker.Infrastructure.Persistence.Mapping;
using LinearMotorEntity = SolarTracker.Domain.Entities.LinearMotor;

namespace SolarTracker.Infrastructure.QueryHandlers.LinearMotor;

public sealed class LinearMotorQueryHandler(SolarTrackerDbContext dbContext) : ILinearMotorQueryHandler
{
    public async ValueTask<LinearMotorEntity?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var row = await dbContext.LinearMotors
            .AsNoTracking()
            .FirstOrDefaultAsync(motor => motor.Id == id, cancellationToken);

        return row is null ? null : LinearMotorPersistenceMapping.ToDomain(row);
    }

    public async ValueTask<IReadOnlyList<LinearMotorEntity>> AnalyzeAsync(
        LinearMotorAnalyzeRequest request,
        CancellationToken cancellationToken)
    {
        var queryable = dbContext.LinearMotors.AsNoTracking();
        queryable = queryable.ApplyAnalyze(request);

        List<LinearMotorDb> rows = await queryable.ToListAsync(cancellationToken);
        if (rows.Count == 0)
        {
            return [];
        }

        List<LinearMotorEntity> entities = new(rows.Count);
        foreach (var row in rows)
        {
            entities.Add(LinearMotorPersistenceMapping.ToDomain(row));
        }

        return entities;
    }
}
