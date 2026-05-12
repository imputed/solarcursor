using System.Text.Json.Serialization;

namespace SolarTracker.Application.Analysis.LinearMotor;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "kind")]
[JsonDerivedType(typeof(LinearMotorLeafPredicate), "predicate")]
[JsonDerivedType(typeof(LinearMotorAllNode), "all")]
[JsonDerivedType(typeof(LinearMotorAnyNode), "any")]
public abstract record LinearMotorAnalysisNode;
