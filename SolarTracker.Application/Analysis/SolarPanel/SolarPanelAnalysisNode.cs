using System.Text.Json.Serialization;

namespace SolarTracker.Application.Analysis.SolarPanel;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "kind")]
[JsonDerivedType(typeof(SolarPanelLeafPredicate), "predicate")]
[JsonDerivedType(typeof(SolarPanelAllNode), "all")]
[JsonDerivedType(typeof(SolarPanelAnyNode), "any")]
public abstract record SolarPanelAnalysisNode;
