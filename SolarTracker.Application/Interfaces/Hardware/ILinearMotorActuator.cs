using SolarTracker.Domain.Entities;

namespace SolarTracker.Application.Interfaces.Hardware;

public interface ILinearMotorActuator
{
    ValueTask MoveUpAsync(LinearMotor linearMotor, int durationMs, CancellationToken cancellationToken);

    ValueTask MoveDownAsync(LinearMotor linearMotor, int durationMs, CancellationToken cancellationToken);
}
