using SolarTracker.Application.Dtos;
using SolarTracker.Application.Results;

namespace SolarTracker.Application.Interfaces.Services;

public interface IInstallationSiteService
{
    ValueTask<int> AddAsync(CreateInstallationSiteDto dto, CancellationToken cancellationToken);

    ValueTask UpdateAsync(UpdateInstallationSiteDto dto, CancellationToken cancellationToken);

    ValueTask<Result<IReadOnlyList<SolarPanelCurrentPositionDto>>> MoveToOptimumAsync(int id, CancellationToken cancellationToken);

    ValueTask DeleteAsync(int id, CancellationToken cancellationToken);
}
