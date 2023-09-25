namespace TinyParser.Core;

public class LambdaFactory
{
    public static Func<T, bool> Produce<T>(string expression)
    {
        var root = new Parser(expression).Parse();
        return RecursiveExpressionFactory.Produce<T>(root);
    }
}
