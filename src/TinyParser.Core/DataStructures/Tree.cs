namespace TinyParser.Core.DataStructures;

public class Node { };

public class Parent : Node
{
    public string Operator { get; set; }
    public Node Left { get; set; }
    public Node Right { get; set; }
}

public class Leaf : Node
{
    public string Field { get; set; }
    public string Comparator { get; set; }
    public string Value { get; set; }
}
