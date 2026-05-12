using Microsoft.EntityFrameworkCore;
using SolarTracker.Application.Analysis;
using SolarTracker.Application.Interfaces.QueryHandlers;
using SolarTracker.Infrastructure.Analysis.SolarPanel;
using SolarTracker.Infrastructure.Persistence;
using SolarTracker.Infrastructure.Persistence.Entities;
using SolarTracker.Infrastructure.Persistence.Mapping;
using SolarPanelEntity = SolarTracker.Domain.Entities.SolarPanel;

namespace SolarTracker.Infrastructure.QueryHandlers.SolarPanel;

public sealed class SolarPanelQueryHandler(SolarTrackerDbContext dbContext) : ISolarPanelQueryHandler
{
    public async ValueTask<SolarPanelEntity?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var row = await dbContext.SolarPanels
            .AsNoTracking()
            .Include(panel => panel.TiltMeasuringUnit)
            .Include(panel => panel.CurrentMeasuringUnit)
            .Include(panel => panel.LinearMotors)
            .AsSplitQuery()
            .FirstOrDefaultAsync(panel => panel.Id == id, cancellationToken);

        return row is null ? null : SolarPanelPersistenceMapping.ToDomain(row, loadChildren: true);
    }

    public async ValueTask<IReadOnlyList<SolarPanelEntity>> AnalyzeAsync(
        SolarPanelAnalyzeRequest request,
        CancellationToken cancellationToken)
    {
        var queryable = dbContext.SolarPanels.AsNoTracking();
        queryable = queryable.ApplyAnalyze(request);

        List<SolarPanelDb> rows = await queryable.ToListAsync(cancellationToken);
        if (rows.Count == 0)
            return [];

        List<SolarPanelEntity> entities = new(rows.Count);
        foreach (var row in rows)
        {
            entities.Add(SolarPanelPersistenceMapping.ToDomain(row, loadChildren: false));
        }

        return entities;
    }
}
