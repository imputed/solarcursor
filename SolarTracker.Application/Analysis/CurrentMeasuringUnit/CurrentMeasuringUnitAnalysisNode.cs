using System.Text.Json.Serialization;

namespace SolarTracker.Application.Analysis;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "kind")]
[JsonDerivedType(typeof(CurrentMeasuringUnitLeafPredicate), "predicate")]
[JsonDerivedType(typeof(CurrentMeasuringUnitAllNode), "all")]
[JsonDerivedType(typeof(CurrentMeasuringUnitAnyNode), "any")]
public abstract record CurrentMeasuringUnitAnalysisNode;
