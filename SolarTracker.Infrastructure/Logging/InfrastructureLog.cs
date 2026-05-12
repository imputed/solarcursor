using Microsoft.Extensions.Logging;

namespace SolarTracker.Infrastructure.Logging;

internal static partial class InfrastructureLog
{
    [LoggerMessage(EventId = 2000, Level = LogLevel.Warning, Message = "MoveToOptimum reached the configured maximum step count for solar panel {SolarPanelId}.")]
    internal static partial void MoveToOptimumMaxStepsReached(ILogger logger, int solarPanelId);

    [LoggerMessage(EventId = 2001, Level = LogLevel.Warning, Message = "Recovering solar panel {SolarPanelId} after motor movement failed at linear motor {LinearMotorId}. {Code}: {Message}")]
    internal static partial void RecoveringSolarPanelMovement(
        ILogger logger,
        int solarPanelId,
        int linearMotorId,
        string code,
        string message);

    [LoggerMessage(EventId = 2002, Level = LogLevel.Warning, Message = "Automatic optimization failed for solar panel {SolarPanelId}. {Code}: {Message}")]
    internal static partial void AutomaticOptimizationFailed(ILogger logger, int solarPanelId, string code, string message);

    [LoggerMessage(EventId = 2003, Level = LogLevel.Error, Message = "Automatic solar panel optimization loop failed.")]
    internal static partial void AutomaticOptimizationLoopFailed(ILogger logger, Exception exception);

    [LoggerMessage(EventId = 2004, Level = LogLevel.Information, Message = "Driving linear motor {LinearMotorId} at installation site {InstallationSiteId} in direction {Direction} using GPIO pin {ActivePin} for {DurationMs} ms.")]
    internal static partial void DrivingLinearMotor(
        ILogger logger,
        string direction,
        int linearMotorId,
        int installationSiteId,
        int activePin,
        int durationMs);

    [LoggerMessage(EventId = 2005, Level = LogLevel.Debug, Message = "Simulating MoveUp for linear motor {LinearMotorId} on pin {MoveUpPin} for {DurationMs} ms.")]
    internal static partial void SimulatingMoveUp(ILogger logger, int linearMotorId, int moveUpPin, int durationMs);

    [LoggerMessage(EventId = 2006, Level = LogLevel.Debug, Message = "Simulating MoveDown for linear motor {LinearMotorId} on pin {MoveDownPin} for {DurationMs} ms.")]
    internal static partial void SimulatingMoveDown(ILogger logger, int linearMotorId, int moveDownPin, int durationMs);

    [LoggerMessage(EventId = 2007, Level = LogLevel.Debug, Message = "Read tilt {Degrees} degrees from measuring unit {TiltMeasuringUnitId} on GPIO pin {GpioPin}.")]
    internal static partial void ReadTiltPosition(ILogger logger, double degrees, int tiltMeasuringUnitId, int gpioPin);

    [LoggerMessage(EventId = 2008, Level = LogLevel.Debug, Message = "Simulating tilt read for measuring unit {TiltMeasuringUnitId} on GPIO pin {GpioPin}.")]
    internal static partial void SimulatingTiltRead(ILogger logger, int tiltMeasuringUnitId, int gpioPin);

    [LoggerMessage(EventId = 2009, Level = LogLevel.Warning, Message = "Linear motor {LinearMotorId} was not found while building the movement context.")]
    internal static partial void LinearMotorNotFound(ILogger logger, int linearMotorId);

    [LoggerMessage(EventId = 2010, Level = LogLevel.Warning, Message = "Solar panel {SolarPanelId} for linear motor {LinearMotorId} was not found while building the movement context.")]
    internal static partial void SolarPanelNotFoundForLinearMotor(ILogger logger, int solarPanelId, int linearMotorId);

    [LoggerMessage(EventId = 2011, Level = LogLevel.Warning, Message = "Installation site {InstallationSiteId} for linear motor {LinearMotorId} was not found while building the movement context.")]
    internal static partial void InstallationSiteNotFoundForLinearMotor(
        ILogger logger,
        int installationSiteId,
        int linearMotorId);
}
