using SolarTracker.Application.Analysis;
using SolarTracker.Application.Dtos;
using SolarTracker.Application.Mapping;
using SolarTracker.Application.Interfaces.Repositories;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Application.Interfaces.Services;

public sealed class LinearMotorService(ILinearMotorRepository repository) : ILinearMotorService
{
    public async ValueTask<LinearMotorDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        LinearMotor? entity = await repository.GetByIdAsync(id, cancellationToken);
        return entity is null ? null : DomainToDtoMapper.ToLinearMotorDto(entity);
    }

    public async ValueTask<IReadOnlyList<LinearMotorDto>> AnalyzeAsync(
        LinearMotorAnalyzeRequest request,
        CancellationToken cancellationToken = default)
    {
        IReadOnlyList<LinearMotor> entities = await repository.AnalyzeAsync(request, cancellationToken);
        return entities.Select(DomainToDtoMapper.ToLinearMotorDto).ToList();
    }

    public async ValueTask<IReadOnlyList<LinearMotorDto>> ListAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyList<LinearMotor> entities = await repository.ListAsync(cancellationToken);
        return entities.Select(DomainToDtoMapper.ToLinearMotorDto).ToList();
    }

    public async ValueTask<int> AddAsync(CreateLinearMotorDto dto, CancellationToken cancellationToken = default)
    {
        LinearMotor entity = DomainToDtoMapper.ToDomain(dto);
        await repository.AddAsync(entity, cancellationToken);
        return entity.Id;
    }

    public async ValueTask UpdateAsync(UpdateLinearMotorDto dto, CancellationToken cancellationToken = default)
    {
        LinearMotor entity = DomainToDtoMapper.ToDomain(dto);
        await repository.UpdateAsync(entity, cancellationToken);
    }

    public async ValueTask DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        LinearMotor? entity = await repository.GetByIdAsync(id, cancellationToken);
        if (entity is not null)
        {
            await repository.DeleteAsync(entity, cancellationToken);
        }
    }
}
