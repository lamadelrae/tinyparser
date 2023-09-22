namespace TinyParser.Core;

using System.Collections.Generic;

public class ExpressionNode
{
    public string Operator { get; set; }
    public string Field { get; set; }
    public string Comparator { get; set; }
    public string Value { get; set; }
    public List<ExpressionNode> Children { get; set; }

    public ExpressionNode()
    {
        Children = new List<ExpressionNode>();
    }
}

public class ExpressionParser
{
    private readonly string expression;
    private int index;

    public ExpressionParser(string expression)
    {
        this.expression = expression;
        this.index = 0;
    }

    public ExpressionNode Parse()
    {
        return ParseExpression();
    }

    private ExpressionNode ParseExpression()
    {
        var node = ParseTerm();

        while (index < expression.Length && expression[index] == ' ')
        {
            index++;
            string op = ReadOperator();
            index++;
            ExpressionNode right = ParseTerm();
            var newRoot = new ExpressionNode
            {
                Operator = op,
                Children = new List<ExpressionNode> { node, right }
            };
            node = newRoot;
        }

        return node;
    }

    private ExpressionNode ParseTerm()
    {
        if (expression[index] == '(')
        {
            index++; // Skip '('
            ExpressionNode node = ParseExpression();
            index++; // Skip ')'
            return node;
        }
        else
        {
            string field = ReadField();

            index++; // skip :
            string comparator = ReadComparator();
            index++; // skip :

            string value = ReadValue();
            return new ExpressionNode
            {
                Field = field,
                Comparator = comparator,
                Value = value
            };
        }
    }

    private string ReadField()
    {
        int start = index;
        while (index < expression.Length && char.IsLetterOrDigit(expression[index]))
        {
            index++;
        }
        return expression[start..index];
    }

    private string ReadComparator()
    {
        int start = index;

        while (index < expression.Length && expression[index] != ':')
        {
            index++;
        }

        return expression[start..index];
    }

    private string ReadValue()
    {
        int start = index;
        while (index < expression.Length && expression[index] != ' ' && expression[index] != ')')
        {
            index++;
        }
        return expression[start..index];
    }

    private string ReadOperator()
    {
        int start = index;
        while (index < expression.Length && (expression[index] == 'O' || expression[index] == 'R' || expression[index] == 'A' || expression[index] == 'N' || expression[index] == 'D'))
        {
            index++;
        }
        return expression[start..index];
    }
}