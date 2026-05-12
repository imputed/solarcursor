namespace SolarTracker.Application.Analysis.LinearMotor;

public sealed record LinearMotorAllNode(IReadOnlyList<LinearMotorAnalysisNode> Items)
    : LinearMotorAnalysisNode;
