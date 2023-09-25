namespace TinyParser.Core;

using TinyParser.Core.DataStructures;

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

    public Node Parse()
    {
        return ParseExpression();
    }

    private Node ParseExpression()
    {
        Node node = ParseTerm();

        while (_index < _expression.Length && _expression[_index] == ' ')
        {
            _index++;
            string op = ReadOperator();
            _index++;

            Node right = ParseTerm();

            var newRoot = new Parent
            {
                Operator = op,
                Left = node,
                Right = right,
            };

            node = newRoot;
        }

        return node;
    }

    private Node ParseTerm()
    {
        if (_expression[_index] == '(')
        {
            _index++; // Skip '('
            Node node = ParseExpression();
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

            return new Leaf
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