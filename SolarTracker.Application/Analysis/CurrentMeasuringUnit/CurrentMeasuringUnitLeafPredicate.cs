namespace SolarTracker.Application.Analysis;

public sealed record CurrentMeasuringUnitLeafPredicate(
    CurrentMeasuringUnitAnalyzeField Field,
    ScalarComparisonOperator Operator,
    int? IntValue,
    string? TextValue) : CurrentMeasuringUnitAnalysisNode;
