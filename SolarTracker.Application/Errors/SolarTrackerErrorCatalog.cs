using System.Globalization;
using SolarTracker.Application.Results;
using SolarTracker.Domain.ValueObjects;

namespace SolarTracker.Application.Errors;

public static class SolarTrackerErrorCatalog
{
    public static class SolarPanel
    {
        private const string NotFoundCode = "solar-panel-not-found";
        private const string NotFoundTemplate = "Solar panel {0} was not found.";
        private const string TiltMeasuringUnitMissingCode = "tilt-measuring-unit-missing";
        private const string TiltMeasuringUnitMissingTemplate = "Solar panel {0} does not have a tilt measuring unit.";
        private const string LinearMotorsMissingCode = "linear-motors-missing";
        private const string LinearMotorsMissingTemplate = "Solar panel {0} does not have any linear motors.";
        private const string ThresholdNotMetCode = "solar-panel-threshold-not-met";
        private const string ThresholdNotMetTemplate =
            "Solar panel {0} did not reach the configured threshold in the allowed number of steps.";
        private const string MovementFailedCode = "solar-panel-movement-failed";
        private const string MovementFailedTemplate =
            "Solar panel {0} movement failed at linear motor {1}. {2}";
        private const string MovementRecoveryFailedCode = "solar-panel-movement-recovery-failed";
        private const string MovementRecoveryFailedTemplate =
            "Solar panel {0} failed to recover after motor {1} failed. Original error: {2}. Recovery error: {3}";
        private const string MovementStepRevertedCode = "solar-panel-movement-step-reverted";
        private const string MovementStepRevertedTemplate =
            "Solar panel {0} movement failed at linear motor {1}, and the already completed motor movements were reverted. Original error: {2}";
        private const string UnsupportedMovementValidationResultTemplate =
            "Unsupported solar panel movement validation result '{0}'.";
        private const string UnsupportedMoveResultStatusTemplate =
            "Unsupported solar panel move result status '{0}'.";

        public static ResultError NotFound(int solarPanelId) =>
            new(NotFoundCode, string.Format(CultureInfo.InvariantCulture, NotFoundTemplate, solarPanelId));

        public static ResultError TiltMeasuringUnitMissing(int solarPanelId) =>
            new(
                TiltMeasuringUnitMissingCode,
                string.Format(CultureInfo.InvariantCulture, TiltMeasuringUnitMissingTemplate, solarPanelId));

        public static ResultError LinearMotorsMissing(int solarPanelId) =>
            new(
                LinearMotorsMissingCode,
                string.Format(CultureInfo.InvariantCulture, LinearMotorsMissingTemplate, solarPanelId));

        public static ResultError ThresholdNotMet(int solarPanelId) =>
            new(ThresholdNotMetCode, string.Format(CultureInfo.InvariantCulture, ThresholdNotMetTemplate, solarPanelId));

        public static ResultError MovementFailed(int solarPanelId, int failedLinearMotorId, string failureMessage) =>
            new(
                MovementFailedCode,
                string.Format(
                    CultureInfo.InvariantCulture,
                    MovementFailedTemplate,
                    solarPanelId,
                    failedLinearMotorId,
                    failureMessage));

        public static ResultError MovementRecoveryFailed(
            int solarPanelId,
            int failedLinearMotorId,
            string moveFailureMessage,
            string recoveryFailureMessage) =>
            new(
                MovementRecoveryFailedCode,
                string.Format(
                    CultureInfo.InvariantCulture,
                    MovementRecoveryFailedTemplate,
                    solarPanelId,
                    failedLinearMotorId,
                    moveFailureMessage,
                    recoveryFailureMessage));

        public static ResultError MovementStepReverted(
            int solarPanelId,
            int failedLinearMotorId,
            string moveFailureMessage) =>
            new(
                MovementStepRevertedCode,
                string.Format(
                    CultureInfo.InvariantCulture,
                    MovementStepRevertedTemplate,
                    solarPanelId,
                    failedLinearMotorId,
                    moveFailureMessage));

        public static ResultError MovementValidationFailure(
            int solarPanelId,
            SolarPanelMovementValidationResult validationResult) =>
            validationResult switch
            {
                SolarPanelMovementValidationResult.TiltMeasuringUnitMissing =>
                    TiltMeasuringUnitMissing(solarPanelId),
                SolarPanelMovementValidationResult.LinearMotorsMissing =>
                    LinearMotorsMissing(solarPanelId),
                _ => throw UnsupportedMovementValidationResult(validationResult),
            };

        public static ResultError MoveFailure(int solarPanelId, SolarPanelMoveResult moveResult) =>
            moveResult.Status switch
            {
                SolarPanelMoveResultStatus.ThresholdNotMet =>
                    ThresholdNotMet(solarPanelId),
                SolarPanelMoveResultStatus.MovementFailed =>
                    MovementFailed(
                        solarPanelId,
                        moveResult.FailedLinearMotorId!.Value,
                        moveResult.FailureMessage!),
                SolarPanelMoveResultStatus.MovementStepReverted =>
                    MovementStepReverted(
                        solarPanelId,
                        moveResult.FailedLinearMotorId!.Value,
                        moveResult.FailureMessage!),
                SolarPanelMoveResultStatus.MovementRecoveryFailed =>
                    MovementRecoveryFailed(
                        solarPanelId,
                        moveResult.FailedLinearMotorId!.Value,
                        moveResult.FailureMessage!,
                        moveResult.RecoveryFailureMessage!),
                _ => throw UnsupportedMoveResultStatus(moveResult.Status),
            };

        private static InvalidOperationException UnsupportedMovementValidationResult(
            SolarPanelMovementValidationResult validationResult) =>
            new(
                string.Format(
                    CultureInfo.InvariantCulture,
                    UnsupportedMovementValidationResultTemplate,
                    validationResult));

        private static InvalidOperationException UnsupportedMoveResultStatus(
            SolarPanelMoveResultStatus status) =>
            new(
                string.Format(
                    CultureInfo.InvariantCulture,
                    UnsupportedMoveResultStatusTemplate,
                    status));
    }

    public static class InstallationSite
    {
        private const string NotFoundCode = "installation-site-not-found";
        private const string NotFoundTemplate = "Installation site {0} was not found.";
        private const string UnsupportedOptimizationResultStatusTemplate =
            "Unsupported installation site optimization result status '{0}'.";

        public static ResultError NotFound(int installationSiteId) =>
            new(NotFoundCode, string.Format(CultureInfo.InvariantCulture, NotFoundTemplate, installationSiteId));

        public static ResultError OptimizationFailure(InstallationSiteOptimizationResult optimizationResult)
        {
            int solarPanelId = optimizationResult.FailedSolarPanelId!.Value;
            return optimizationResult.Status switch
            {
                InstallationSiteOptimizationResultStatus.SolarPanelValidationFailed =>
                    SolarPanel.MovementValidationFailure(
                        solarPanelId,
                        optimizationResult.ValidationResult!.Value),
                InstallationSiteOptimizationResultStatus.SolarPanelMovementFailed =>
                    SolarPanel.MoveFailure(
                        solarPanelId,
                        optimizationResult.MoveResult!.Value),
                _ => throw UnsupportedOptimizationResultStatus(optimizationResult.Status),
            };
        }

        private static InvalidOperationException UnsupportedOptimizationResultStatus(
            InstallationSiteOptimizationResultStatus status) =>
            new(
                string.Format(
                    CultureInfo.InvariantCulture,
                    UnsupportedOptimizationResultStatusTemplate,
                    status));
    }
}
