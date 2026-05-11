using System.Linq.Expressions;
using SolarTracker.Application.Analysis;
using SolarTracker.Infrastructure.Analysis.Common;
using SolarTracker.Infrastructure.Persistence.Entities;

namespace SolarTracker.Infrastructure.Analysis.CurrentMeasuringUnit;

internal static class CurrentMeasuringUnitAnalyzeExpressionBuilder
{
    internal static Expression<Func<CurrentMeasuringUnitDb, bool>> BuildPredicate(CurrentMeasuringUnitAnalysisNode? root)
    {
        ParameterExpression parameter = Expression.Parameter(typeof(CurrentMeasuringUnitDb), "currentMeasuringUnit");
        Expression body = root is null ? Expression.Constant(true) : Build(parameter, root);
        return Expression.Lambda<Func<CurrentMeasuringUnitDb, bool>>(body, parameter);
    }

    private static Expression Build(ParameterExpression parameter, CurrentMeasuringUnitAnalysisNode node) =>
        node switch
        {
            CurrentMeasuringUnitLeafPredicate leaf => BuildLeaf(parameter, leaf),
            CurrentMeasuringUnitAllNode all => Aggregators.CombineAll(MapChildren(parameter, all.Items)),
            CurrentMeasuringUnitAnyNode any => Aggregators.CombineAny(MapChildren(parameter, any.Items)),
            _ => throw new ArgumentOutOfRangeException(nameof(node), node.GetType().Name, null),
        };

    private static List<Expression> MapChildren(
        ParameterExpression parameter,
        IReadOnlyList<CurrentMeasuringUnitAnalysisNode> nodes) =>
        nodes.Select(child => Build(parameter, child)).ToList();

    private static Expression BuildLeaf(ParameterExpression parameter, CurrentMeasuringUnitLeafPredicate leaf) =>
        leaf.Field switch
        {
            CurrentMeasuringUnitAnalyzeField.Id => Primitives.IntComparison(
                Expression.Property(parameter, nameof(CurrentMeasuringUnitDb.Id)),
                leaf.Operator,
                leaf.IntValue!.Value),
            CurrentMeasuringUnitAnalyzeField.SolarPanelId => Primitives.IntComparison(
                Expression.Property(parameter, nameof(CurrentMeasuringUnitDb.SolarPanelId)),
                leaf.Operator,
                leaf.IntValue!.Value),
            CurrentMeasuringUnitAnalyzeField.Name => Primitives.StringComparison(
                Expression.Property(parameter, nameof(CurrentMeasuringUnitDb.Name)),
                leaf.Operator,
                leaf.TextValue!),
            CurrentMeasuringUnitAnalyzeField.GpioPin => Primitives.IntComparison(
                Expression.Property(parameter, nameof(CurrentMeasuringUnitDb.GpioPin)),
                leaf.Operator,
                leaf.IntValue!.Value),
            _ => throw new ArgumentOutOfRangeException(nameof(leaf.Field), leaf.Field, null),
        };
}
