using System.Globalization;

namespace SolarTracker.Domain.Errors;

public static class DomainTextCatalog
{
    public static class SolarPanel
    {
        private const string PositionRequiresTiltMeasuringUnitMessage =
            "Solar panel position requires a tilt measuring unit.";
        private const string MovementRequiresLinearMotorsMessage =
            "Solar panel movement requires at least one linear motor.";
        private const string UnsupportedMovementValidationResultTemplate =
            "Unsupported solar panel movement validation result '{0}'.";

        public static string PositionRequiresTiltMeasuringUnit() => PositionRequiresTiltMeasuringUnitMessage;

        public static string MovementRequiresLinearMotors() => MovementRequiresLinearMotorsMessage;

        public static string UnsupportedMovementValidationResult(
            SolarTracker.Domain.ValueObjects.SolarPanelMovementValidationResult validationResult) =>
            string.Format(
                CultureInfo.InvariantCulture,
                UnsupportedMovementValidationResultTemplate,
                validationResult);
    }

    public static class InstallationSite
    {
        private const string PositionRequiresSolarPanelsMessage =
            "Installation site position requires at least one solar panel.";
        private const string OptimizationRequiresSolarTrackingConfigurationTemplate =
            "Installation site optimization requires a solar tracking configuration for solar panel {0}.";

        public static string PositionRequiresSolarPanels() => PositionRequiresSolarPanelsMessage;

        public static string OptimizationRequiresSolarTrackingConfiguration(int solarPanelId) =>
            string.Format(
                CultureInfo.InvariantCulture,
                OptimizationRequiresSolarTrackingConfigurationTemplate,
                solarPanelId);
    }
}
