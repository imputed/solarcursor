using SolarTracker.Domain.Entities;

namespace SolarTracker.Application.Interfaces.Repositories;

public interface IInstallationSiteRepository
{
    ValueTask AddAsync(InstallationSite entity, CancellationToken cancellationToken);

    ValueTask UpdateAsync(InstallationSite entity, CancellationToken cancellationToken);

    ValueTask DeleteAsync(int id, CancellationToken cancellationToken);
}
