namespace SolarTracker.Application.Analysis.SolarPanel;

public sealed record SolarPanelAnyNode(IReadOnlyList<SolarPanelAnalysisNode> Items)
    : SolarPanelAnalysisNode;
