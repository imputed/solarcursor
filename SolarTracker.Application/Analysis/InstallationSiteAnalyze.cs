using System.Text.Json.Serialization;

namespace SolarTracker.Application.Analysis;

public enum InstallationSiteAnalyzeField
{
    Id = 0,
    Name = 1,
}

[JsonPolymorphic(TypeDiscriminatorPropertyName = "kind")]
[JsonDerivedType(typeof(InstallationSiteLeafPredicate), "predicate")]
[JsonDerivedType(typeof(InstallationSiteAllNode), "all")]
[JsonDerivedType(typeof(InstallationSiteAnyNode), "any")]
public abstract record InstallationSiteAnalysisNode;

public sealed record InstallationSiteLeafPredicate(
    InstallationSiteAnalyzeField Field,
    ScalarComparisonOperator Operator,
    int? IntValue,
    string? TextValue) : InstallationSiteAnalysisNode;

public sealed record InstallationSiteAllNode(IReadOnlyList<InstallationSiteAnalysisNode> Items)
    : InstallationSiteAnalysisNode;

public sealed record InstallationSiteAnyNode(IReadOnlyList<InstallationSiteAnalysisNode> Items)
    : InstallationSiteAnalysisNode;

/// <summary>Scalar fields only — no filtering on navigations.</summary>
public sealed record InstallationSiteAnalyzeRequest(
    InstallationSiteAnalysisNode? Filter,
    InstallationSiteAnalyzeField? SortBy,
    bool SortDescending = false,
    int Skip = 0,
    int Take = 50);
