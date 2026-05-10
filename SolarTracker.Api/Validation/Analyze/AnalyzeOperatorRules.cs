using SolarTracker.Application.Analysis;

namespace SolarTracker.Api.Validation.Analyze;

internal static class AnalyzeOperatorRules
{
    internal static bool AllowsIntOperands(ScalarComparisonOperator op) =>
        op is ScalarComparisonOperator.Equal
            or ScalarComparisonOperator.NotEqual
            or ScalarComparisonOperator.GreaterThan
            or ScalarComparisonOperator.GreaterThanOrEqual
            or ScalarComparisonOperator.LessThan
            or ScalarComparisonOperator.LessThanOrEqual;

    internal static bool AllowsStringOperands(ScalarComparisonOperator op) =>
        op switch
        {
            ScalarComparisonOperator.Contains or ScalarComparisonOperator.StartsWith => true,
            ScalarComparisonOperator.Equal or ScalarComparisonOperator.NotEqual => true,
            ScalarComparisonOperator.GreaterThan or ScalarComparisonOperator.GreaterThanOrEqual => true,
            ScalarComparisonOperator.LessThan or ScalarComparisonOperator.LessThanOrEqual => true,
            _ => false,
        };
}
