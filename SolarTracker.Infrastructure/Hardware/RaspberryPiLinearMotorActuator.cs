using System.Device.Gpio;
using Microsoft.Extensions.Logging;
using SolarTracker.Domain.Abstractions;
using SolarTracker.Infrastructure.Logging;

namespace SolarTracker.Infrastructure.Hardware;

public sealed class RaspberryPiLinearMotorActuator(ILogger<RaspberryPiLinearMotorActuator> logger) : ISteeringCommandReceiver
{
    public ValueTask MoveUpAsync(int moveUpPin, int moveDownPin, CancellationToken cancellationToken) =>
        DriveAsync(moveUpPin, moveDownPin, moveUpPin, "MoveUp", cancellationToken);

    public ValueTask MoveDownAsync(int moveUpPin, int moveDownPin, CancellationToken cancellationToken) =>
        DriveAsync(moveUpPin, moveDownPin, moveDownPin, "MoveDown", cancellationToken);

    public ValueTask StopAsync(int moveUpPin, int moveDownPin, CancellationToken cancellationToken) =>
        StopPinsAsync(moveUpPin, moveDownPin, cancellationToken);

    private ValueTask DriveAsync(
        int moveUpPin,
        int moveDownPin,
        int activePin,
        string direction,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        using var controller = new GpioController();

        OpenOutputPin(controller, moveUpPin);
        OpenOutputPin(controller, moveDownPin);

        controller.Write(moveUpPin, PinValue.Low);
        controller.Write(moveDownPin, PinValue.Low);

        InfrastructureLog.SendingSteeringCommand(
            logger,
            direction,
            moveUpPin,
            moveDownPin);

        controller.Write(activePin, PinValue.High);
        return ValueTask.CompletedTask;
    }

    private ValueTask StopPinsAsync(int moveUpPin, int moveDownPin, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        using var controller = new GpioController();

        OpenOutputPin(controller, moveUpPin);
        OpenOutputPin(controller, moveDownPin);

        InfrastructureLog.SendingSteeringCommand(logger, "Stop", moveUpPin, moveDownPin);
        controller.Write(moveUpPin, PinValue.Low);
        controller.Write(moveDownPin, PinValue.Low);
        return ValueTask.CompletedTask;
    }

    private static void OpenOutputPin(GpioController controller, int pinNumber)
    {
        if (!controller.IsPinOpen(pinNumber))
            controller.OpenPin(pinNumber, PinMode.Output);
    }
}
