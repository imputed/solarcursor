using System.Text.Json.Serialization;

namespace SolarTracker.Application.Analysis.TiltMeasuringUnit;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "kind")]
[JsonDerivedType(typeof(TiltMeasuringUnitLeafPredicate), "predicate")]
[JsonDerivedType(typeof(TiltMeasuringUnitAllNode), "all")]
[JsonDerivedType(typeof(TiltMeasuringUnitAnyNode), "any")]
public abstract record TiltMeasuringUnitAnalysisNode;
