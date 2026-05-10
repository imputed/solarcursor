using SolarTracker.Application.Analysis;
using SolarTracker.Application.Dtos;

namespace SolarTracker.Application.Interfaces.Services;

public interface ISolarPanelService
{
    ValueTask<SolarPanelDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    ValueTask<IReadOnlyList<SolarPanelDto>> AnalyzeAsync(
        SolarPanelAnalyzeRequest request,
        CancellationToken cancellationToken = default);

    ValueTask<IReadOnlyList<SolarPanelDto>> ListAsync(CancellationToken cancellationToken = default);

    ValueTask<int> AddAsync(CreateSolarPanelDto dto, CancellationToken cancellationToken = default);

    ValueTask UpdateAsync(UpdateSolarPanelDto dto, CancellationToken cancellationToken = default);

    ValueTask DeleteAsync(int id, CancellationToken cancellationToken = default);
}
