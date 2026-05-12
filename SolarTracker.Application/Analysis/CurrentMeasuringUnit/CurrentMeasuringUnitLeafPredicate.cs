using SolarTracker.Application.Analysis.Common;

namespace SolarTracker.Application.Analysis.CurrentMeasuringUnit;

public sealed record CurrentMeasuringUnitLeafPredicate(
    CurrentMeasuringUnitAnalyzeField Field,
    ScalarComparisonOperator Operator,
    int? IntValue,
    string? TextValue) : CurrentMeasuringUnitAnalysisNode;
