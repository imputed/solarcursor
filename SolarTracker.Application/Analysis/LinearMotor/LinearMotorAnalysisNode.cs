using System.Text.Json.Serialization;

namespace SolarTracker.Application.Analysis;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "kind")]
[JsonDerivedType(typeof(LinearMotorLeafPredicate), "predicate")]
[JsonDerivedType(typeof(LinearMotorAllNode), "all")]
[JsonDerivedType(typeof(LinearMotorAnyNode), "any")]
public abstract record LinearMotorAnalysisNode;
