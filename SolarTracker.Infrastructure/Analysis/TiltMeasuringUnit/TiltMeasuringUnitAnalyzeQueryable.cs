using SolarTracker.Application.Analysis.TiltMeasuringUnit;
using SolarTracker.Infrastructure.Persistence.Entities;

namespace SolarTracker.Infrastructure.Analysis.TiltMeasuringUnit;

internal static class TiltMeasuringUnitAnalyzeQueryable
{
    internal static IQueryable<TiltMeasuringUnitDb> ApplyAnalyze(
        this IQueryable<TiltMeasuringUnitDb> query,
        TiltMeasuringUnitAnalyzeRequest request)
    {
        query = query.Where(TiltMeasuringUnitAnalyzeExpressionBuilder.BuildPredicate(request.Filter));
        query = ApplySort(query, request.SortBy ?? TiltMeasuringUnitAnalyzeField.Id, request.SortDescending);
        query = query.Skip(request.Skip).Take(request.Take);

        return query;
    }

    private static IQueryable<TiltMeasuringUnitDb> ApplySort(
        IQueryable<TiltMeasuringUnitDb> query,
        TiltMeasuringUnitAnalyzeField sortBy,
        bool descending) =>
        sortBy switch
        {
            TiltMeasuringUnitAnalyzeField.Id => descending ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id),
            TiltMeasuringUnitAnalyzeField.SolarPanelId =>
                descending
                    ? query.OrderByDescending(x => x.SolarPanelId).ThenBy(x => x.Id)
                    : query.OrderBy(x => x.SolarPanelId).ThenBy(x => x.Id),
            TiltMeasuringUnitAnalyzeField.Name =>
                descending
                    ? query.OrderByDescending(x => x.Name).ThenBy(x => x.Id)
                    : query.OrderBy(x => x.Name).ThenBy(x => x.Id),
            TiltMeasuringUnitAnalyzeField.GpioPin =>
                descending
                    ? query.OrderByDescending(x => x.GpioPin).ThenBy(x => x.Id)
                    : query.OrderBy(x => x.GpioPin).ThenBy(x => x.Id),
            _ =>
                descending ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id),
        };
}
