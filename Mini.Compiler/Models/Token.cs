namespace Mini.Compiler.Models;

public class Token(string type, string value)
{
    public string Type { get; } = type;
    public string Value { get; } = value;

    public override string ToString()
        => $"{Type,-15} | {Value}";
} 