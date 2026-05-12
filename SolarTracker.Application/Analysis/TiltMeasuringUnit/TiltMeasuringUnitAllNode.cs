namespace SolarTracker.Application.Analysis.TiltMeasuringUnit;

public sealed record TiltMeasuringUnitAllNode(IReadOnlyList<TiltMeasuringUnitAnalysisNode> Items)
    : TiltMeasuringUnitAnalysisNode;
