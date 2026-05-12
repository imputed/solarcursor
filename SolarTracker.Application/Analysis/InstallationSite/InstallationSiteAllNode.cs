namespace SolarTracker.Application.Analysis.InstallationSite;

public sealed record InstallationSiteAllNode(IReadOnlyList<InstallationSiteAnalysisNode> Items)
    : InstallationSiteAnalysisNode;
