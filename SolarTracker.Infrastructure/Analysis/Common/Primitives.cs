using System.Linq.Expressions;
using SolarTracker.Application.Analysis;
using SolarTracker.Infrastructure.Errors;

namespace SolarTracker.Infrastructure.Analysis.Common;

internal static class Primitives
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
            _ => throw new NotSupportedException(InfrastructureTextCatalog.UnsupportedIntegerOperator(op)),
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
            _ => throw new NotSupportedException(InfrastructureTextCatalog.UnsupportedStringOperator(op)),
        };
    }
}
