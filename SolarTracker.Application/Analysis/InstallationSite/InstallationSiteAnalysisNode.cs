using System.Text.Json.Serialization;

namespace SolarTracker.Application.Analysis.InstallationSite;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "kind")]
[JsonDerivedType(typeof(InstallationSiteLeafPredicate), "predicate")]
[JsonDerivedType(typeof(InstallationSiteAllNode), "all")]
[JsonDerivedType(typeof(InstallationSiteAnyNode), "any")]
public abstract record InstallationSiteAnalysisNode;
