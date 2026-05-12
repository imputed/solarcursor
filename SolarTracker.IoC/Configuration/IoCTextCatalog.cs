namespace SolarTracker.IoC.Configuration;

internal static class IoCTextCatalog
{
    private const string MissingSolarTrackerConnectionStringMessage =
        "Connection string 'SolarTracker' must be configured before adding infrastructure services.";

    internal static string MissingSolarTrackerConnectionString() => MissingSolarTrackerConnectionStringMessage;
}
