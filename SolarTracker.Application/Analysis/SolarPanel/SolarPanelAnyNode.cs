namespace SolarTracker.Application.Analysis;

public sealed record SolarPanelAnyNode(IReadOnlyList<SolarPanelAnalysisNode> Items)
    : SolarPanelAnalysisNode;
