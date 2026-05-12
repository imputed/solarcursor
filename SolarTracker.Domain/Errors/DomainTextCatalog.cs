namespace SolarTracker.Domain.Errors;

public static class DomainTextCatalog
{
    public static class SolarPanel
    {
        private const string PositionRequiresTiltMeasuringUnitMessage =
            "Solar panel position requires a tilt measuring unit.";

        public static string PositionRequiresTiltMeasuringUnit() => PositionRequiresTiltMeasuringUnitMessage;
    }

    public static class InstallationSite
    {
        private const string PositionRequiresSolarPanelsMessage =
            "Installation site position requires at least one solar panel.";

        public static string PositionRequiresSolarPanels() => PositionRequiresSolarPanelsMessage;
    }
}
