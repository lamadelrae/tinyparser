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
            var fieldExpression = Expression.PropertyOrField(parameter, leaf.Field);
            var valueExpression = Expression.Constant(InferType(leaf.Value));

            if (leaf.Comparator == "eq") return Expression.Equal(fieldExpression, valueExpression);
            else if (leaf.Comparator == "lt") return Expression.LessThan(fieldExpression, valueExpression);
            else if (leaf.Comparator == "gt") return Expression.GreaterThan(fieldExpression, valueExpression);
            else if (leaf.Comparator == "like")
            {
                valueExpression = Expression.Constant(InferType(leaf.Value.Trim('*')));

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

    private static object InferType(string input)
    {
        if (int.TryParse(input, out int intValue)) return intValue;
        else if (double.TryParse(input, out double doubleValue)) return doubleValue;
        else if (bool.TryParse(input, out bool boolValue)) return boolValue;
        else if (DateTime.TryParse(input, out DateTime dateTimeValue)) return dateTimeValue;
        else return input;
    }
}
