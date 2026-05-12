namespace SolarTracker.Application.Analysis.InstallationSite;

public sealed record InstallationSiteAnyNode(IReadOnlyList<InstallationSiteAnalysisNode> Items)
    : InstallationSiteAnalysisNode;
