using System.Globalization;

namespace SolarTracker.Api.Errors;

internal static class ApiProblemCatalog
{
    internal const string InstallationSiteEntityName = "installation site";
    internal const string SolarPanelEntityName = "solar panel";
    internal const string LinearMotorEntityName = "linear motor";
    internal const string CurrentMeasuringUnitEntityName = "current measuring unit";
    internal const string TiltMeasuringUnitEntityName = "tilt measuring unit";

    private const string ValidationKeyId = "id";
    private const string RouteIdMustEqualBodyIdMessage = "Route id must equal body Id.";
    private const string ServerErrorTitle = "Server error";
    private const string ServerErrorType = "https://httpstatuses.com/500";
    private const string InstallationSitePersistenceFailedTitle = "Installation site persistence failed";
    private const string InstallationSitePersistenceFailedDetailTemplate =
        "Installation site {0} could not be loaded after creation.";
    private const string SolarPanelPersistenceFailedTitle = "Solar panel persistence failed";
    private const string SolarPanelPersistenceFailedDetailTemplate =
        "Solar panel {0} could not be loaded after creation.";
    private const string LinearMotorPersistenceFailedTitle = "Linear motor persistence failed";
    private const string LinearMotorPersistenceFailedDetailTemplate =
        "Linear motor {0} could not be loaded after creation.";
    private const string CurrentMeasuringUnitPersistenceFailedTitle = "Current measuring unit persistence failed";
    private const string CurrentMeasuringUnitPersistenceFailedDetailTemplate =
        "Current measuring unit {0} could not be loaded after creation.";
    private const string TiltMeasuringUnitPersistenceFailedTitle = "Tilt measuring unit persistence failed";
    private const string TiltMeasuringUnitPersistenceFailedDetailTemplate =
        "Tilt measuring unit {0} could not be loaded after creation.";
    private const string InstallationSiteMovementFailedTitle = "Installation site movement failed";

    internal static Dictionary<string, string[]> RouteIdMustEqualBodyId() =>
        new()
        {
            [ValidationKeyId] = [RouteIdMustEqualBodyIdMessage],
        };

    internal static string ServerErrorTitleText() => ServerErrorTitle;

    internal static string ServerErrorTypeUri() => ServerErrorType;

    internal static (string Title, string Detail) InstallationSitePersistenceFailed(int installationSiteId) =>
        (
            InstallationSitePersistenceFailedTitle,
            string.Format(CultureInfo.InvariantCulture, InstallationSitePersistenceFailedDetailTemplate, installationSiteId)
        );

    internal static (string Title, string Detail) SolarPanelPersistenceFailed(int solarPanelId) =>
        (
            SolarPanelPersistenceFailedTitle,
            string.Format(CultureInfo.InvariantCulture, SolarPanelPersistenceFailedDetailTemplate, solarPanelId)
        );

    internal static (string Title, string Detail) LinearMotorPersistenceFailed(int linearMotorId) =>
        (
            LinearMotorPersistenceFailedTitle,
            string.Format(CultureInfo.InvariantCulture, LinearMotorPersistenceFailedDetailTemplate, linearMotorId)
        );

    internal static (string Title, string Detail) CurrentMeasuringUnitPersistenceFailed(int currentMeasuringUnitId) =>
        (
            CurrentMeasuringUnitPersistenceFailedTitle,
            string.Format(
                CultureInfo.InvariantCulture,
                CurrentMeasuringUnitPersistenceFailedDetailTemplate,
                currentMeasuringUnitId)
        );

    internal static (string Title, string Detail) TiltMeasuringUnitPersistenceFailed(int tiltMeasuringUnitId) =>
        (
            TiltMeasuringUnitPersistenceFailedTitle,
            string.Format(CultureInfo.InvariantCulture, TiltMeasuringUnitPersistenceFailedDetailTemplate, tiltMeasuringUnitId)
        );

    internal static (string Title, string Detail) InstallationSiteMovementFailed(string detail) =>
        (InstallationSiteMovementFailedTitle, detail);
}
