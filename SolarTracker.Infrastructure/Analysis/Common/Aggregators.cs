using System.Linq.Expressions;

namespace SolarTracker.Infrastructure.Analysis.Common;

internal static class Aggregators
{
    internal static Expression CombineAll(IReadOnlyList<Expression> operands)
    {
        if (operands.Count == 0)
            return Expression.Constant(true);

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
            return Expression.Constant(false);

        Expression aggregate = operands[0];
        for (int i = 1; i < operands.Count; i++)
        {
            aggregate = Expression.OrElse(aggregate, operands[i]);
        }

        return aggregate;
    }
}
