using Microsoft.EntityFrameworkCore;
using SolarTracker.Application.Analysis;
using SolarTracker.Application.Interfaces.QueryHandlers;
using SolarTracker.Infrastructure.Analysis.CurrentMeasuringUnit;
using SolarTracker.Infrastructure.Persistence;
using SolarTracker.Infrastructure.Persistence.Entities;
using SolarTracker.Infrastructure.Persistence.Mapping;
using CurrentMeasuringUnitEntity = SolarTracker.Domain.Entities.CurrentMeasuringUnit;

namespace SolarTracker.Infrastructure.QueryHandlers.CurrentMeasuringUnit;

public sealed class CurrentMeasuringUnitQueryHandler(SolarTrackerDbContext dbContext) : ICurrentMeasuringUnitQueryHandler
{
    public async ValueTask<CurrentMeasuringUnitEntity?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var row = await dbContext.CurrentMeasuringUnits
            .AsNoTracking()
            .FirstOrDefaultAsync(unit => unit.Id == id, cancellationToken);

        return row is null ? null : CurrentMeasuringUnitPersistenceMapping.ToDomain(row);
    }

    public async ValueTask<IReadOnlyList<CurrentMeasuringUnitEntity>> AnalyzeAsync(
        CurrentMeasuringUnitAnalyzeRequest request,
        CancellationToken cancellationToken)
    {
        var queryable = dbContext.CurrentMeasuringUnits.AsNoTracking();
        queryable = queryable.ApplyAnalyze(request);

        List<CurrentMeasuringUnitDb> rows = await queryable.ToListAsync(cancellationToken);
        if (rows.Count == 0)
            return [];

        List<CurrentMeasuringUnitEntity> entities = new(rows.Count);
        foreach (var row in rows)
        {
            entities.Add(CurrentMeasuringUnitPersistenceMapping.ToDomain(row));
        }

        return entities;
    }
}
