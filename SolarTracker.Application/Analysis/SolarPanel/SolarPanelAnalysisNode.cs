using System.Text.Json.Serialization;

namespace SolarTracker.Application.Analysis;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "kind")]
[JsonDerivedType(typeof(SolarPanelLeafPredicate), "predicate")]
[JsonDerivedType(typeof(SolarPanelAllNode), "all")]
[JsonDerivedType(typeof(SolarPanelAnyNode), "any")]
public abstract record SolarPanelAnalysisNode;
