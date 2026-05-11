namespace SolarTracker.Application.Analysis;

public sealed record CurrentMeasuringUnitAnyNode(IReadOnlyList<CurrentMeasuringUnitAnalysisNode> Items)
    : CurrentMeasuringUnitAnalysisNode;
