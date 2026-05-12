using Microsoft.Extensions.Logging;
using SolarTracker.Domain.Abstractions;
using SolarTracker.Domain.ValueObjects;
using SolarTracker.Infrastructure.Logging;

namespace SolarTracker.Infrastructure.Hardware;

public sealed class FakeLinearMotorActuator(ILogger<FakeLinearMotorActuator> logger) : ILinearMotorActuator
{
    public async ValueTask MoveUpAsync(LinearMotorMovementContext context, CancellationToken cancellationToken)
    {
        InfrastructureLog.SimulatingMoveUp(logger, context.LinearMotorId, context.MoveUpGpioPin, context.DurationMs);

        await Task.Delay(context.DurationMs, cancellationToken);
    }

    public async ValueTask MoveDownAsync(LinearMotorMovementContext context, CancellationToken cancellationToken)
    {
        InfrastructureLog.SimulatingMoveDown(logger, context.LinearMotorId, context.MoveDownGpioPin, context.DurationMs);

        await Task.Delay(context.DurationMs, cancellationToken);
    }
}
