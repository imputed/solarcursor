using FluentValidation;
using SolarTracker.Application.Analysis;
using SolarTracker.Api.Validation;

namespace SolarTracker.Api.Validation.Analyze;

public sealed class InstallationSiteAnalyzeRequestValidator : AbstractValidator<InstallationSiteAnalyzeRequest>
{
    private const int MaxDepth = 32;
    private const int MaxLeafCount = 100;

    public InstallationSiteAnalyzeRequestValidator()
    {
        RuleFor(r => r.Take).InclusiveBetween(1, 500);
        RuleFor(r => r.Skip).GreaterThanOrEqualTo(0);
        RuleFor(r => r.SortBy).Must(f => !f.HasValue || Enum.IsDefined(f.Value))
            .WithMessage(ValidationMessageCatalog.SortByMustBeDefinedWhitelistedField());
        RuleFor(r => r).Custom(ValidateGraph);
    }

    private static void ValidateGraph(
        InstallationSiteAnalyzeRequest request,
        ValidationContext<InstallationSiteAnalyzeRequest> context)
    {
        var leafCounter = new LeafCounter();

        ValidateNode(request.Filter, context, "filter", 0, leafCounter);

        if (leafCounter.Count > MaxLeafCount)
        {
            context.AddFailure(nameof(InstallationSiteAnalyzeRequest.Filter),
                ValidationMessageCatalog.MaximumPredicateLeavesSupported(MaxLeafCount));
        }
    }

    private static void ValidateNode(
        InstallationSiteAnalysisNode? node,
        ValidationContext<InstallationSiteAnalyzeRequest> context,
        string path,
        int depth,
        LeafCounter leafCounter)
    {
        if (node is null)
            return;

        if (depth > MaxDepth)
        {
            context.AddFailure(path,
                ValidationMessageCatalog.FilterNestingMustNotExceed(MaxDepth));
            return;
        }

        switch (node)
        {
            case InstallationSiteLeafPredicate leaf:
                ValidateLeaf(leaf, context, $"{path}.{leaf.Field}", leafCounter);
                break;

            case InstallationSiteAllNode all:
                ValidateItems(all.Items, context, path, $"{path}.items", depth, leafCounter, "All");
                break;

            case InstallationSiteAnyNode any:
                ValidateItems(any.Items, context, path, $"{path}.items", depth, leafCounter, "Any");
                break;
        }
    }

    private static void ValidateItems(
        IReadOnlyList<InstallationSiteAnalysisNode>? items,
        ValidationContext<InstallationSiteAnalyzeRequest> context,
        string path,
        string itemsPath,
        int depth,
        LeafCounter leafCounter,
        string combiner)
    {
        if (items is null)
        {
            context.AddFailure(itemsPath,
                ValidationMessageCatalog.CombinerItemsCannotBeNull(combiner));
            return;
        }

        int i = 0;
        foreach (InstallationSiteAnalysisNode item in items)
        {
            ValidateNode(item, context, $"{path}[{i}]", depth + 1, leafCounter);
            i++;
        }
    }

    private static void ValidateLeaf(
        InstallationSiteLeafPredicate leaf,
        ValidationContext<InstallationSiteAnalyzeRequest> context,
        string path,
        LeafCounter leafCounter)
    {
        leafCounter.Count++;

        if (!Enum.IsDefined(leaf.Field))
        {
            context.AddFailure(path, ValidationMessageCatalog.PredicateFieldMustBeDefinedOnWhitelist());
            return;
        }

        switch (leaf.Field)
        {
            case InstallationSiteAnalyzeField.Id:

                if (leaf.IntValue is null)
                {
                    context.AddFailure(path,
                        ValidationMessageCatalog.ValueRequiredWhenFilteringOn(
                            nameof(leaf.IntValue),
                            InstallationSiteAnalyzeField.Id));
                }

                if (leaf.TextValue is not null)
                {
                    context.AddFailure(path,
                        ValidationMessageCatalog.ValueMustNotBeSetForIntegerWhitelistFields(nameof(leaf.TextValue)));
                }

                if (!AnalyzeOperatorRules.AllowsIntOperands(leaf.Operator))
                {
                    context.AddFailure(nameof(leaf.Operator),
                        ValidationMessageCatalog.OperatorNotPermittedForField(
                            leaf.Operator,
                            InstallationSiteAnalyzeField.Id));
                }

                break;

            case InstallationSiteAnalyzeField.Name:

                if (leaf.TextValue is null)
                {
                    context.AddFailure(path,
                        ValidationMessageCatalog.ValueRequiredWhenFilteringOn(
                            nameof(leaf.TextValue),
                            InstallationSiteAnalyzeField.Name));
                }

                if (leaf.IntValue is not null)
                {
                    context.AddFailure(path,
                        ValidationMessageCatalog.ValueMustNotBeSetForStringWhitelistFields(nameof(leaf.IntValue)));
                }

                if (!AnalyzeOperatorRules.AllowsStringOperands(leaf.Operator))
                {
                    context.AddFailure(nameof(leaf.Operator),
                        ValidationMessageCatalog.OperatorNotPermittedForField(
                            leaf.Operator,
                            InstallationSiteAnalyzeField.Name));
                }

                break;

            default:
                context.AddFailure(path,
                    ValidationMessageCatalog.FieldNotWhitelistedRootScalar(leaf.Field));
                break;
        }
    }

    private sealed class LeafCounter
    {
        internal int Count;
    }
}
