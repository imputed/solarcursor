namespace SolarTracker.Application.Analysis;

public sealed record CurrentMeasuringUnitAllNode(IReadOnlyList<CurrentMeasuringUnitAnalysisNode> Items)
    : CurrentMeasuringUnitAnalysisNode;
