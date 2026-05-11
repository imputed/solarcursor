namespace SolarTracker.Application.Analysis;

public sealed record TiltMeasuringUnitAnalyzeRequest(
    TiltMeasuringUnitAnalysisNode? Filter,
    TiltMeasuringUnitAnalyzeField? SortBy,
    bool SortDescending = false,
    int Skip = 0,
    int Take = 50);
