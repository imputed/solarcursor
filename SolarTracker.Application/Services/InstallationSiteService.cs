using SolarTracker.Application.Dtos;
using SolarTracker.Application.Mapping;
using SolarTracker.Application.Interfaces.Repositories;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Application.Interfaces.Services;

public sealed class InstallationSiteService(IInstallationSiteRepository repository) : IInstallationSiteService
{
    public async ValueTask<int> AddAsync(CreateInstallationSiteDto dto, CancellationToken cancellationToken)
    {
        var entity = InstallationSiteMapping.ToDomain(dto);
        await repository.AddAsync(entity, cancellationToken);
        return entity.Id;
    }

    public async ValueTask UpdateAsync(UpdateInstallationSiteDto dto, CancellationToken cancellationToken)
    {
        var entity = InstallationSiteMapping.ToDomain(dto);
        await repository.UpdateAsync(entity, cancellationToken);
    }

    public async ValueTask DeleteAsync(int id, CancellationToken cancellationToken)
        => await repository.DeleteAsync(id, cancellationToken);
}
