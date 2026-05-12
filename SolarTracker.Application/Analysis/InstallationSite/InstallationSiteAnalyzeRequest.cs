namespace SolarTracker.Application.Analysis.InstallationSite;

/// <summary>Scalar fields only — no filtering on navigations.</summary>
public sealed record InstallationSiteAnalyzeRequest(
    InstallationSiteAnalysisNode? Filter,
    InstallationSiteAnalyzeField? SortBy,
    bool SortDescending = false,
    int Skip = 0,
    int Take = 50);
