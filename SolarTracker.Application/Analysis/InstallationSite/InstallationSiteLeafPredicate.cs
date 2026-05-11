namespace SolarTracker.Application.Analysis;

public sealed record InstallationSiteLeafPredicate(
    InstallationSiteAnalyzeField Field,
    ScalarComparisonOperator Operator,
    int? IntValue,
    string? TextValue) : InstallationSiteAnalysisNode;
