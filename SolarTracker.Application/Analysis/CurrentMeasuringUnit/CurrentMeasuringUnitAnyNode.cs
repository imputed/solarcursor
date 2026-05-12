namespace SolarTracker.Application.Analysis.CurrentMeasuringUnit;

public sealed record CurrentMeasuringUnitAnyNode(IReadOnlyList<CurrentMeasuringUnitAnalysisNode> Items)
    : CurrentMeasuringUnitAnalysisNode;
