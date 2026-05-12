using System.Device.Gpio;
using Microsoft.Extensions.Logging;
using SolarTracker.Application.Interfaces.Hardware;
using SolarTracker.Domain.Entities;
using SolarTracker.Infrastructure.Logging;

namespace SolarTracker.Infrastructure.Hardware;

public sealed class RaspberryPiLinearMotorActuator(ILogger<RaspberryPiLinearMotorActuator> logger) : ILinearMotorActuator
{
    public ValueTask MoveUpAsync(LinearMotor linearMotor, int durationMs, CancellationToken cancellationToken) =>
        DriveAsync(linearMotor, linearMotor.MoveUpGpioPin, linearMotor.MoveDownGpioPin, durationMs, "MoveUp", cancellationToken);

    public ValueTask MoveDownAsync(LinearMotor linearMotor, int durationMs, CancellationToken cancellationToken) =>
        DriveAsync(linearMotor, linearMotor.MoveDownGpioPin, linearMotor.MoveUpGpioPin, durationMs, "MoveDown", cancellationToken);

    private async ValueTask DriveAsync(
        LinearMotor linearMotor,
        int activePin,
        int inactivePin,
        int durationMs,
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
            linearMotor.Id,
            linearMotor.SolarPanelId,
            activePin,
            durationMs);

        controller.Write(activePin, PinValue.High);

        try
        {
            await Task.Delay(durationMs, cancellationToken);
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
