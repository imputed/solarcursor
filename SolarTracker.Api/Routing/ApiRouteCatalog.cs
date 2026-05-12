using System.Globalization;

namespace SolarTracker.Api.Routing;

internal static class ApiRouteCatalog
{
    private const string InstallationSiteByIdTemplate = "/api/installation-sites/{0}";
    private const string SolarPanelByIdTemplate = "/api/solar-panels/{0}";
    private const string LinearMotorByIdTemplate = "/api/linear-motors/{0}";
    private const string CurrentMeasuringUnitByIdTemplate = "/api/current-measuring-units/{0}";
    private const string TiltMeasuringUnitByIdTemplate = "/api/tilt-measuring-units/{0}";

    internal static string InstallationSiteById(int installationSiteId) =>
        string.Format(CultureInfo.InvariantCulture, InstallationSiteByIdTemplate, installationSiteId);

    internal static string SolarPanelById(int solarPanelId) =>
        string.Format(CultureInfo.InvariantCulture, SolarPanelByIdTemplate, solarPanelId);

    internal static string LinearMotorById(int linearMotorId) =>
        string.Format(CultureInfo.InvariantCulture, LinearMotorByIdTemplate, linearMotorId);

    internal static string CurrentMeasuringUnitById(int currentMeasuringUnitId) =>
        string.Format(CultureInfo.InvariantCulture, CurrentMeasuringUnitByIdTemplate, currentMeasuringUnitId);

    internal static string TiltMeasuringUnitById(int tiltMeasuringUnitId) =>
        string.Format(CultureInfo.InvariantCulture, TiltMeasuringUnitByIdTemplate, tiltMeasuringUnitId);
}
