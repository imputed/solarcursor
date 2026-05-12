namespace SolarTracker.Application.Analysis.SolarPanel;

public sealed record SolarPanelAllNode(IReadOnlyList<SolarPanelAnalysisNode> Items)
    : SolarPanelAnalysisNode;
