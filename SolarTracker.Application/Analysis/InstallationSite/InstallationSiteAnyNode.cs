namespace SolarTracker.Application.Analysis;

public sealed record InstallationSiteAnyNode(IReadOnlyList<InstallationSiteAnalysisNode> Items)
    : InstallationSiteAnalysisNode;
