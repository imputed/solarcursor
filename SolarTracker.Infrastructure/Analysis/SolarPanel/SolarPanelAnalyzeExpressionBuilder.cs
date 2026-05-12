using System.Linq.Expressions;
using SolarTracker.Application.Analysis.SolarPanel;
using SolarTracker.Infrastructure.Analysis.Common;
using SolarTracker.Infrastructure.Persistence.Entities;

namespace SolarTracker.Infrastructure.Analysis.SolarPanel;

internal static class SolarPanelAnalyzeExpressionBuilder
{
    internal static Expression<Func<SolarPanelDb, bool>> BuildPredicate(SolarPanelAnalysisNode? root)
    {
        ParameterExpression parameter = Expression.Parameter(typeof(SolarPanelDb), "solarPanel");
        Expression body = root is null ? Expression.Constant(true) : Build(parameter, root);
        return Expression.Lambda<Func<SolarPanelDb, bool>>(body, parameter);
    }

    private static Expression Build(ParameterExpression parameter, SolarPanelAnalysisNode node) =>
        node switch
        {
            SolarPanelLeafPredicate leaf => BuildLeaf(parameter, leaf),
            SolarPanelAllNode all => Aggregators.CombineAll(MapChildren(parameter, all.Items)),
            SolarPanelAnyNode any => Aggregators.CombineAny(MapChildren(parameter, any.Items)),
            _ => throw new ArgumentOutOfRangeException(nameof(node), node.GetType().Name, null),
        };

    private static List<Expression> MapChildren(
        ParameterExpression parameter,
        IReadOnlyList<SolarPanelAnalysisNode> nodes) =>
        nodes.Select(child => Build(parameter, child)).ToList();

    private static Expression BuildLeaf(ParameterExpression parameter, SolarPanelLeafPredicate leaf) =>
        leaf.Field switch
        {
            SolarPanelAnalyzeField.Id => Primitives.IntComparison(
                Expression.Property(parameter, nameof(SolarPanelDb.Id)),
                leaf.Operator,
                leaf.IntValue!.Value),
            SolarPanelAnalyzeField.InstallationSiteId => Primitives.IntComparison(
                Expression.Property(parameter, nameof(SolarPanelDb.InstallationSiteId)),
                leaf.Operator,
                leaf.IntValue!.Value),
            SolarPanelAnalyzeField.SerialNumber => Primitives.StringComparison(
                Expression.Property(parameter, nameof(SolarPanelDb.SerialNumber)),
                leaf.Operator,
                leaf.TextValue!),
            _ => throw new ArgumentOutOfRangeException(nameof(leaf.Field), leaf.Field, null),
        };
}
