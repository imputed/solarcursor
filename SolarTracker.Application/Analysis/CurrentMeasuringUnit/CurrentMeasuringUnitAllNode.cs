namespace SolarTracker.Application.Analysis.CurrentMeasuringUnit;

public sealed record CurrentMeasuringUnitAllNode(IReadOnlyList<CurrentMeasuringUnitAnalysisNode> Items)
    : CurrentMeasuringUnitAnalysisNode;
