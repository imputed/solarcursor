using SolarTracker.Application.Analysis;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Application.Interfaces.QueryHandlers;

public interface ITiltMeasuringUnitQueryHandler
{
    ValueTask<TiltMeasuringUnit?> GetByIdAsync(int id, CancellationToken cancellationToken);

    ValueTask<IReadOnlyList<TiltMeasuringUnit>> AnalyzeAsync(TiltMeasuringUnitAnalyzeRequest request, CancellationToken cancellationToken);
}
