using SolarTracker.Application.Analysis;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Application.Interfaces.Repositories;

public interface IInstallationSiteRepository
{
    ValueTask<InstallationSite?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    ValueTask<IReadOnlyList<InstallationSite>> AnalyzeAsync(
        InstallationSiteAnalyzeRequest request,
        CancellationToken cancellationToken = default);

    ValueTask<IReadOnlyList<InstallationSite>> ListAsync(CancellationToken cancellationToken = default);

    ValueTask AddAsync(InstallationSite entity, CancellationToken cancellationToken = default);

    ValueTask UpdateAsync(InstallationSite entity, CancellationToken cancellationToken = default);

    ValueTask DeleteAsync(InstallationSite entity, CancellationToken cancellationToken = default);
}
