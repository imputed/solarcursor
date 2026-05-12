using Microsoft.Extensions.Logging;

namespace SolarTracker.Api.Logging;

internal static partial class ApiLog
{
    [LoggerMessage(EventId = 3000, Level = LogLevel.Error, Message = "Unhandled exception.")]
    internal static partial void UnhandledException(ILogger logger, Exception exception);

    [LoggerMessage(EventId = 3001, Level = LogLevel.Error, Message = "Create for {EntityName} returned id {EntityId}, but the entity could not be loaded afterwards.")]
    internal static partial void CreatePersistenceReadFailed(ILogger logger, string entityName, int entityId);

    [LoggerMessage(EventId = 3003, Level = LogLevel.Warning, Message = "MoveToOptimum returned a conflict for installation site {InstallationSiteId}. {Code}: {Message}")]
    internal static partial void InstallationSiteMoveToOptimumConflict(
        ILogger logger,
        int installationSiteId,
        string code,
        string message);
}
