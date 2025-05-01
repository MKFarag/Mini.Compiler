namespace Mini.Compiler.Services;

public class Parser(List<Token> tokens)
{
    private readonly List<Token> _tokens = tokens;
    private int _currentTokenIndex = 0;

    public ParseTreeNode Parse()
    {
        return ParseProgram();
    }

    private ParseTreeNode ParseProgram()
    {
        var programNode = new ParseTreeNode("Program", "Program");
        
        while (_currentTokenIndex < _tokens.Count)
        {
            programNode.AddChild(ParseStatement());
        }

        return programNode;
    }

    private ParseTreeNode ParseStatement()
    {
        if (_currentTokenIndex >= _tokens.Count)
            throw new Exception("Unexpected end of input while parsing statement");
            
        var currentToken = PeekToken();

        if (currentToken.Type == "KEYWORD" && (currentToken.Value == "int" || currentToken.Value == "float" || 
            currentToken.Value == "double" || currentToken.Value == "string"))
        {
            return ParseDeclaration();
        }
        else if (currentToken.Type == "KEYWORD" && currentToken.Value == "if")
        {
            return ParseIfStatement();
        }
        else
        {
            var expr = ParseExpression();
            // Check for semicolons for expression statements
            if (_currentTokenIndex < _tokens.Count && PeekToken().Type == "SEPARATOR" && PeekToken().Value == ";")
                ConsumeToken(); // Consume the semicolon
            return expr;
        }
    }

    private ParseTreeNode ParseDeclaration()
    {
        var typeToken = ConsumeToken();
        
        if (_currentTokenIndex >= _tokens.Count)
            throw new Exception("Unexpected end of input after type in declaration");
            
        var identifierToken = ConsumeToken();
        
        if (_currentTokenIndex >= _tokens.Count)
            throw new Exception("Unexpected end of input after identifier in declaration");
            
        ConsumeToken("OPERATOR", "=");
        
        if (_currentTokenIndex >= _tokens.Count)
            throw new Exception("Unexpected end of input after = in declaration");
            
        var expressionNode = ParseExpression();
        
        if (_currentTokenIndex >= _tokens.Count)
            throw new Exception("Unexpected end of input after expression in declaration");
            
        ConsumeToken("SEPARATOR", ";");

        var declarationNode = new ParseTreeNode("Declaration", typeToken.Value);
        declarationNode.AddChild(new ParseTreeNode("Identifier", identifierToken.Value));
        declarationNode.AddChild(expressionNode);
        return declarationNode;
    }

    private ParseTreeNode ParseIfStatement()
    {
        ConsumeToken("KEYWORD", "if");
        ConsumeToken("SEPARATOR", "(");
        
        // Parse the condition expression
        var conditionNode = ParseExpression();
        
        // Validate condition expression
        if (conditionNode.Type != "BinaryOperation" || 
            conditionNode.Value != "==" && conditionNode.Value != "!=" && 
             conditionNode.Value != "<" && conditionNode.Value != ">" && 
             conditionNode.Value != "<=" && conditionNode.Value != ">=")
        {
            throw new Exception("If condition must be a comparison expression (==, !=, <, >, <=, >=)");
        }
        
        ConsumeToken("SEPARATOR", ")");
        ConsumeToken("SEPARATOR", "{");
        
        var ifNode = new ParseTreeNode("IfStatement", "if");
        ifNode.AddChild(conditionNode);
        
        // Parse statements inside if block
        while (_currentTokenIndex < _tokens.Count && 
               !(PeekToken().Type == "SEPARATOR" && PeekToken().Value == "}"))
        {
            ifNode.AddChild(ParseStatement());
        }
        
        if (_currentTokenIndex >= _tokens.Count)
            throw new Exception("Unexpected end of input: missing closing brace for if statement");
            
        ConsumeToken("SEPARATOR", "}");
        
        // Handle else clause if present
        if (_currentTokenIndex < _tokens.Count && 
            PeekToken().Type == "KEYWORD" && PeekToken().Value == "else")
        {
            ConsumeToken("KEYWORD", "else");
            
            // Check if there's an else if
            if (_currentTokenIndex < _tokens.Count && 
                PeekToken().Type == "KEYWORD" && PeekToken().Value == "if")
            {
                // Handle else if
                var elseIfNode = ParseIfStatement();
                var elseNode = new ParseTreeNode("ElseStatement", "else");
                elseNode.AddChild(elseIfNode);
                ifNode.AddChild(elseNode);
            }
            else
            {
                // Handle regular else
                ConsumeToken("SEPARATOR", "{");
                
                var elseNode = new ParseTreeNode("ElseStatement", "else");
                while (_currentTokenIndex < _tokens.Count && 
                       !(PeekToken().Type == "SEPARATOR" && PeekToken().Value == "}"))
                {
                    elseNode.AddChild(ParseStatement());
                }
                
                if (_currentTokenIndex >= _tokens.Count)
                    throw new Exception("Unexpected end of input: missing closing brace for else statement");
                    
                ifNode.AddChild(elseNode);
                ConsumeToken("SEPARATOR", "}");
            }
        }

        return ifNode;
    }

    private ParseTreeNode ParseExpression()
    {
        var leftNode = ParseTerm();
        
        while (_currentTokenIndex < _tokens.Count && 
               PeekToken().Type == "OPERATOR" && 
               (PeekToken().Value == "+" || PeekToken().Value == "-" || 
                PeekToken().Value == "==" || PeekToken().Value == "!=" || 
                PeekToken().Value == "<" || PeekToken().Value == ">" || 
                PeekToken().Value == "<=" || PeekToken().Value == ">="))
        {
            var operatorToken = ConsumeToken();
            var rightNode = ParseTerm();
            
            var operatorNode = new ParseTreeNode("BinaryOperation", operatorToken.Value);
            operatorNode.AddChild(leftNode);
            operatorNode.AddChild(rightNode);
            leftNode = operatorNode;
        }
        
        return leftNode;
    }

    private ParseTreeNode ParseTerm()
    {
        var leftNode = ParseFactor();
        
        while (_currentTokenIndex < _tokens.Count && 
               PeekToken().Type == "OPERATOR" && 
               (PeekToken().Value == "*" || PeekToken().Value == "/"))
        {
            var operatorToken = ConsumeToken();
            var rightNode = ParseFactor();
            
            var operatorNode = new ParseTreeNode("BinaryOperation", operatorToken.Value);
            operatorNode.AddChild(leftNode);
            operatorNode.AddChild(rightNode);
            leftNode = operatorNode;
        }
        
        return leftNode;
    }

    private ParseTreeNode ParseFactor()
    {
        if (_currentTokenIndex >= _tokens.Count)
            throw new Exception("Unexpected end of input while parsing factor");
            
        var token = PeekToken();
        
        if (token.Type == "NUMBER")
        {
            return new ParseTreeNode("Number", ConsumeToken().Value);
        }
        else if (token.Type == "IDENTIFIER")
        {
            return new ParseTreeNode("Identifier", ConsumeToken().Value);
        }
        else if (token.Type == "STRING")
        {
            return new ParseTreeNode("String", ConsumeToken().Value);
        }
        else if (token.Type == "SEPARATOR" && token.Value == "(")
        {
            ConsumeToken(); // Consume the opening parenthesis
            var expressionNode = ParseExpression();
            
            if (_currentTokenIndex >= _tokens.Count)
                throw new Exception("Unexpected end of input: missing closing parenthesis");
                
            ConsumeToken("SEPARATOR", ")");
            return expressionNode;
        }
        
        throw new Exception($"Unexpected token: {token.Type} '{token.Value}' while parsing factor");
    }

    private Token PeekToken()
    {
        if (_currentTokenIndex >= _tokens.Count)
            throw new Exception("Unexpected end of input");
        return _tokens[_currentTokenIndex];
    }

    private Token ConsumeToken()
    {
        if (_currentTokenIndex >= _tokens.Count)
            throw new Exception("Unexpected end of input");
        return _tokens[_currentTokenIndex++];
    }

    private Token ConsumeToken(string expectedType, string expectedValue)
    {
        if (_currentTokenIndex >= _tokens.Count)
            throw new Exception($"Unexpected end of input, expected {expectedType} '{expectedValue}'");
            
        var token = ConsumeToken();
        if (token.Type != expectedType || token.Value != expectedValue)
            throw new Exception($"Expected {expectedType} '{expectedValue}', but got {token.Type} '{token.Value}'");
        return token;
    }
} 