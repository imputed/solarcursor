using System.Globalization;
using SolarTracker.Application.Results;

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
        private const string MovementRecoveryFailedCode = "solar-panel-movement-recovery-failed";
        private const string MovementRecoveryFailedTemplate =
            "Solar panel {0} failed to recover after motor {1} failed. Original error: {2}: {3}. Recovery error: {4}: {5}";
        private const string MovementStepRevertedCode = "solar-panel-movement-step-reverted";
        private const string MovementStepRevertedTemplate =
            "Solar panel {0} movement failed at linear motor {1}, and the already completed motor movements were reverted. Original error: {2}: {3}";

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

        public static ResultError MovementRecoveryFailed(
            int solarPanelId,
            int failedLinearMotorId,
            ResultError moveError,
            ResultError recoveryError) =>
            new(
                MovementRecoveryFailedCode,
                string.Format(
                    CultureInfo.InvariantCulture,
                    MovementRecoveryFailedTemplate,
                    solarPanelId,
                    failedLinearMotorId,
                    moveError.Code,
                    moveError.Message,
                    recoveryError.Code,
                    recoveryError.Message));

        public static ResultError MovementStepReverted(
            int solarPanelId,
            int failedLinearMotorId,
            ResultError moveError) =>
            new(
                MovementStepRevertedCode,
                string.Format(
                    CultureInfo.InvariantCulture,
                    MovementStepRevertedTemplate,
                    solarPanelId,
                    failedLinearMotorId,
                    moveError.Code,
                    moveError.Message));
    }

    public static class InstallationSite
    {
        private const string NotFoundCode = "installation-site-not-found";
        private const string NotFoundTemplate = "Installation site {0} was not found.";

        public static ResultError NotFound(int installationSiteId) =>
            new(NotFoundCode, string.Format(CultureInfo.InvariantCulture, NotFoundTemplate, installationSiteId));
    }

    public static class LinearMotor
    {
        private const string NotFoundCode = "linear-motor-not-found";
        private const string NotFoundTemplate = "Linear motor {0} was not found.";
        private const string RecoveryFailedCode = "linear-motor-recovery-failed";
        private const string RecoveryFailedTemplate = "Linear motor {0} could not be recovered. {1}: {2}";

        public static ResultError NotFound(int linearMotorId) =>
            new(NotFoundCode, string.Format(CultureInfo.InvariantCulture, NotFoundTemplate, linearMotorId));

        public static ResultError RecoveryFailed(int linearMotorId, ResultError recoveryError) =>
            new(
                RecoveryFailedCode,
                string.Format(
                    CultureInfo.InvariantCulture,
                    RecoveryFailedTemplate,
                    linearMotorId,
                    recoveryError.Code,
                    recoveryError.Message));
    }
}
