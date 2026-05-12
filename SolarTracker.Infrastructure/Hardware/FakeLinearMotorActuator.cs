using Microsoft.Extensions.Logging;
using SolarTracker.Domain.Abstractions;
using SolarTracker.Infrastructure.Logging;

namespace SolarTracker.Infrastructure.Hardware;

public sealed class FakeLinearMotorActuator(ILogger<FakeLinearMotorActuator> logger) : ISteeringCommandReceiver
{
    public ValueTask MoveUpAsync(int moveUpPin, int moveDownPin, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        InfrastructureLog.SimulatingMoveUp(logger, moveUpPin, moveDownPin);
        return ValueTask.CompletedTask;
    }

    public ValueTask MoveDownAsync(int moveUpPin, int moveDownPin, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        InfrastructureLog.SimulatingMoveDown(logger, moveUpPin, moveDownPin);
        return ValueTask.CompletedTask;
    }

    public ValueTask StopAsync(int moveUpPin, int moveDownPin, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        InfrastructureLog.SimulatingStop(logger, moveUpPin, moveDownPin);
        return ValueTask.CompletedTask;
    }
}
