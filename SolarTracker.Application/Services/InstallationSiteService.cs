using SolarTracker.Application.Analysis;
using SolarTracker.Application.Dtos;
using SolarTracker.Application.Mapping;
using SolarTracker.Application.Interfaces.Repositories;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Application.Interfaces.Services;

public sealed class InstallationSiteService(IInstallationSiteRepository repository) : IInstallationSiteService
{
    public async ValueTask<InstallationSiteDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await repository.GetByIdAsync(id, cancellationToken);
        return entity is null ? null : DomainToDtoMapper.ToDto(entity);
    }

    public async ValueTask<IReadOnlyList<InstallationSiteDto>> AnalyzeAsync(
        InstallationSiteAnalyzeRequest request,
        CancellationToken cancellationToken = default)
    {
        IReadOnlyList<InstallationSite> entities = await repository.AnalyzeAsync(request, cancellationToken);
        return entities.Select(DomainToDtoMapper.ToDto).ToList();
    }

    public async ValueTask<IReadOnlyList<InstallationSiteDto>> ListAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyList<InstallationSite> entities = await repository.ListAsync(cancellationToken);
        return entities.Select(DomainToDtoMapper.ToDto).ToList();
    }

    public async ValueTask<int> AddAsync(CreateInstallationSiteDto dto, CancellationToken cancellationToken = default)
    {
        var entity = DomainToDtoMapper.ToDomain(dto);
        await repository.AddAsync(entity, cancellationToken);
        return entity.Id;
    }

    public async ValueTask UpdateAsync(UpdateInstallationSiteDto dto, CancellationToken cancellationToken = default)
    {
        var entity = DomainToDtoMapper.ToDomain(dto);
        await repository.UpdateAsync(entity, cancellationToken);
    }

    public async ValueTask DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await repository.GetByIdAsync(id, cancellationToken);
        if (entity is not null)
        {
            await repository.DeleteAsync(entity, cancellationToken);
        }
    }
}
