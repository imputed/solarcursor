using SolarTracker.Application.Analysis;
using SolarTracker.Infrastructure.Persistence.Entities;

namespace SolarTracker.Infrastructure.Analysis;

internal static class SolarPanelAnalyzeQueryable
{
    internal static IQueryable<SolarPanelDb> ApplyAnalyze(
        this IQueryable<SolarPanelDb> query,
        SolarPanelAnalyzeRequest request)
    {
        query = query.Where(AnalyzeExpressionPrimitives.BuildSolarPanelPredicate(request.Filter));
        query = ApplySort(query, request.SortBy ?? SolarPanelAnalyzeField.Id, request.SortDescending);
        query = query.Skip(request.Skip).Take(request.Take);

        return query;
    }

    private static IQueryable<SolarPanelDb> ApplySort(
        IQueryable<SolarPanelDb> query,
        SolarPanelAnalyzeField sortBy,
        bool descending) =>
        sortBy switch
        {
            SolarPanelAnalyzeField.Id => descending ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id),
            SolarPanelAnalyzeField.InstallationSiteId =>
                descending
                    ? query.OrderByDescending(x => x.InstallationSiteId).ThenBy(x => x.Id)
                    : query.OrderBy(x => x.InstallationSiteId).ThenBy(x => x.Id),
            SolarPanelAnalyzeField.SerialNumber =>
                descending
                    ? query.OrderByDescending(x => x.SerialNumber).ThenBy(x => x.Id)
                    : query.OrderBy(x => x.SerialNumber).ThenBy(x => x.Id),
            _ =>
                descending ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id),
        };
}
