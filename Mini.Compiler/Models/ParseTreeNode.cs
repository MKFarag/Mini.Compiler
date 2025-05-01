namespace Mini.Compiler.Models;

public class ParseTreeNode(string type, string value)
{
    public string Type { get; } = type;
    public string Value { get; } = value;
    public List<ParseTreeNode> Children { get; } = [];

    public void AddChild(ParseTreeNode child)
        => Children.Add(child);

    public string ToString(int indent = 0)
        => ToStringWithPrefix("", indent);

    private string ToStringWithPrefix(string prefix, int indent)
    {
        string result = prefix;
        
        // Add the current node
        if (indent == 0)
            result += "└─ ";
        else
            result += "├─ ";
            
        result += $"{Type}: {Value}\n";
        
        // Add children
        for (int i = 0; i < Children.Count; i++)
        {
            bool isLastChild = i == Children.Count - 1;
            string childPrefix = prefix + (indent == 0 ? "   " : isLastChild ? "    " : "│   ");
            
            if (isLastChild)
                result += Children[i].ToStringWithPrefix(childPrefix, 0);
            else
                result += Children[i].ToStringWithPrefix(childPrefix, 1);
        }
        
        return result;
    }
} 