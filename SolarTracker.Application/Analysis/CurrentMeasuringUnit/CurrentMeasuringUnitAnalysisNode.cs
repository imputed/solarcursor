using System.Text.Json.Serialization;

namespace SolarTracker.Application.Analysis.CurrentMeasuringUnit;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "kind")]
[JsonDerivedType(typeof(CurrentMeasuringUnitLeafPredicate), "predicate")]
[JsonDerivedType(typeof(CurrentMeasuringUnitAllNode), "all")]
[JsonDerivedType(typeof(CurrentMeasuringUnitAnyNode), "any")]
public abstract record CurrentMeasuringUnitAnalysisNode;
