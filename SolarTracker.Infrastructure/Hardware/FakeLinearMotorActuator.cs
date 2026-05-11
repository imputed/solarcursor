using Microsoft.Extensions.Logging;
using SolarTracker.Application.Interfaces.Hardware;

namespace SolarTracker.Infrastructure.Hardware;

public sealed class FakeLinearMotorActuator(ILogger<FakeLinearMotorActuator> logger) : ILinearMotorActuator
{
    public async ValueTask MoveUpAsync(LinearMotorMovementContext context, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Simulating MoveUp for linear motor {LinearMotorId} on pin {MoveUpPin} for {DurationMs} ms.",
            context.LinearMotorId,
            context.MoveUpGpioPin,
            context.DurationMs);

        await Task.Delay(context.DurationMs, cancellationToken);
    }

    public async ValueTask MoveDownAsync(LinearMotorMovementContext context, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Simulating MoveDown for linear motor {LinearMotorId} on pin {MoveDownPin} for {DurationMs} ms.",
            context.LinearMotorId,
            context.MoveDownGpioPin,
            context.DurationMs);

        await Task.Delay(context.DurationMs, cancellationToken);
    }
}
