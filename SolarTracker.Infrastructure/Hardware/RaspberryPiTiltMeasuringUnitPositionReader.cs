using System.Device.Gpio;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using SolarTracker.Domain.Abstractions;
using SolarTracker.Domain.Entities;
using SolarTracker.Domain.ValueObjects;

namespace SolarTracker.Infrastructure.Hardware;

public sealed class RaspberryPiTiltMeasuringUnitPositionReader(
    ILogger<RaspberryPiTiltMeasuringUnitPositionReader> logger) : ITiltMeasuringUnitPositionReader
{
    private const int SampleCount = 3;
    private const double MaxTiltDegrees = 90d;

    public async ValueTask<TiltMeasurement> GetCurrentPositionAsync(
        TiltMeasuringUnit unit,
        CancellationToken cancellationToken)
    {
        using var controller = new GpioController();
        OpenInputPin(controller, unit.GpioPin);

        double dutyCycleSum = 0d;
        for (int i = 0; i < SampleCount; i++)
        {
            dutyCycleSum += await ReadDutyCycleAsync(controller, unit.GpioPin, cancellationToken);
        }

        double averagedDutyCycle = dutyCycleSum / SampleCount;
        double degrees = Math.Clamp(averagedDutyCycle * MaxTiltDegrees, 0d, MaxTiltDegrees);

        logger.LogInformation(
            "Read tilt {Degrees} degrees from measuring unit {TiltMeasuringUnitId} on GPIO pin {GpioPin}.",
            degrees,
            unit.Id,
            unit.GpioPin);

        return new TiltMeasurement(degrees, DateTime.UtcNow);
    }

    private static void OpenInputPin(GpioController controller, int pinNumber)
    {
        if (!controller.IsPinOpen(pinNumber))
        {
            controller.OpenPin(pinNumber, PinMode.Input);
        }
    }

    private static async ValueTask<double> ReadDutyCycleAsync(
        GpioController controller,
        int pinNumber,
        CancellationToken cancellationToken)
    {
        await WaitForEdgeAsync(controller, pinNumber, PinEventTypes.Rising, cancellationToken);

        Stopwatch highStopwatch = Stopwatch.StartNew();
        await WaitForEdgeAsync(controller, pinNumber, PinEventTypes.Falling, cancellationToken);
        highStopwatch.Stop();

        Stopwatch lowStopwatch = Stopwatch.StartNew();
        await WaitForEdgeAsync(controller, pinNumber, PinEventTypes.Rising, cancellationToken);
        lowStopwatch.Stop();

        double periodMs = highStopwatch.Elapsed.TotalMilliseconds + lowStopwatch.Elapsed.TotalMilliseconds;
        if (periodMs <= 0d)
        {
            throw new InvalidOperationException("PWM period must be greater than zero.");
        }

        return highStopwatch.Elapsed.TotalMilliseconds / periodMs;
    }

    private static async ValueTask WaitForEdgeAsync(
        GpioController controller,
        int pinNumber,
        PinEventTypes edge,
        CancellationToken cancellationToken)
    {
        TaskCompletionSource taskCompletionSource = new(TaskCreationOptions.RunContinuationsAsynchronously);

        void Handler(object sender, PinValueChangedEventArgs args)
        {
            if ((args.ChangeType & edge) != 0)
            {
                taskCompletionSource.TrySetResult();
            }
        }

        controller.RegisterCallbackForPinValueChangedEvent(pinNumber, edge, Handler);

        try
        {
            using CancellationTokenRegistration registration =
                cancellationToken.Register(() => taskCompletionSource.TrySetCanceled(cancellationToken));
            await taskCompletionSource.Task.ConfigureAwait(false);
        }
        finally
        {
            controller.UnregisterCallbackForPinValueChangedEvent(pinNumber, Handler);
        }
    }
}
