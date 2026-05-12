using SolarTracker.Application.Analysis.CurrentMeasuringUnit;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Application.Interfaces.QueryHandlers;

public interface ICurrentMeasuringUnitQueryHandler
{
    ValueTask<CurrentMeasuringUnit?> GetByIdAsync(int id, CancellationToken cancellationToken);

    ValueTask<IReadOnlyList<CurrentMeasuringUnit>> AnalyzeAsync(CurrentMeasuringUnitAnalyzeRequest request, CancellationToken cancellationToken);
}
