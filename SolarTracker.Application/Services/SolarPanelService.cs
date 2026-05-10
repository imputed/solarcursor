using SolarTracker.Application.Analysis;
using SolarTracker.Application.Dtos;
using SolarTracker.Application.Interfaces.Repositories;
using SolarTracker.Application.Mapping;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Application.Interfaces.Services;

public sealed class SolarPanelService(ISolarPanelRepository repository) : ISolarPanelService
{
    public async ValueTask<SolarPanelDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        SolarPanel? entity = await repository.GetByIdAsync(id, cancellationToken);
        return entity is null ? null : DomainToDtoMapper.ToSolarPanelDto(entity);
    }

    public async ValueTask<IReadOnlyList<SolarPanelDto>> AnalyzeAsync(
        SolarPanelAnalyzeRequest request,
        CancellationToken cancellationToken = default)
    {
        IReadOnlyList<SolarPanel> entities = await repository.AnalyzeAsync(request, cancellationToken);
        return entities.Select(DomainToDtoMapper.ToSolarPanelDto).ToList();
    }

    public async ValueTask<IReadOnlyList<SolarPanelDto>> ListAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyList<SolarPanel> entities = await repository.ListAsync(cancellationToken);
        return entities.Select(DomainToDtoMapper.ToSolarPanelDto).ToList();
    }

    public async ValueTask<int> AddAsync(CreateSolarPanelDto dto, CancellationToken cancellationToken = default)
    {
        SolarPanel entity = DomainToDtoMapper.ToDomain(dto);
        await repository.AddAsync(entity, cancellationToken);
        return entity.Id;
    }

    public async ValueTask UpdateAsync(UpdateSolarPanelDto dto, CancellationToken cancellationToken = default)
    {
        SolarPanel entity = DomainToDtoMapper.ToDomain(dto);
        await repository.UpdateAsync(entity, cancellationToken);
    }

    public async ValueTask DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        SolarPanel? entity = await repository.GetByIdAsync(id, cancellationToken);
        if (entity is not null)
        {
            await repository.DeleteAsync(entity, cancellationToken);
        }
    }
}
