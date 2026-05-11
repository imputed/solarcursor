using System.Text.Json.Serialization;

namespace SolarTracker.Application.Analysis;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "kind")]
[JsonDerivedType(typeof(TiltMeasuringUnitLeafPredicate), "predicate")]
[JsonDerivedType(typeof(TiltMeasuringUnitAllNode), "all")]
[JsonDerivedType(typeof(TiltMeasuringUnitAnyNode), "any")]
public abstract record TiltMeasuringUnitAnalysisNode;
