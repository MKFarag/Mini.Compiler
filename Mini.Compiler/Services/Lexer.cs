namespace Mini.Compiler.Services;

public class Lexer(string input)
{
    private readonly string _input = input;
    private int _position = 0;

    // Define our token types
    private static readonly string[] _keywords = ["int", "float", "double", "string", "if", "else"];
    private static readonly string[] _operators = ["+", "-", "*", "/", "=", "==", "!=", "<", ">", "<=", ">="];
    private static readonly string[] _separators = [";", "(", ")", "{", "}"];

    public List<Token> Tokenize()
    {
        var tokens = new List<Token>();

        // State variables to track the parsing process
        bool foundDataType = false;
        bool foundIdentifier = false;
        bool foundAssignment = false;
        bool isDeclaration = false;
        string? currentDataType = null;
        string? currentIdentifier = null;

        while (_position < _input.Length)
        {
            char current = _input[_position];

            // Skip whitespace
            if (char.IsWhiteSpace(current))
            {
                _position++;
                continue;
            }

            Token token;

            // Handle numbers
            if (char.IsDigit(current))
            {
                token = ReadNumber();
                
                // Type checking for number assignments
                if (foundAssignment && currentDataType != null && isDeclaration)
                {
                    if (currentDataType == "string")
                    {
                        throw new Exception($"Cannot assign number to string variable '{currentIdentifier}'");
                    }
                }
            }
            // Handle identifiers and keywords
            else if (char.IsLetter(current))
            {
                token = ReadIdentifier();

                if (token.Type == "KEYWORD" && _keywords.Contains(token.Value))
                {
                    if (token.Value == "int" || token.Value == "float" || token.Value == "double" || token.Value == "string")
                    {
                        if (foundDataType)
                        {
                            throw new Exception("Multiple data types in declaration");
                        }
                        foundDataType = true;
                        isDeclaration = true;
                        currentDataType = token.Value;
                    }
                }
                else if (token.Type == "IDENTIFIER")
                {
                    if (isDeclaration && !foundDataType)
                    {
                        throw new Exception("Variable declaration must start with a data type");
                    }
                    foundIdentifier = true;
                    currentIdentifier = token.Value;
                }
            }
            // Handle operators
            else if (_operators.Any(op => op.StartsWith(current.ToString())))
            {
                token = ReadOperator();

                if (token.Value == "=")
                {
                    if (isDeclaration && !foundIdentifier)
                    {
                        throw new Exception("Assignment operator must follow an identifier");
                    }
                    foundAssignment = true;
                }
            }
            // Handle separators
            else if (_separators.Contains(current.ToString()))
            {
                token = new Token("SEPARATOR", current.ToString());

                _position++;

                if (token.Value == ";")
                {
                    // Validate declaration statements only if this is a declaration
                    if (isDeclaration)
                    {
                        if (!foundDataType)
                        {
                            throw new Exception("Variable declaration must start with a data type");
                        }

                        if (!foundIdentifier)
                        {
                            throw new Exception("Variable declaration must contain an identifier");
                        }

                        if (!foundAssignment)
                        {
                            throw new Exception("Variable declaration must contain an assignment");
                        }
                    }

                    // Reset state for next statement
                    foundDataType = false;
                    foundIdentifier = false;
                    foundAssignment = false;
                    isDeclaration = false;
                    currentDataType = null;
                    currentIdentifier = null;
                }
            }
            // Handle string literals
            else if (current == '"')
            {
                token = ReadString();
                
                // Type checking for string assignments
                if (foundAssignment && currentDataType != null && isDeclaration)
                {
                    if (currentDataType != "string")
                    {
                        throw new Exception($"Cannot assign string to {currentDataType} variable '{currentIdentifier}'");
                    }
                }
            }
            else
            {
                throw new Exception($"Invalid character: {current}");
            }

            tokens.Add(token);
        }

        // Check if we have an incomplete statement at the end
        if (isDeclaration)
        {
            throw new Exception("Missing semicolon at the end of statement");
        }

        return tokens;
    }

    #region Number

    private Token ReadNumber()
    {
        int start = _position;
        bool hasDecimalPoint = false;

        while (_position < _input.Length && 
              (char.IsDigit(_input[_position]) || !hasDecimalPoint && _input[_position] == '.'))
        {
            if (_input[_position] == '.')
                hasDecimalPoint = true;

            _position++;
        }

        string number = _input[start.._position];
        return new Token("NUMBER", number);
    }

    #endregion

    #region Identifier and Keyword

    private Token ReadIdentifier()
    {
        int start = _position;

        while (_position < _input.Length && (char.IsLetterOrDigit(_input[_position]) || _input[_position] == '_'))
            _position++;

        string identifier = _input[start.._position];
        string type = _keywords.Contains(identifier) ? "KEYWORD" : "IDENTIFIER";
        return new Token(type, identifier);
    }

    #endregion

    #region Operator

    private Token ReadOperator()
    {
        int start = _position;

        while (_position < _input.Length && 
               _operators.Any(op => op.StartsWith(_input.Substring(start, _position - start + 1))))
            _position++;

        string op = _input[start.._position];
        return new Token("OPERATOR", op);
    }

    #endregion

    #region String

    private Token ReadString()
    {
        _position++; // Skip opening quote
        int start = _position;
        while (_position < _input.Length && _input[_position] != '"')
        {
            _position++;
        }
        if (_position >= _input.Length)
        {
            throw new Exception("Unterminated string literal");
        }
        string str = _input[start.._position];
        _position++; // Skip closing quote
        return new Token("STRING", str);
    }

    #endregion
}