namespace SolarTracker.Application.Analysis.LinearMotor;

public sealed record LinearMotorAnyNode(IReadOnlyList<LinearMotorAnalysisNode> Items)
    : LinearMotorAnalysisNode;
