using System.Linq.Expressions;
using SolarTracker.Application.Analysis.InstallationSite;
using SolarTracker.Infrastructure.Analysis.Common;
using SolarTracker.Infrastructure.Persistence.Entities;

namespace SolarTracker.Infrastructure.Analysis.InstallationSite;

internal static class InstallationSiteAnalyzeExpressionBuilder
{
    internal static Expression<Func<InstallationSiteDb, bool>> BuildPredicate(InstallationSiteAnalysisNode? root)
    {
        ParameterExpression parameter = Expression.Parameter(typeof(InstallationSiteDb), "installationSite");
        Expression body = root is null ? Expression.Constant(true) : Build(parameter, root);
        return Expression.Lambda<Func<InstallationSiteDb, bool>>(body, parameter);
    }

    private static Expression Build(ParameterExpression parameter, InstallationSiteAnalysisNode node) =>
        node switch
        {
            InstallationSiteLeafPredicate leaf => BuildLeaf(parameter, leaf),
            InstallationSiteAllNode all => Aggregators.CombineAll(MapChildren(parameter, all.Items)),
            InstallationSiteAnyNode any => Aggregators.CombineAny(MapChildren(parameter, any.Items)),
            _ => throw new ArgumentOutOfRangeException(nameof(node), node.GetType().Name, null),
        };

    private static List<Expression> MapChildren(
        ParameterExpression parameter,
        IReadOnlyList<InstallationSiteAnalysisNode> nodes) =>
        nodes.Select(child => Build(parameter, child)).ToList();

    private static Expression BuildLeaf(ParameterExpression parameter, InstallationSiteLeafPredicate leaf) =>
        leaf.Field switch
        {
            InstallationSiteAnalyzeField.Id => Primitives.IntComparison(
                Expression.Property(parameter, nameof(InstallationSiteDb.Id)),
                leaf.Operator,
                leaf.IntValue!.Value),
            InstallationSiteAnalyzeField.Name => Primitives.StringComparison(
                Expression.Property(parameter, nameof(InstallationSiteDb.Name)),
                leaf.Operator,
                leaf.TextValue!),
            _ => throw new ArgumentOutOfRangeException(nameof(leaf.Field), leaf.Field, null),
        };
}
