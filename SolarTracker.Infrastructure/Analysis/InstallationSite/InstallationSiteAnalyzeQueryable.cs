using SolarTracker.Application.Analysis.InstallationSite;
using SolarTracker.Infrastructure.Persistence.Entities;

namespace SolarTracker.Infrastructure.Analysis.InstallationSite;

internal static class InstallationSiteAnalyzeQueryable
{
    internal static IQueryable<InstallationSiteDb> ApplyAnalyze(
        this IQueryable<InstallationSiteDb> query,
        InstallationSiteAnalyzeRequest request)
    {
        query = query.Where(InstallationSiteAnalyzeExpressionBuilder.BuildPredicate(request.Filter));
        query = ApplySort(query, request.SortBy ?? InstallationSiteAnalyzeField.Id, request.SortDescending);
        query = query.Skip(request.Skip).Take(request.Take);

        return query;
    }

    private static IQueryable<InstallationSiteDb> ApplySort(
        IQueryable<InstallationSiteDb> query,
        InstallationSiteAnalyzeField sortBy,
        bool descending) =>
        sortBy switch
        {
            InstallationSiteAnalyzeField.Id => descending ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id),
            InstallationSiteAnalyzeField.Name =>
                descending ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
            _ =>
                descending ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id),
        };
}
