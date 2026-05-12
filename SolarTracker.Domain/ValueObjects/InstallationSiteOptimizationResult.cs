namespace SolarTracker.Domain.ValueObjects;

public readonly record struct InstallationSiteOptimizationResult
{
    public InstallationSiteOptimizationResultStatus Status { get; init; }

    public int? FailedSolarPanelId { get; init; }

    public SolarPanelMovementValidationResult? ValidationResult { get; init; }

    public SolarPanelMoveResult? MoveResult { get; init; }

    public bool IsSuccess => Status == InstallationSiteOptimizationResultStatus.Success;

    public static InstallationSiteOptimizationResult Success() =>
        new()
        {
            Status = InstallationSiteOptimizationResultStatus.Success,
        };

    public static InstallationSiteOptimizationResult SolarPanelValidationFailed(
        int failedSolarPanelId,
        SolarPanelMovementValidationResult validationResult) =>
        new()
        {
            Status = InstallationSiteOptimizationResultStatus.SolarPanelValidationFailed,
            FailedSolarPanelId = failedSolarPanelId,
            ValidationResult = validationResult,
        };

    public static InstallationSiteOptimizationResult SolarPanelMovementFailed(
        int failedSolarPanelId,
        SolarPanelMoveResult moveResult) =>
        new()
        {
            Status = InstallationSiteOptimizationResultStatus.SolarPanelMovementFailed,
            FailedSolarPanelId = failedSolarPanelId,
            MoveResult = moveResult,
        };
}
