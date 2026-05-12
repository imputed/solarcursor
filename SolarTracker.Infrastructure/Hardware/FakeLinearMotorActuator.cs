using Microsoft.Extensions.Logging;
using SolarTracker.Application.Interfaces.Hardware;
using SolarTracker.Domain.Entities;
using SolarTracker.Infrastructure.Logging;

namespace SolarTracker.Infrastructure.Hardware;

public sealed class FakeLinearMotorActuator(ILogger<FakeLinearMotorActuator> logger) : ILinearMotorActuator
{
    public async ValueTask MoveUpAsync(LinearMotor linearMotor, int durationMs, CancellationToken cancellationToken)
    {
        InfrastructureLog.SimulatingMoveUp(logger, linearMotor.Id, linearMotor.MoveUpGpioPin, durationMs);

        await Task.Delay(durationMs, cancellationToken);
    }

    public async ValueTask MoveDownAsync(LinearMotor linearMotor, int durationMs, CancellationToken cancellationToken)
    {
        InfrastructureLog.SimulatingMoveDown(logger, linearMotor.Id, linearMotor.MoveDownGpioPin, durationMs);

        await Task.Delay(durationMs, cancellationToken);
    }
}
