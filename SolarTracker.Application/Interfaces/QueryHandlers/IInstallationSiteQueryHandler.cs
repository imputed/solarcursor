using SolarTracker.Application.Analysis;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Application.Interfaces.QueryHandlers;

public interface IInstallationSiteQueryHandler
{
    ValueTask<InstallationSite?> GetByIdAsync(int id, CancellationToken cancellationToken);

    ValueTask<IReadOnlyList<InstallationSite>> AnalyzeAsync(
        InstallationSiteAnalyzeRequest request,
        CancellationToken cancellationToken);
}
