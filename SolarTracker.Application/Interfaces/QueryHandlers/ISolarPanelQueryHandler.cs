using SolarTracker.Application.Analysis;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Application.Interfaces.QueryHandlers;

public interface ISolarPanelQueryHandler
{
    ValueTask<SolarPanel?> GetByIdAsync(int id, CancellationToken cancellationToken);

    ValueTask<IReadOnlyList<SolarPanel>> AnalyzeAsync(
        SolarPanelAnalyzeRequest request,
        CancellationToken cancellationToken);
}
