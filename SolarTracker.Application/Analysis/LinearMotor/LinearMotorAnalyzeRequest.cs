namespace SolarTracker.Application.Analysis.LinearMotor;

public sealed record LinearMotorAnalyzeRequest(
    LinearMotorAnalysisNode? Filter,
    LinearMotorAnalyzeField? SortBy,
    bool SortDescending = false,
    int Skip = 0,
    int Take = 50);
