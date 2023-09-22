using TinyParser.Core;

string expression = "(field1:eq:value1 OR (field2:gt:value2 AND field3:lt:value3)) AND (field4:lt:value4)";
ExpressionParser parser = new ExpressionParser(expression);
ExpressionNode root = parser.Parse();
PrintExpressionTree(root, 0);

static void PrintExpressionTree(ExpressionNode node, int indentLevel)
{
    string indent = new(' ', indentLevel * 4);

    if (node.Operator != null)
    {
        Console.WriteLine($"{indent}Operator: {node.Operator}");
    }
    else
    {
        Console.WriteLine($"{indent}Field: {node.Field}, Comparator: {node.Comparator}, Value: {node.Value}");
    }

    foreach (var child in node.Children)
    {
        PrintExpressionTree(child, indentLevel + 1);
    }
}