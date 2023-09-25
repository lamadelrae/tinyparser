using System.Linq.Expressions;

namespace TinyParser.Core.Generators.LINQ;

public class LambdaFactory
{
    public static Func<T, bool> Produce<T>(string expression)
    {
        var root = new Parser(expression).Parse();
        var parameter = Expression.Parameter(typeof(T), typeof(T).Name);
        var linqExpression = RecursiveExpressionGenerator.BuildExpression(root, parameter);

        var lambda = Expression.Lambda<Func<T, bool>>(linqExpression, parameter).Compile();
        return lambda;
    }
}
