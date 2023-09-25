namespace TinyParser.Core;

using System.Collections.Generic;

public class Parser
{
    private readonly string _expression;
    private int _index;

    private static readonly char[] OperatorChars =
    {
        'O',
        'R',
        'A',
        'N',
        'D'
    };

    public Parser(string expression)
    {
        _expression = expression;
        _index = 0;
    }

    public TreeNode Parse()
    {
        return ParseExpression();
    }

    private TreeNode ParseExpression()
    {
        var node = ParseTerm();

        while (_index < _expression.Length && _expression[_index] == ' ')
        {
            _index++;
            string op = ReadOperator();
            _index++;

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
        if (_expression[_index] == '(')
        {
            _index++; // Skip '('
            TreeNode node = ParseExpression();
            _index++; // Skip ')'

            return node;
        }
        else
        {
            string field = ReadField();

            _index++; // skip :
            string comparator = ReadComparator();
            _index++; // skip :

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
        int start = _index;

        while (_index < _expression.Length && char.IsLetterOrDigit(_expression[_index]))
        {
            _index++;
        }

        return _expression[start.._index];
    }

    private string ReadComparator()
    {
        int start = _index;

        while (_index < _expression.Length && _expression[_index] != ':')
        {
            _index++;
        }

        return _expression[start.._index];
    }

    private string ReadValue()
    {
        int start = _index;

        while (_index < _expression.Length && _expression[_index] != ' ' && _expression[_index] != ')')
        {
            _index++;
        }

        return _expression[start.._index];
    }

    private string ReadOperator()
    {
        int start = _index;

        while (_index < _expression.Length && OperatorChars.Contains(_expression[_index]))
        {
            _index++;
        }

        return _expression[start.._index];
    }
}