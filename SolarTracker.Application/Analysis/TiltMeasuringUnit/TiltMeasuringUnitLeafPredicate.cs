namespace SolarTracker.Application.Analysis;

public sealed record TiltMeasuringUnitLeafPredicate(
    TiltMeasuringUnitAnalyzeField Field,
    ScalarComparisonOperator Operator,
    int? IntValue,
    string? TextValue) : TiltMeasuringUnitAnalysisNode;
