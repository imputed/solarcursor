using SolarTracker.Application.Dtos;
using SolarTracker.Application.Results;

namespace SolarTracker.Application.Interfaces.Services;

public interface ISolarPanelService
{
    ValueTask<int> AddAsync(CreateSolarPanelDto dto, CancellationToken cancellationToken);

    ValueTask UpdateAsync(UpdateSolarPanelDto dto, CancellationToken cancellationToken);

    ValueTask<Result<SolarPanelCurrentPositionDto>> GetCurrentPositionAsync(int id, CancellationToken cancellationToken);

    ValueTask<Result<SolarPanelCurrentPositionDto>> MoveToOptimumAsync(int id, CancellationToken cancellationToken);

    ValueTask DeleteAsync(int id, CancellationToken cancellationToken);
}
