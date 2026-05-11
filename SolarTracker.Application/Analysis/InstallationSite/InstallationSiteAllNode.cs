namespace SolarTracker.Application.Analysis;

public sealed record InstallationSiteAllNode(IReadOnlyList<InstallationSiteAnalysisNode> Items)
    : InstallationSiteAnalysisNode;
