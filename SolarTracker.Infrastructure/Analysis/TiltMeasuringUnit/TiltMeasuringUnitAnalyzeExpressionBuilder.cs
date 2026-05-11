using System.Linq.Expressions;
using SolarTracker.Application.Analysis;
using SolarTracker.Infrastructure.Analysis.Common;
using SolarTracker.Infrastructure.Persistence.Entities;

namespace SolarTracker.Infrastructure.Analysis.TiltMeasuringUnit;

internal static class TiltMeasuringUnitAnalyzeExpressionBuilder
{
    internal static Expression<Func<TiltMeasuringUnitDb, bool>> BuildPredicate(TiltMeasuringUnitAnalysisNode? root)
    {
        ParameterExpression parameter = Expression.Parameter(typeof(TiltMeasuringUnitDb), "tiltMeasuringUnit");
        Expression body = root is null ? Expression.Constant(true) : Build(parameter, root);
        return Expression.Lambda<Func<TiltMeasuringUnitDb, bool>>(body, parameter);
    }

    private static Expression Build(ParameterExpression parameter, TiltMeasuringUnitAnalysisNode node) =>
        node switch
        {
            TiltMeasuringUnitLeafPredicate leaf => BuildLeaf(parameter, leaf),
            TiltMeasuringUnitAllNode all => Aggregators.CombineAll(MapChildren(parameter, all.Items)),
            TiltMeasuringUnitAnyNode any => Aggregators.CombineAny(MapChildren(parameter, any.Items)),
            _ => throw new ArgumentOutOfRangeException(nameof(node), node.GetType().Name, null),
        };

    private static List<Expression> MapChildren(
        ParameterExpression parameter,
        IReadOnlyList<TiltMeasuringUnitAnalysisNode> nodes) =>
        nodes.Select(child => Build(parameter, child)).ToList();

    private static Expression BuildLeaf(ParameterExpression parameter, TiltMeasuringUnitLeafPredicate leaf) =>
        leaf.Field switch
        {
            TiltMeasuringUnitAnalyzeField.Id => Primitives.IntComparison(
                Expression.Property(parameter, nameof(TiltMeasuringUnitDb.Id)),
                leaf.Operator,
                leaf.IntValue!.Value),
            TiltMeasuringUnitAnalyzeField.InstallationSiteId => Primitives.IntComparison(
                Expression.Property(parameter, nameof(TiltMeasuringUnitDb.InstallationSiteId)),
                leaf.Operator,
                leaf.IntValue!.Value),
            TiltMeasuringUnitAnalyzeField.Name => Primitives.StringComparison(
                Expression.Property(parameter, nameof(TiltMeasuringUnitDb.Name)),
                leaf.Operator,
                leaf.TextValue!),
            TiltMeasuringUnitAnalyzeField.GpioPin => Primitives.IntComparison(
                Expression.Property(parameter, nameof(TiltMeasuringUnitDb.GpioPin)),
                leaf.Operator,
                leaf.IntValue!.Value),
            _ => throw new ArgumentOutOfRangeException(nameof(leaf.Field), leaf.Field, null),
        };
}
