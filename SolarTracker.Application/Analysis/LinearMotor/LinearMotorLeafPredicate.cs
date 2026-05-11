namespace SolarTracker.Application.Analysis;

public sealed record LinearMotorLeafPredicate(
    LinearMotorAnalyzeField Field,
    ScalarComparisonOperator Operator,
    int? IntValue,
    string? TextValue) : LinearMotorAnalysisNode;
