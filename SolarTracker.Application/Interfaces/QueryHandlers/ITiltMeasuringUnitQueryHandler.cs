using SolarTracker.Application.Analysis.TiltMeasuringUnit;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Application.Interfaces.QueryHandlers;

public interface ITiltMeasuringUnitQueryHandler
{
    ValueTask<TiltMeasuringUnit?> GetByIdAsync(int id, CancellationToken cancellationToken);

    ValueTask<IReadOnlyList<TiltMeasuringUnit>> AnalyzeAsync(TiltMeasuringUnitAnalyzeRequest request, CancellationToken cancellationToken);
}
