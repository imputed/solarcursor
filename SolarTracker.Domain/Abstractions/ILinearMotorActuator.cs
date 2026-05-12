using SolarTracker.Domain.ValueObjects;

namespace SolarTracker.Domain.Abstractions;

public interface ILinearMotorActuator
{
    ValueTask MoveUpAsync(LinearMotorMovementContext context, CancellationToken cancellationToken);

    ValueTask MoveDownAsync(LinearMotorMovementContext context, CancellationToken cancellationToken);
}
