using SolarTracker.Application.Analysis;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Application.Interfaces.Repositories;

public interface ISolarPanelRepository
{
    ValueTask<SolarPanel?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    ValueTask<IReadOnlyList<SolarPanel>> AnalyzeAsync(
        SolarPanelAnalyzeRequest request,
        CancellationToken cancellationToken = default);

    ValueTask<IReadOnlyList<SolarPanel>> ListAsync(CancellationToken cancellationToken = default);

    ValueTask AddAsync(SolarPanel entity, CancellationToken cancellationToken = default);

    ValueTask UpdateAsync(SolarPanel entity, CancellationToken cancellationToken = default);

    ValueTask DeleteAsync(SolarPanel entity, CancellationToken cancellationToken = default);
}
