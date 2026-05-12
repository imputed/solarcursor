using FluentValidation;
using SolarTracker.Application.Analysis;
using SolarTracker.Api.Validation;

namespace SolarTracker.Api.Validation.Analyze;

public sealed class CurrentMeasuringUnitAnalyzeRequestValidator : AbstractValidator<CurrentMeasuringUnitAnalyzeRequest>
{
    private const int MaxDepth = 32;
    private const int MaxLeafCount = 100;

    public CurrentMeasuringUnitAnalyzeRequestValidator()
    {
        RuleFor(r => r.Take).InclusiveBetween(1, 500);
        RuleFor(r => r.Skip).GreaterThanOrEqualTo(0);
        RuleFor(r => r.SortBy).Must(f => !f.HasValue || Enum.IsDefined(f.Value))
            .WithMessage(ValidationMessageCatalog.SortByMustBeDefinedWhitelistedField());
        RuleFor(r => r).Custom(ValidateGraph);
    }

    private static void ValidateGraph(
        CurrentMeasuringUnitAnalyzeRequest request,
        ValidationContext<CurrentMeasuringUnitAnalyzeRequest> context)
    {
        var leafCounter = new LeafCounter();

        ValidateNode(request.Filter, context, "filter", 0, leafCounter);

        if (leafCounter.Count > MaxLeafCount)
        {
            context.AddFailure(nameof(CurrentMeasuringUnitAnalyzeRequest.Filter),
                ValidationMessageCatalog.MaximumPredicateLeavesSupported(MaxLeafCount));
        }
    }

    private static void ValidateNode(
        CurrentMeasuringUnitAnalysisNode? node,
        ValidationContext<CurrentMeasuringUnitAnalyzeRequest> context,
        string path,
        int depth,
        LeafCounter leafCounter)
    {
        if (node is null)
            return;

        if (depth > MaxDepth)
        {
            context.AddFailure(path, ValidationMessageCatalog.FilterNestingMustNotExceed(MaxDepth));
            return;
        }

        switch (node)
        {
            case CurrentMeasuringUnitLeafPredicate leaf:
                ValidateLeaf(leaf, context, $"{path}.{leaf.Field}", leafCounter);
                break;
            case CurrentMeasuringUnitAllNode all:
                ValidateItems(all.Items, context, path, $"{path}.items", depth, leafCounter, "All");
                break;
            case CurrentMeasuringUnitAnyNode any:
                ValidateItems(any.Items, context, path, $"{path}.items", depth, leafCounter, "Any");
                break;
        }
    }

    private static void ValidateItems(
        IReadOnlyList<CurrentMeasuringUnitAnalysisNode>? items,
        ValidationContext<CurrentMeasuringUnitAnalyzeRequest> context,
        string path,
        string itemsPath,
        int depth,
        LeafCounter leafCounter,
        string combiner)
    {
        if (items is null)
        {
            context.AddFailure(itemsPath, ValidationMessageCatalog.CombinerItemsCannotBeNull(combiner));
            return;
        }

        int i = 0;
        foreach (CurrentMeasuringUnitAnalysisNode item in items)
        {
            ValidateNode(item, context, $"{path}[{i}]", depth + 1, leafCounter);
            i++;
        }
    }

    private static void ValidateLeaf(
        CurrentMeasuringUnitLeafPredicate leaf,
        ValidationContext<CurrentMeasuringUnitAnalyzeRequest> context,
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
            case CurrentMeasuringUnitAnalyzeField.Id:
            case CurrentMeasuringUnitAnalyzeField.SolarPanelId:
            case CurrentMeasuringUnitAnalyzeField.GpioPin:
                ValidateIntLeaf(leaf, context, path);
                break;
            case CurrentMeasuringUnitAnalyzeField.Name:
                ValidateStringLeaf(leaf, context, path);
                break;
            default:
                context.AddFailure(path, ValidationMessageCatalog.FieldNotWhitelistedRootScalar(leaf.Field));
                break;
        }
    }

    private static void ValidateIntLeaf(
        CurrentMeasuringUnitLeafPredicate leaf,
        ValidationContext<CurrentMeasuringUnitAnalyzeRequest> context,
        string path)
    {
        if (leaf.IntValue is null)
        {
            context.AddFailure(path, ValidationMessageCatalog.ValueRequiredForIntegerWhitelistFields(nameof(leaf.IntValue)));
        }

        if (leaf.TextValue is not null)
        {
            context.AddFailure(path, ValidationMessageCatalog.ValueMustNotBeSetForIntegerWhitelistFields(nameof(leaf.TextValue)));
        }

        if (!AnalyzeOperatorRules.AllowsIntOperands(leaf.Operator))
        {
            context.AddFailure(nameof(leaf.Operator), ValidationMessageCatalog.OperatorNotPermittedForIntegerFields(leaf.Operator));
        }
    }

    private static void ValidateStringLeaf(
        CurrentMeasuringUnitLeafPredicate leaf,
        ValidationContext<CurrentMeasuringUnitAnalyzeRequest> context,
        string path)
    {
        if (leaf.TextValue is null)
        {
            context.AddFailure(path, ValidationMessageCatalog.ValueRequiredWhenFilteringStringWhitelistFields(nameof(leaf.TextValue)));
        }

        if (leaf.IntValue is not null)
        {
            context.AddFailure(path, ValidationMessageCatalog.ValueMustNotBeSetForStringWhitelistFields(nameof(leaf.IntValue)));
        }

        if (!AnalyzeOperatorRules.AllowsStringOperands(leaf.Operator))
        {
            context.AddFailure(nameof(leaf.Operator), ValidationMessageCatalog.OperatorNotPermittedForStringFields(leaf.Operator));
        }
    }

    private sealed class LeafCounter
    {
        internal int Count;
    }
}
