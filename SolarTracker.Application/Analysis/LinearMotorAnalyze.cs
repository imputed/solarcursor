using System.Text.Json.Serialization;

namespace SolarTracker.Application.Analysis;

public enum LinearMotorAnalyzeField
{
    Id = 0,
    InstallationSiteId = 1,
    Name = 2,
}

[JsonPolymorphic(TypeDiscriminatorPropertyName = "kind")]
[JsonDerivedType(typeof(LinearMotorLeafPredicate), "predicate")]
[JsonDerivedType(typeof(LinearMotorAllNode), "all")]
[JsonDerivedType(typeof(LinearMotorAnyNode), "any")]
public abstract record LinearMotorAnalysisNode;

public sealed record LinearMotorLeafPredicate(
    LinearMotorAnalyzeField Field,
    ScalarComparisonOperator Operator,
    int? IntValue,
    string? TextValue) : LinearMotorAnalysisNode;

public sealed record LinearMotorAllNode(IReadOnlyList<LinearMotorAnalysisNode> Items)
    : LinearMotorAnalysisNode;

public sealed record LinearMotorAnyNode(IReadOnlyList<LinearMotorAnalysisNode> Items)
    : LinearMotorAnalysisNode;

public sealed record LinearMotorAnalyzeRequest(
    LinearMotorAnalysisNode? Filter,
    LinearMotorAnalyzeField? SortBy,
    bool SortDescending = false,
    int Skip = 0,
    int Take = 50);
