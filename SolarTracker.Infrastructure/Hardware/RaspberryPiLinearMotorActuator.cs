using System.Device.Gpio;
using Microsoft.Extensions.Logging;
using SolarTracker.Domain.Abstractions;
using SolarTracker.Domain.ValueObjects;
using SolarTracker.Infrastructure.Logging;

namespace SolarTracker.Infrastructure.Hardware;

public sealed class RaspberryPiLinearMotorActuator(ILogger<RaspberryPiLinearMotorActuator> logger) : ILinearMotorActuator
{
    public ValueTask MoveUpAsync(LinearMotorMovementContext context, CancellationToken cancellationToken) =>
        DriveAsync(context, context.MoveUpGpioPin, context.MoveDownGpioPin, "MoveUp", cancellationToken);

    public ValueTask MoveDownAsync(LinearMotorMovementContext context, CancellationToken cancellationToken) =>
        DriveAsync(context, context.MoveDownGpioPin, context.MoveUpGpioPin, "MoveDown", cancellationToken);

    private async ValueTask DriveAsync(
        LinearMotorMovementContext context,
        int activePin,
        int inactivePin,
        string direction,
        CancellationToken cancellationToken)
    {
        using var controller = new GpioController();

        OpenOutputPin(controller, activePin);
        OpenOutputPin(controller, inactivePin);

        controller.Write(inactivePin, PinValue.Low);
        controller.Write(activePin, PinValue.Low);

        InfrastructureLog.DrivingLinearMotor(
            logger,
            direction,
            context.LinearMotorId,
            context.InstallationSiteId,
            activePin,
            context.DurationMs);

        controller.Write(activePin, PinValue.High);

        try
        {
            await Task.Delay(context.DurationMs, cancellationToken);
        }
        finally
        {
            controller.Write(activePin, PinValue.Low);
            controller.Write(inactivePin, PinValue.Low);
        }
    }

    private static void OpenOutputPin(GpioController controller, int pinNumber)
    {
        if (!controller.IsPinOpen(pinNumber))
            controller.OpenPin(pinNumber, PinMode.Output);
    }
}
