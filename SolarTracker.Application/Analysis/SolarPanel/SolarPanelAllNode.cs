namespace SolarTracker.Application.Analysis;

public sealed record SolarPanelAllNode(IReadOnlyList<SolarPanelAnalysisNode> Items)
    : SolarPanelAnalysisNode;
