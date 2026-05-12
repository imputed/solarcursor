namespace SolarTracker.Application.Analysis.CurrentMeasuringUnit;

public sealed record CurrentMeasuringUnitAnalyzeRequest(
    CurrentMeasuringUnitAnalysisNode? Filter,
    CurrentMeasuringUnitAnalyzeField? SortBy,
    bool SortDescending = false,
    int Skip = 0,
    int Take = 50);
