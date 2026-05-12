using Microsoft.Extensions.Logging;

namespace SolarTracker.Api.Logging;

internal static partial class ApiLog
{
    [LoggerMessage(EventId = 3000, Level = LogLevel.Error, Message = "Unhandled exception.")]
    internal static partial void UnhandledException(ILogger logger, Exception exception);

    [LoggerMessage(EventId = 3001, Level = LogLevel.Error, Message = "Create for {EntityName} returned id {EntityId}, but the entity could not be loaded afterwards.")]
    internal static partial void CreatePersistenceReadFailed(ILogger logger, string entityName, int entityId);

    [LoggerMessage(EventId = 3002, Level = LogLevel.Warning, Message = "MoveToOptimum returned a conflict for solar panel {SolarPanelId}. {Code}: {Message}")]
    internal static partial void SolarPanelMoveToOptimumConflict(
        ILogger logger,
        int solarPanelId,
        string code,
        string message);
}
