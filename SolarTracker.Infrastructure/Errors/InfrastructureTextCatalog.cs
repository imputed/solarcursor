using System.Globalization;
using SolarTracker.Application.Analysis.Common;

namespace SolarTracker.Infrastructure.Errors;

internal static class InfrastructureTextCatalog
{
    private const string PwmPeriodMustBeGreaterThanZeroMessage = "PWM period must be greater than zero.";
    private const string UnsupportedIntegerOperatorTemplate = "Operator {0} is not supported for integer fields.";
    private const string UnsupportedStringOperatorTemplate = "Operator {0} is not supported for string fields.";

    internal static string PwmPeriodMustBeGreaterThanZero() => PwmPeriodMustBeGreaterThanZeroMessage;

    internal static string UnsupportedIntegerOperator(ScalarComparisonOperator op) =>
        string.Format(CultureInfo.InvariantCulture, UnsupportedIntegerOperatorTemplate, op);

    internal static string UnsupportedStringOperator(ScalarComparisonOperator op) =>
        string.Format(CultureInfo.InvariantCulture, UnsupportedStringOperatorTemplate, op);
}
