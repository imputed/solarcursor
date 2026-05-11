namespace SolarTracker.Application.Analysis;

public sealed record SolarPanelLeafPredicate(
    SolarPanelAnalyzeField Field,
    ScalarComparisonOperator Operator,
    int? IntValue,
    string? TextValue) : SolarPanelAnalysisNode;
