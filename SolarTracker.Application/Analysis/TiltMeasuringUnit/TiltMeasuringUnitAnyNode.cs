namespace SolarTracker.Application.Analysis;

public sealed record TiltMeasuringUnitAnyNode(IReadOnlyList<TiltMeasuringUnitAnalysisNode> Items)
    : TiltMeasuringUnitAnalysisNode;
