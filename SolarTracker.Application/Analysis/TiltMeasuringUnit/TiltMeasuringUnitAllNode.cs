namespace SolarTracker.Application.Analysis;

public sealed record TiltMeasuringUnitAllNode(IReadOnlyList<TiltMeasuringUnitAnalysisNode> Items)
    : TiltMeasuringUnitAnalysisNode;
