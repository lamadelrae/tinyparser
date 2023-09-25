namespace TinyParser.Core;

using System.Collections.Generic;

public class Parser
{
    private readonly string _expression;
    private int index;

    private static char[] OperatorChars =
    {
        'O',
        'R',
        'A',
        'N',
        'D'
    };

    public Parser(string expression)
    {
        this._expression = expression;
        this.index = 0;
    }

    public TreeNode Parse()
    {
        return ParseExpression();
    }

    private TreeNode ParseExpression()
    {
        var node = ParseTerm();

        while (index < _expression.Length && _expression[index] == ' ')
        {
            index++;
            string op = ReadOperator();
            index++;
            TreeNode right = ParseTerm();
            var newRoot = new TreeNode
            {
                Operator = op,
                Children = new List<TreeNode> { node, right }
            };
            node = newRoot;
        }

        return node;
    }

    private TreeNode ParseTerm()
    {
        if (_expression[index] == '(')
        {
            index++; // Skip '('
            TreeNode node = ParseExpression();
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
            return new TreeNode
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
        while (index < _expression.Length && char.IsLetterOrDigit(_expression[index]))
        {
            index++;
        }
        return _expression[start..index];
    }

    private string ReadComparator()
    {
        int start = index;

        while (index < _expression.Length && _expression[index] != ':')
        {
            index++;
        }

        return _expression[start..index];
    }

    private string ReadValue()
    {
        int start = index;
        while (index < _expression.Length && _expression[index] != ' ' && _expression[index] != ')')
        {
            index++;
        }
        return _expression[start..index];
    }

    private string ReadOperator()
    {
        int start = index;

        while (index < _expression.Length && OperatorChars.Contains(_expression[index]))
        {
            index++;
        }

        return _expression[start..index];
    }
}