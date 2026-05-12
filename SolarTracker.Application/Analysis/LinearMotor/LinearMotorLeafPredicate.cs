using SolarTracker.Application.Analysis.Common;

namespace SolarTracker.Application.Analysis.LinearMotor;

public sealed record LinearMotorLeafPredicate(
    LinearMotorAnalyzeField Field,
    ScalarComparisonOperator Operator,
    int? IntValue,
    string? TextValue) : LinearMotorAnalysisNode;
