using Microsoft.Extensions.Logging;

namespace SolarTracker.Application.Logging;

internal static partial class ApplicationLog
{
    [LoggerMessage(EventId = 1000, Level = LogLevel.Information, Message = "Created installation site {InstallationSiteId} with name '{Name}'.")]
    internal static partial void CreatedInstallationSite(ILogger logger, int installationSiteId, string name);

    [LoggerMessage(EventId = 1001, Level = LogLevel.Information, Message = "Updated installation site {InstallationSiteId}.")]
    internal static partial void UpdatedInstallationSite(ILogger logger, int installationSiteId);

    [LoggerMessage(EventId = 1002, Level = LogLevel.Information, Message = "Deleted installation site {InstallationSiteId}.")]
    internal static partial void DeletedInstallationSite(ILogger logger, int installationSiteId);

    [LoggerMessage(EventId = 1003, Level = LogLevel.Information, Message = "Created solar panel {SolarPanelId} for installation site {InstallationSiteId}.")]
    internal static partial void CreatedSolarPanel(ILogger logger, int solarPanelId, int installationSiteId);

    [LoggerMessage(EventId = 1004, Level = LogLevel.Information, Message = "Updated solar panel {SolarPanelId}.")]
    internal static partial void UpdatedSolarPanel(ILogger logger, int solarPanelId);

    [LoggerMessage(EventId = 1005, Level = LogLevel.Information, Message = "Deleted solar panel {SolarPanelId}.")]
    internal static partial void DeletedSolarPanel(ILogger logger, int solarPanelId);

    [LoggerMessage(EventId = 1006, Level = LogLevel.Debug, Message = "Retrieved current position for solar panel {SolarPanelId}. Current {CurrentPosition}, optimal {OptimalPosition}.")]
    internal static partial void RetrievedSolarPanelCurrentPosition(
        ILogger logger,
        int solarPanelId,
        double currentPosition,
        double optimalPosition);

    [LoggerMessage(EventId = 1007, Level = LogLevel.Warning, Message = "Current position for solar panel {SolarPanelId} could not be retrieved. {Code}: {Message}")]
    internal static partial void SolarPanelCurrentPositionUnavailable(
        ILogger logger,
        int solarPanelId,
        string code,
        string message);

    [LoggerMessage(EventId = 1010, Level = LogLevel.Information, Message = "Created linear motor {LinearMotorId} for solar panel {SolarPanelId}.")]
    internal static partial void CreatedLinearMotor(ILogger logger, int linearMotorId, int solarPanelId);

    [LoggerMessage(EventId = 1011, Level = LogLevel.Information, Message = "Updated linear motor {LinearMotorId}.")]
    internal static partial void UpdatedLinearMotor(ILogger logger, int linearMotorId);

    [LoggerMessage(EventId = 1012, Level = LogLevel.Information, Message = "Deleted linear motor {LinearMotorId}.")]
    internal static partial void DeletedLinearMotor(ILogger logger, int linearMotorId);

    [LoggerMessage(EventId = 1013, Level = LogLevel.Information, Message = "Created tilt measuring unit {TiltMeasuringUnitId} for solar panel {SolarPanelId}.")]
    internal static partial void CreatedTiltMeasuringUnit(ILogger logger, int tiltMeasuringUnitId, int solarPanelId);

    [LoggerMessage(EventId = 1014, Level = LogLevel.Information, Message = "Updated tilt measuring unit {TiltMeasuringUnitId}.")]
    internal static partial void UpdatedTiltMeasuringUnit(ILogger logger, int tiltMeasuringUnitId);

    [LoggerMessage(EventId = 1015, Level = LogLevel.Information, Message = "Deleted tilt measuring unit {TiltMeasuringUnitId}.")]
    internal static partial void DeletedTiltMeasuringUnit(ILogger logger, int tiltMeasuringUnitId);

    [LoggerMessage(EventId = 1016, Level = LogLevel.Information, Message = "Created current measuring unit {CurrentMeasuringUnitId} for solar panel {SolarPanelId}.")]
    internal static partial void CreatedCurrentMeasuringUnit(ILogger logger, int currentMeasuringUnitId, int solarPanelId);

    [LoggerMessage(EventId = 1017, Level = LogLevel.Information, Message = "Updated current measuring unit {CurrentMeasuringUnitId}.")]
    internal static partial void UpdatedCurrentMeasuringUnit(ILogger logger, int currentMeasuringUnitId);

    [LoggerMessage(EventId = 1018, Level = LogLevel.Information, Message = "Deleted current measuring unit {CurrentMeasuringUnitId}.")]
    internal static partial void DeletedCurrentMeasuringUnit(ILogger logger, int currentMeasuringUnitId);

    [LoggerMessage(EventId = 1019, Level = LogLevel.Warning, Message = "Solar panel {SolarPanelId} was not found while reading solar tracking configuration.")]
    internal static partial void SolarPanelNotFoundForTrackingConfiguration(ILogger logger, int solarPanelId);

    [LoggerMessage(EventId = 1020, Level = LogLevel.Debug, Message = "Retrieved solar tracking configuration for solar panel {SolarPanelId}.")]
    internal static partial void RetrievedSolarTrackingConfiguration(ILogger logger, int solarPanelId);

    [LoggerMessage(EventId = 1021, Level = LogLevel.Information, Message = "Updated solar tracking configuration for solar panel {SolarPanelId}. Threshold {PositionThresholdDegrees}, duration {StepDurationMs}, max steps {MaxAdjustmentSteps}.")]
    internal static partial void UpdatedSolarTrackingConfiguration(
        ILogger logger,
        int solarPanelId,
        double positionThresholdDegrees,
        int stepDurationMs,
        int maxAdjustmentSteps);

    [LoggerMessage(EventId = 1022, Level = LogLevel.Warning, Message = "Solar panel {SolarPanelId} was not found while reading solar panel optimization state.")]
    internal static partial void SolarPanelNotFoundForOptimizationState(ILogger logger, int solarPanelId);

    [LoggerMessage(EventId = 1023, Level = LogLevel.Debug, Message = "Retrieved solar panel optimization state for solar panel {SolarPanelId}. Enabled: {IsEnabled}.")]
    internal static partial void RetrievedSolarPanelOptimizationState(ILogger logger, int solarPanelId, bool isEnabled);

    [LoggerMessage(EventId = 1024, Level = LogLevel.Information, Message = "Updated solar panel optimization state for solar panel {SolarPanelId}. Enabled: {IsEnabled}.")]
    internal static partial void UpdatedSolarPanelOptimizationState(ILogger logger, int solarPanelId, bool isEnabled);

    [LoggerMessage(EventId = 1025, Level = LogLevel.Debug, Message = "Retrieved solar optimization schedule configuration. Interval {IntervalMinutes} minutes.")]
    internal static partial void RetrievedSolarOptimizationScheduleConfiguration(ILogger logger, int intervalMinutes);

    [LoggerMessage(EventId = 1026, Level = LogLevel.Information, Message = "Updated solar optimization schedule configuration. Interval {IntervalMinutes} minutes.")]
    internal static partial void UpdatedSolarOptimizationScheduleConfiguration(ILogger logger, int intervalMinutes);

    [LoggerMessage(EventId = 1027, Level = LogLevel.Information, Message = "MoveToOptimum completed for installation site {InstallationSiteId} across {SolarPanelCount} solar panels.")]
    internal static partial void InstallationSiteMoveToOptimumCompleted(ILogger logger, int installationSiteId, int solarPanelCount);

    [LoggerMessage(EventId = 1028, Level = LogLevel.Warning, Message = "MoveToOptimum failed for installation site {InstallationSiteId}. {Code}: {Message}")]
    internal static partial void InstallationSiteMoveToOptimumFailed(
        ILogger logger,
        int installationSiteId,
        string code,
        string message);
}
