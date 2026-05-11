namespace SolarTracker.Application.Analysis;

public sealed record LinearMotorAllNode(IReadOnlyList<LinearMotorAnalysisNode> Items)
    : LinearMotorAnalysisNode;
