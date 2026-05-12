using SolarTracker.Application.Analysis.CurrentMeasuringUnit;
using SolarTracker.Infrastructure.Persistence.Entities;

namespace SolarTracker.Infrastructure.Analysis.CurrentMeasuringUnit;

internal static class CurrentMeasuringUnitAnalyzeQueryable
{
    internal static IQueryable<CurrentMeasuringUnitDb> ApplyAnalyze(
        this IQueryable<CurrentMeasuringUnitDb> query,
        CurrentMeasuringUnitAnalyzeRequest request)
    {
        query = query.Where(CurrentMeasuringUnitAnalyzeExpressionBuilder.BuildPredicate(request.Filter));
        query = ApplySort(query, request.SortBy ?? CurrentMeasuringUnitAnalyzeField.Id, request.SortDescending);
        query = query.Skip(request.Skip).Take(request.Take);

        return query;
    }

    private static IQueryable<CurrentMeasuringUnitDb> ApplySort(
        IQueryable<CurrentMeasuringUnitDb> query,
        CurrentMeasuringUnitAnalyzeField sortBy,
        bool descending) =>
        sortBy switch
        {
            CurrentMeasuringUnitAnalyzeField.Id => descending ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id),
            CurrentMeasuringUnitAnalyzeField.SolarPanelId =>
                descending
                    ? query.OrderByDescending(x => x.SolarPanelId).ThenBy(x => x.Id)
                    : query.OrderBy(x => x.SolarPanelId).ThenBy(x => x.Id),
            CurrentMeasuringUnitAnalyzeField.Name =>
                descending
                    ? query.OrderByDescending(x => x.Name).ThenBy(x => x.Id)
                    : query.OrderBy(x => x.Name).ThenBy(x => x.Id),
            CurrentMeasuringUnitAnalyzeField.GpioPin =>
                descending
                    ? query.OrderByDescending(x => x.GpioPin).ThenBy(x => x.Id)
                    : query.OrderBy(x => x.GpioPin).ThenBy(x => x.Id),
            _ =>
                descending ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id),
        };
}
