using Microsoft.EntityFrameworkCore;
using SolarTracker.Application.Analysis.InstallationSite;
using SolarTracker.Application.Interfaces.QueryHandlers;
using SolarTracker.Infrastructure.Analysis.InstallationSite;
using SolarTracker.Infrastructure.Persistence;
using SolarTracker.Infrastructure.Persistence.Entities;
using SolarTracker.Infrastructure.Persistence.Mapping;
using InstallationSiteEntity = SolarTracker.Domain.Entities.InstallationSite;

namespace SolarTracker.Infrastructure.QueryHandlers.InstallationSite;

public sealed class InstallationSiteQueryHandler(SolarTrackerDbContext dbContext) : IInstallationSiteQueryHandler
{
    public async ValueTask<InstallationSiteEntity?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var row = await dbContext.InstallationSites
            .AsNoTracking()
            .Include(site => site.SolarPanels)
                .ThenInclude(panel => panel.TiltMeasuringUnit)
            .Include(site => site.SolarPanels)
                .ThenInclude(panel => panel.CurrentMeasuringUnit)
            .Include(site => site.SolarPanels)
                .ThenInclude(panel => panel.LinearMotors)
            .AsSplitQuery()
            .FirstOrDefaultAsync(site => site.Id == id, cancellationToken);

        return row is null ? null : InstallationSitePersistenceMapping.ToDomain(row, loadChildren: true);
    }

    public async ValueTask<IReadOnlyList<InstallationSiteEntity>> AnalyzeAsync(
        InstallationSiteAnalyzeRequest request,
        CancellationToken cancellationToken)
    {
        var queryable = dbContext.InstallationSites.AsNoTracking();
        queryable = queryable.ApplyAnalyze(request);

        List<InstallationSiteDb> rows = await queryable.ToListAsync(cancellationToken);
        if (rows.Count == 0)
            return [];

        List<InstallationSiteEntity> entities = new(rows.Count);
        foreach (var row in rows)
        {
            entities.Add(InstallationSitePersistenceMapping.ToDomain(row, loadChildren: false));
        }

        return entities;
    }
}
