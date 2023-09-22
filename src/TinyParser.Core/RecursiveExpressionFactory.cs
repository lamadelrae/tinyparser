using System.Linq.Expressions;
using System.Reflection;

namespace TinyParser.Core;

public class RecursiveExpressionFactory
{
    public static Func<T, bool> Produce<T>(TreeNode node)
    {
        var parameter = Expression.Parameter(typeof(T), typeof(T).Name);
        var expression = BuildExpression(node, parameter);
        var lambda = Expression.Lambda<Func<T, bool>>(expression, parameter).Compile();
        return lambda;
    }

    private static Expression BuildExpression(TreeNode node, ParameterExpression parameter)
    {
        if (node.Operator == "OR")
        {
            var leftExpression = BuildExpression(node.Children[0], parameter);
            var rightExpression = BuildExpression(node.Children[1], parameter);
            return Expression.OrElse(leftExpression, rightExpression);
        }
        else if (node.Operator == "AND")
        {
            var leftExpression = BuildExpression(node.Children[0], parameter);
            var rightExpression = BuildExpression(node.Children[1], parameter);
            return Expression.AndAlso(leftExpression, rightExpression);
        }
        else if (node.Comparator == "eq")
        {
            var fieldExpression = Expression.PropertyOrField(parameter, node.Field);
            var valueExpression = Expression.Constant(node.Value);
            return Expression.Equal(fieldExpression, valueExpression);
        }
        else if (node.Comparator == "lt")
        {
            var fieldExpression = Expression.PropertyOrField(parameter, node.Field);
            var valueExpression = Expression.Constant(node.Value);
            return Expression.LessThan(fieldExpression, valueExpression);
        }
        else if (node.Comparator == "gt")
        {
            var fieldExpression = Expression.PropertyOrField(parameter, node.Field);
            var valueExpression = Expression.Constant(node.Value);
            return Expression.GreaterThan(fieldExpression, valueExpression);
        }
        else if (node.Comparator == "like")
        {
            var fieldExpression = Expression.PropertyOrField(parameter, node.Field);
            var valueExpression = Expression.Constant(node.Value.Trim('*'));

            MethodInfo? method;
            if (node.Value.StartsWith('*') && node.Value.EndsWith('*')) method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            else if (node.Value.EndsWith('*')) method = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
            else if (node.Value.StartsWith('*')) method = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
            else method = typeof(string).GetMethod("Equals", new[] { typeof(string) });

            return Expression.Call(fieldExpression, method, valueExpression);
        }

        throw new NotSupportedException($"Operator '{node.Operator}' not supported.");
    }
}
