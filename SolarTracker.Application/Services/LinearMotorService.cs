using SolarTracker.Application.Dtos;
using SolarTracker.Application.Mapping;
using SolarTracker.Application.Interfaces.Repositories;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Application.Interfaces.Services;

public sealed class LinearMotorService(ILinearMotorRepository repository) : ILinearMotorService
{
    public async ValueTask<int> AddAsync(CreateLinearMotorDto dto, CancellationToken cancellationToken)
    {
        LinearMotor entity = LinearMotorMapping.ToDomain(dto);
        await repository.AddAsync(entity, cancellationToken);
        return entity.Id;
    }

    public async ValueTask UpdateAsync(UpdateLinearMotorDto dto, CancellationToken cancellationToken)
    {
        LinearMotor entity = LinearMotorMapping.ToDomain(dto);
        await repository.UpdateAsync(entity, cancellationToken);
    }

    public async ValueTask DeleteAsync(int id, CancellationToken cancellationToken)
        => await repository.DeleteAsync(id, cancellationToken);
}
