using Microsoft.EntityFrameworkCore;
using SolarTracker.Application.Analysis;
using SolarTracker.Application.Interfaces.QueryHandlers;
using SolarTracker.Infrastructure.Analysis.TiltMeasuringUnit;
using SolarTracker.Infrastructure.Persistence;
using SolarTracker.Infrastructure.Persistence.Entities;
using SolarTracker.Infrastructure.Persistence.Mapping;
using TiltMeasuringUnitEntity = SolarTracker.Domain.Entities.TiltMeasuringUnit;

namespace SolarTracker.Infrastructure.QueryHandlers.TiltMeasuringUnit;

public sealed class TiltMeasuringUnitQueryHandler(SolarTrackerDbContext dbContext) : ITiltMeasuringUnitQueryHandler
{
    public async ValueTask<TiltMeasuringUnitEntity?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var row = await dbContext.TiltMeasuringUnits
            .AsNoTracking()
            .FirstOrDefaultAsync(unit => unit.Id == id, cancellationToken);

        return row is null ? null : TiltMeasuringUnitPersistenceMapping.ToDomain(row);
    }

    public async ValueTask<IReadOnlyList<TiltMeasuringUnitEntity>> AnalyzeAsync(
        TiltMeasuringUnitAnalyzeRequest request,
        CancellationToken cancellationToken)
    {
        var queryable = dbContext.TiltMeasuringUnits.AsNoTracking();
        queryable = queryable.ApplyAnalyze(request);

        List<TiltMeasuringUnitDb> rows = await queryable.ToListAsync(cancellationToken);
        if (rows.Count == 0)
        {
            return [];
        }

        List<TiltMeasuringUnitEntity> entities = new(rows.Count);
        foreach (var row in rows)
        {
            entities.Add(TiltMeasuringUnitPersistenceMapping.ToDomain(row));
        }

        return entities;
    }
}
