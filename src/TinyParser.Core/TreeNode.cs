using System.Linq.Expressions;
using System.Reflection;

namespace TinyParser.Core;

public class TreeNode
{
    public string Operator { get; set; }
    public string Field { get; set; }
    public string Comparator { get; set; }
    public string Value { get; set; }
    public List<TreeNode> Children { get; set; }

    public TreeNode()
    {
        Children = new List<TreeNode>();
    }
}
