namespace TinyParser.Core;

public class LambaFactory
{
    public static Func<T, bool> Produce<T>(string expression)
    {
        var parser = new Parser(expression);
        TreeNode root = parser.Parse();
        return RecursiveExpressionFactory.Produce<T>(root);
    }
}
