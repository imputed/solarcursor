namespace SolarTracker.Application.Interfaces.Hardware;

public interface ILinearMotorActuator
{
    ValueTask MoveUpAsync(LinearMotorMovementContext context, CancellationToken cancellationToken);

    ValueTask MoveDownAsync(LinearMotorMovementContext context, CancellationToken cancellationToken);
}
