using SolarTracker.Domain.Abstractions;

namespace SolarTracker.Domain.Entities;

public sealed class LinearMotor
{
    public int Id { get; set; }

    public int SolarPanelId { get; set; }

    public string? Name { get; set; }

    public int MoveUpGpioPin { get; set; }

    public int MoveDownGpioPin { get; set; }

    public async Task MoveUpAsync(
        ISteeringCommandReceiver receiver,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(receiver);
        await receiver.MoveUpAsync(MoveUpGpioPin, MoveDownGpioPin, cancellationToken);
    }

    public async Task MoveDownAsync(
        ISteeringCommandReceiver receiver,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(receiver);
        await receiver.MoveDownAsync(MoveUpGpioPin, MoveDownGpioPin, cancellationToken);
    }

    public async Task StopAsync(
        ISteeringCommandReceiver receiver,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(receiver);
        await receiver.StopAsync(MoveUpGpioPin, MoveDownGpioPin, cancellationToken);
    }
}
