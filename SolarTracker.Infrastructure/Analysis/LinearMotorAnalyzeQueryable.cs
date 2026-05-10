using SolarTracker.Application.Analysis;
using SolarTracker.Infrastructure.Persistence.Entities;

namespace SolarTracker.Infrastructure.Analysis;

internal static class LinearMotorAnalyzeQueryable
{
    internal static IQueryable<LinearMotorDb> ApplyAnalyze(
        this IQueryable<LinearMotorDb> query,
        LinearMotorAnalyzeRequest request)
    {
        query = query.Where(AnalyzeExpressionPrimitives.BuildLinearMotorPredicate(request.Filter));
        query = ApplySort(query, request.SortBy ?? LinearMotorAnalyzeField.Id, request.SortDescending);
        query = query.Skip(request.Skip).Take(request.Take);

        return query;
    }

    private static IQueryable<LinearMotorDb> ApplySort(
        IQueryable<LinearMotorDb> query,
        LinearMotorAnalyzeField sortBy,
        bool descending) =>
        sortBy switch
        {
            LinearMotorAnalyzeField.Id => descending ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id),
            LinearMotorAnalyzeField.InstallationSiteId =>
                descending
                    ? query.OrderByDescending(x => x.InstallationSiteId).ThenBy(x => x.Id)
                    : query.OrderBy(x => x.InstallationSiteId).ThenBy(x => x.Id),
            LinearMotorAnalyzeField.Name =>
                descending
                    ? query.OrderByDescending(x => x.Name).ThenBy(x => x.Id)
                    : query.OrderBy(x => x.Name).ThenBy(x => x.Id),
            _ =>
                descending ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id),
        };
}
