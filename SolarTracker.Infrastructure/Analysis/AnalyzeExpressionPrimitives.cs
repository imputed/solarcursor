using System.Linq.Expressions;
using SolarTracker.Application.Analysis;
using SolarTracker.Infrastructure.Persistence.Entities;

namespace SolarTracker.Infrastructure.Analysis;

internal static class AnalyzeExpressionPrimitives
{
    internal static Expression IntComparison(MemberExpression property, ScalarComparisonOperator op, int value)
    {
        Expression rhs = Expression.Constant(value);
        return op switch
        {
            ScalarComparisonOperator.Equal => Expression.Equal(property, rhs),
            ScalarComparisonOperator.NotEqual => Expression.NotEqual(property, rhs),
            ScalarComparisonOperator.GreaterThan => Expression.GreaterThan(property, rhs),
            ScalarComparisonOperator.GreaterThanOrEqual => Expression.GreaterThanOrEqual(property, rhs),
            ScalarComparisonOperator.LessThan => Expression.LessThan(property, rhs),
            ScalarComparisonOperator.LessThanOrEqual => Expression.LessThanOrEqual(property, rhs),
            _ => throw new NotSupportedException($"Operator {op} is not supported for integer fields."),
        };
    }

    internal static Expression StringComparison(MemberExpression property, ScalarComparisonOperator op, string text)
    {
        Expression valueExpr = Expression.Constant(text, typeof(string));

        return op switch
        {
            ScalarComparisonOperator.Equal => Expression.Equal(property, valueExpr),
            ScalarComparisonOperator.NotEqual => Expression.NotEqual(property, valueExpr),
            ScalarComparisonOperator.GreaterThan => Expression.GreaterThan(property, valueExpr),
            ScalarComparisonOperator.GreaterThanOrEqual => Expression.GreaterThanOrEqual(property, valueExpr),
            ScalarComparisonOperator.LessThan => Expression.LessThan(property, valueExpr),
            ScalarComparisonOperator.LessThanOrEqual => Expression.LessThanOrEqual(property, valueExpr),
            ScalarComparisonOperator.Contains =>
                Expression.AndAlso(
                    Expression.NotEqual(property, Expression.Constant(null, property.Type)),
                    Expression.Call(property, nameof(string.Contains), Type.EmptyTypes, valueExpr)),
            ScalarComparisonOperator.StartsWith =>
                Expression.AndAlso(
                    Expression.NotEqual(property, Expression.Constant(null, property.Type)),
                    Expression.Call(property, nameof(string.StartsWith), Type.EmptyTypes, valueExpr)),
            _ =>
                throw new NotSupportedException($"Operator {op} is not supported for string fields."),
        };
    }

    internal static Expression CombineAll(IReadOnlyList<Expression> operands)
    {
        if (operands.Count == 0)
        {
            return Expression.Constant(true);
        }

        Expression aggregate = operands[0];
        for (int i = 1; i < operands.Count; i++)
        {
            aggregate = Expression.AndAlso(aggregate, operands[i]);
        }

        return aggregate;
    }

    internal static Expression CombineAny(IReadOnlyList<Expression> operands)
    {
        if (operands.Count == 0)
        {
            return Expression.Constant(false);
        }

        Expression aggregate = operands[0];
        for (int i = 1; i < operands.Count; i++)
        {
            aggregate = Expression.OrElse(aggregate, operands[i]);
        }

        return aggregate;
    }

    internal static Expression BuildInstallationSite(ParameterExpression parameter, InstallationSiteAnalysisNode node) =>
        node switch
        {
            InstallationSiteLeafPredicate leaf => BuildInstallationSiteLeaf(parameter, leaf),
            InstallationSiteAllNode all =>
                CombineAll(MapInstallationSiteChildren(parameter, all.Items)),
            InstallationSiteAnyNode any =>
                CombineAny(MapInstallationSiteChildren(parameter, any.Items)),
            _ => throw new ArgumentOutOfRangeException(nameof(node), node?.GetType().Name, null),
        };

    internal static Expression BuildSolarPanel(ParameterExpression parameter, SolarPanelAnalysisNode node) =>
        node switch
        {
            SolarPanelLeafPredicate leaf => BuildSolarPanelLeaf(parameter, leaf),
            SolarPanelAllNode all =>
                CombineAll(MapSolarPanelChildren(parameter, all.Items)),
            SolarPanelAnyNode any =>
                CombineAny(MapSolarPanelChildren(parameter, any.Items)),
            _ => throw new ArgumentOutOfRangeException(nameof(node), node?.GetType().Name, null),
        };

    internal static Expression BuildLinearMotor(ParameterExpression parameter, LinearMotorAnalysisNode node) =>
        node switch
        {
            LinearMotorLeafPredicate leaf => BuildLinearMotorLeaf(parameter, leaf),
            LinearMotorAllNode all =>
                CombineAll(MapLinearMotorChildren(parameter, all.Items)),
            LinearMotorAnyNode any =>
                CombineAny(MapLinearMotorChildren(parameter, any.Items)),
            _ => throw new ArgumentOutOfRangeException(nameof(node), node?.GetType().Name, null),
        };

    internal static Expression<Func<InstallationSiteDb, bool>> BuildInstallationSitePredicate(
        InstallationSiteAnalysisNode? root)
    {
        ParameterExpression parameter = Expression.Parameter(typeof(InstallationSiteDb), "installationSite");

        Expression body = root is null ? Expression.Constant(true) : BuildInstallationSite(parameter, root);

        return Expression.Lambda<Func<InstallationSiteDb, bool>>(body, parameter);
    }

    internal static Expression<Func<SolarPanelDb, bool>> BuildSolarPanelPredicate(SolarPanelAnalysisNode? root)
    {
        ParameterExpression parameter = Expression.Parameter(typeof(SolarPanelDb), "solarPanel");

        Expression body = root is null ? Expression.Constant(true) : BuildSolarPanel(parameter, root);

        return Expression.Lambda<Func<SolarPanelDb, bool>>(body, parameter);
    }

    internal static Expression<Func<LinearMotorDb, bool>> BuildLinearMotorPredicate(LinearMotorAnalysisNode? root)
    {
        ParameterExpression parameter = Expression.Parameter(typeof(LinearMotorDb), "linearMotor");

        Expression body = root is null ? Expression.Constant(true) : BuildLinearMotor(parameter, root);

        return Expression.Lambda<Func<LinearMotorDb, bool>>(body, parameter);
    }

    private static List<Expression> MapInstallationSiteChildren(
        ParameterExpression parameter,
        IReadOnlyList<InstallationSiteAnalysisNode> nodes) =>
        nodes.Select(child => BuildInstallationSite(parameter, child)).ToList();

    private static List<Expression> MapSolarPanelChildren(ParameterExpression parameter, IReadOnlyList<SolarPanelAnalysisNode> nodes) =>
        nodes.Select(child => BuildSolarPanel(parameter, child)).ToList();

    private static List<Expression> MapLinearMotorChildren(ParameterExpression parameter, IReadOnlyList<LinearMotorAnalysisNode> nodes) =>
        nodes.Select(child => BuildLinearMotor(parameter, child)).ToList();

    private static Expression BuildInstallationSiteLeaf(ParameterExpression parameter, InstallationSiteLeafPredicate leaf) =>
        leaf.Field switch
        {
            InstallationSiteAnalyzeField.Id => IntComparison(
                Expression.Property(parameter, nameof(InstallationSiteDb.Id)),
                leaf.Operator,

                leaf.IntValue!.Value),
            InstallationSiteAnalyzeField.Name => StringComparison(
                Expression.Property(parameter, nameof(InstallationSiteDb.Name)),
                leaf.Operator,

                leaf.TextValue!),
            _ => throw new ArgumentOutOfRangeException(nameof(leaf.Field), leaf.Field, null),
        };

    private static Expression BuildSolarPanelLeaf(ParameterExpression parameter, SolarPanelLeafPredicate leaf) =>
        leaf.Field switch
        {
            SolarPanelAnalyzeField.Id => IntComparison(
                Expression.Property(parameter, nameof(SolarPanelDb.Id)),
                leaf.Operator,

                leaf.IntValue!.Value),

            SolarPanelAnalyzeField.InstallationSiteId => IntComparison(
                Expression.Property(parameter, nameof(SolarPanelDb.InstallationSiteId)),
                leaf.Operator,

                leaf.IntValue!.Value),

            SolarPanelAnalyzeField.SerialNumber => StringComparison(
                Expression.Property(parameter, nameof(SolarPanelDb.SerialNumber)),
                leaf.Operator,

                leaf.TextValue!),
            _ => throw new ArgumentOutOfRangeException(nameof(leaf.Field), leaf.Field, null),
        };

    private static Expression BuildLinearMotorLeaf(ParameterExpression parameter, LinearMotorLeafPredicate leaf) =>
        leaf.Field switch
        {
            LinearMotorAnalyzeField.Id => IntComparison(
                Expression.Property(parameter, nameof(LinearMotorDb.Id)),
                leaf.Operator,

                leaf.IntValue!.Value),

            LinearMotorAnalyzeField.InstallationSiteId => IntComparison(
                Expression.Property(parameter, nameof(LinearMotorDb.InstallationSiteId)),
                leaf.Operator,

                leaf.IntValue!.Value),

            LinearMotorAnalyzeField.Name => StringComparison(
                Expression.Property(parameter, nameof(LinearMotorDb.Name)),
                leaf.Operator,

                leaf.TextValue!),
            _ => throw new ArgumentOutOfRangeException(nameof(leaf.Field), leaf.Field, null),
        };
}
