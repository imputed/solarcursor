using SolarTracker.Application.Analysis.Common;

namespace SolarTracker.Application.Analysis.SolarPanel;

public sealed record SolarPanelLeafPredicate(
    SolarPanelAnalyzeField Field,
    ScalarComparisonOperator Operator,
    int? IntValue,
    string? TextValue) : SolarPanelAnalysisNode;
