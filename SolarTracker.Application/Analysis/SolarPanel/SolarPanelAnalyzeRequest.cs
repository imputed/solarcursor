namespace SolarTracker.Application.Analysis.SolarPanel;

public sealed record SolarPanelAnalyzeRequest(
    SolarPanelAnalysisNode? Filter,
    SolarPanelAnalyzeField? SortBy,
    bool SortDescending = false,
    int Skip = 0,
    int Take = 50);
