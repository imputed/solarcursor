using SolarTracker.Application.Dtos.InstallationSite;
using SolarTracker.Application.Results;

namespace SolarTracker.Application.Interfaces.Services;

public interface IInstallationSiteService
{
    ValueTask<int> AddAsync(CreateInstallationSiteDto dto, CancellationToken cancellationToken);

    ValueTask UpdateAsync(UpdateInstallationSiteDto dto, CancellationToken cancellationToken);

    ValueTask DeleteAsync(int id, CancellationToken cancellationToken);

    Task<Result> Optimize(int id, CancellationToken cancellationToken);
}
