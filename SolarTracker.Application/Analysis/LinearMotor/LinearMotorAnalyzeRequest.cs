namespace SolarTracker.Application.Analysis;

public sealed record LinearMotorAnalyzeRequest(
    LinearMotorAnalysisNode? Filter,
    LinearMotorAnalyzeField? SortBy,
    bool SortDescending = false,
    int Skip = 0,
    int Take = 50);
