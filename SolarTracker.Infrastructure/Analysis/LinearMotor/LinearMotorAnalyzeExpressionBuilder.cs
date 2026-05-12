using System.Linq.Expressions;
using SolarTracker.Application.Analysis.LinearMotor;
using SolarTracker.Infrastructure.Analysis.Common;
using SolarTracker.Infrastructure.Persistence.Entities;

namespace SolarTracker.Infrastructure.Analysis.LinearMotor;

internal static class LinearMotorAnalyzeExpressionBuilder
{
    internal static Expression<Func<LinearMotorDb, bool>> BuildPredicate(LinearMotorAnalysisNode? root)
    {
        ParameterExpression parameter = Expression.Parameter(typeof(LinearMotorDb), "linearMotor");
        Expression body = root is null ? Expression.Constant(true) : Build(parameter, root);
        return Expression.Lambda<Func<LinearMotorDb, bool>>(body, parameter);
    }

    private static Expression Build(ParameterExpression parameter, LinearMotorAnalysisNode node) =>
        node switch
        {
            LinearMotorLeafPredicate leaf => BuildLeaf(parameter, leaf),
            LinearMotorAllNode all => Aggregators.CombineAll(MapChildren(parameter, all.Items)),
            LinearMotorAnyNode any => Aggregators.CombineAny(MapChildren(parameter, any.Items)),
            _ => throw new ArgumentOutOfRangeException(nameof(node), node.GetType().Name, null),
        };

    private static List<Expression> MapChildren(
        ParameterExpression parameter,
        IReadOnlyList<LinearMotorAnalysisNode> nodes) =>
        nodes.Select(child => Build(parameter, child)).ToList();

    private static Expression BuildLeaf(ParameterExpression parameter, LinearMotorLeafPredicate leaf) =>
        leaf.Field switch
        {
            LinearMotorAnalyzeField.Id => Primitives.IntComparison(
                Expression.Property(parameter, nameof(LinearMotorDb.Id)),
                leaf.Operator,
                leaf.IntValue!.Value),
            LinearMotorAnalyzeField.SolarPanelId => Primitives.IntComparison(
                Expression.Property(parameter, nameof(LinearMotorDb.SolarPanelId)),
                leaf.Operator,
                leaf.IntValue!.Value),
            LinearMotorAnalyzeField.Name => Primitives.StringComparison(
                Expression.Property(parameter, nameof(LinearMotorDb.Name)),
                leaf.Operator,
                leaf.TextValue!),
            _ => throw new ArgumentOutOfRangeException(nameof(leaf.Field), leaf.Field, null),
        };
}
