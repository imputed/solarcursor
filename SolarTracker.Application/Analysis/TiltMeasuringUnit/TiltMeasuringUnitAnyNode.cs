namespace SolarTracker.Application.Analysis.TiltMeasuringUnit;

public sealed record TiltMeasuringUnitAnyNode(IReadOnlyList<TiltMeasuringUnitAnalysisNode> Items)
    : TiltMeasuringUnitAnalysisNode;
