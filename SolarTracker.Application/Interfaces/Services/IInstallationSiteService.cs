using SolarTracker.Application.Analysis;
using SolarTracker.Application.Dtos;

namespace SolarTracker.Application.Interfaces.Services;

public interface IInstallationSiteService
{
    ValueTask<InstallationSiteDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    ValueTask<IReadOnlyList<InstallationSiteDto>> AnalyzeAsync(
        InstallationSiteAnalyzeRequest request,
        CancellationToken cancellationToken = default);

    ValueTask<IReadOnlyList<InstallationSiteDto>> ListAsync(CancellationToken cancellationToken = default);

    ValueTask<int> AddAsync(CreateInstallationSiteDto dto, CancellationToken cancellationToken = default);

    ValueTask UpdateAsync(UpdateInstallationSiteDto dto, CancellationToken cancellationToken = default);

    ValueTask DeleteAsync(int id, CancellationToken cancellationToken = default);
}
