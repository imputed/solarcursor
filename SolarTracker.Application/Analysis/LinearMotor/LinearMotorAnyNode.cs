namespace SolarTracker.Application.Analysis;

public sealed record LinearMotorAnyNode(IReadOnlyList<LinearMotorAnalysisNode> Items)
    : LinearMotorAnalysisNode;
