using SolarTracker.Domain.Entities;

namespace SolarTracker.Application.Interfaces.Repositories;

public interface ILinearMotorRepository
{
    ValueTask AddAsync(LinearMotor entity, CancellationToken cancellationToken);

    ValueTask UpdateAsync(LinearMotor entity, CancellationToken cancellationToken);

    ValueTask DeleteAsync(int id, CancellationToken cancellationToken);
}
