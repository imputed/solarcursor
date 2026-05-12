using FluentValidation;
using SolarTracker.Application.Analysis;

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
            .WithMessage("SortBy must be a defined whitelisted field.");
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
                $"A maximum of {MaxLeafCount} predicate leaves is supported.");
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
                $"Filter nesting must not exceed {MaxDepth} levels.");
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
            context.AddFailure($"{itemsPath}",
                $"{combiner}.items cannot be null.");
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
            context.AddFailure(path, "Predicate field must be defined on the whitelist.");
            return;
        }

        switch (leaf.Field)
        {
            case InstallationSiteAnalyzeField.Id:

                if (leaf.IntValue is null)
                {
                    context.AddFailure(path,
                        $"{nameof(leaf.IntValue)} is required when filtering on '{InstallationSiteAnalyzeField.Id}'.");
                }

                if (leaf.TextValue is not null)
                {
                    context.AddFailure(path,
                        $"{nameof(leaf.TextValue)} must not be set for integer whitelist fields.");
                }

                if (!AnalyzeOperatorRules.AllowsIntOperands(leaf.Operator))
                {
                    context.AddFailure(nameof(leaf.Operator),
                        $"Operator '{leaf.Operator}' is not permitted for '{InstallationSiteAnalyzeField.Id}'.");
                }

                break;

            case InstallationSiteAnalyzeField.Name:

                if (leaf.TextValue is null)
                {
                    context.AddFailure(path,
                        $"{nameof(leaf.TextValue)} is required when filtering on '{InstallationSiteAnalyzeField.Name}'.");
                }

                if (leaf.IntValue is not null)
                {
                    context.AddFailure(path,
                        $"{nameof(leaf.IntValue)} must not be set for string whitelist fields.");
                }

                if (!AnalyzeOperatorRules.AllowsStringOperands(leaf.Operator))
                {
                    context.AddFailure(nameof(leaf.Operator),
                        $"Operator '{leaf.Operator}' is not permitted for '{InstallationSiteAnalyzeField.Name}'.");
                }

                break;

            default:
                context.AddFailure(path,
                    $"Field '{leaf.Field}' is not a whitelisted root scalar.");
                break;
        }
    }

    private sealed class LeafCounter
    {
        internal int Count;
    }
}
