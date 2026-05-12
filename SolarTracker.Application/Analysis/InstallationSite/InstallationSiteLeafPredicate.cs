using SolarTracker.Application.Analysis.Common;

namespace SolarTracker.Application.Analysis.InstallationSite;

public sealed record InstallationSiteLeafPredicate(
    InstallationSiteAnalyzeField Field,
    ScalarComparisonOperator Operator,
    int? IntValue,
    string? TextValue) : InstallationSiteAnalysisNode;
