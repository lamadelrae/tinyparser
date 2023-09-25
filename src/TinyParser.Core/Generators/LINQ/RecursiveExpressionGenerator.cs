using System.Linq.Expressions;
using System.Reflection;
using TinyParser.Core.DataStructures;

namespace TinyParser.Core;

public class RecursiveExpressionGenerator
{
    public static Expression BuildExpression(Node node, ParameterExpression parameter)
    {
        if (node is Parent treeNode)
        {
            if (treeNode.Operator == "OR")
            {
                var leftExpression = BuildExpression(treeNode.Left, parameter);
                var rightExpression = BuildExpression(treeNode.Right, parameter);
                return Expression.OrElse(leftExpression, rightExpression);
            }
            else if (treeNode.Operator == "AND")
            {
                var leftExpression = BuildExpression(treeNode.Left, parameter);
                var rightExpression = BuildExpression(treeNode.Right, parameter);
                return Expression.AndAlso(leftExpression, rightExpression);
            }
        }

        if (node is Leaf leaf)
        {
            if (leaf.Comparator == "eq")
            {
                var fieldExpression = Expression.PropertyOrField(parameter, leaf.Field);
                var valueExpression = Expression.Constant(leaf.Value);
                return Expression.Equal(fieldExpression, valueExpression);
            }
            else if (leaf.Comparator == "lt")
            {
                var fieldExpression = Expression.PropertyOrField(parameter, leaf.Field);
                var valueExpression = Expression.Constant(leaf.Value);
                return Expression.LessThan(fieldExpression, valueExpression);
            }
            else if (leaf.Comparator == "gt")
            {
                var fieldExpression = Expression.PropertyOrField(parameter, leaf.Field);
                var valueExpression = Expression.Constant(leaf.Value);
                return Expression.GreaterThan(fieldExpression, valueExpression);
            }
            else if (leaf.Comparator == "like")
            {
                var fieldExpression = Expression.PropertyOrField(parameter, leaf.Field);
                var valueExpression = Expression.Constant(leaf.Value.Trim('*'));

                MethodInfo? method;
                if (leaf.Value.StartsWith('*') && leaf.Value.EndsWith('*')) method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                else if (leaf.Value.EndsWith('*')) method = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
                else if (leaf.Value.StartsWith('*')) method = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
                else method = typeof(string).GetMethod("Equals", new[] { typeof(string) });

                return Expression.Call(fieldExpression, method, valueExpression);
            }
        }

        throw new NotSupportedException($"Not supported.");
    }
}
