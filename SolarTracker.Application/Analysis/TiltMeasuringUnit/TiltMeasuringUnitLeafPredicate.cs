using SolarTracker.Application.Analysis.Common;

namespace SolarTracker.Application.Analysis.TiltMeasuringUnit;

public sealed record TiltMeasuringUnitLeafPredicate(
    TiltMeasuringUnitAnalyzeField Field,
    ScalarComparisonOperator Operator,
    int? IntValue,
    string? TextValue) : TiltMeasuringUnitAnalysisNode;
