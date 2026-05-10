using System.Text.Json.Serialization;

namespace SolarTracker.Application.Analysis;

public enum SolarPanelAnalyzeField
{
    Id = 0,
    InstallationSiteId = 1,
    SerialNumber = 2,
}

[JsonPolymorphic(TypeDiscriminatorPropertyName = "kind")]
[JsonDerivedType(typeof(SolarPanelLeafPredicate), "predicate")]
[JsonDerivedType(typeof(SolarPanelAllNode), "all")]
[JsonDerivedType(typeof(SolarPanelAnyNode), "any")]
public abstract record SolarPanelAnalysisNode;

public sealed record SolarPanelLeafPredicate(
    SolarPanelAnalyzeField Field,
    ScalarComparisonOperator Operator,
    int? IntValue,
    string? TextValue) : SolarPanelAnalysisNode;

public sealed record SolarPanelAllNode(IReadOnlyList<SolarPanelAnalysisNode> Items)
    : SolarPanelAnalysisNode;

public sealed record SolarPanelAnyNode(IReadOnlyList<SolarPanelAnalysisNode> Items)
    : SolarPanelAnalysisNode;

public sealed record SolarPanelAnalyzeRequest(
    SolarPanelAnalysisNode? Filter,
    SolarPanelAnalyzeField? SortBy,
    bool SortDescending = false,
    int Skip = 0,
    int Take = 50);
