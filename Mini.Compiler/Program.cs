namespace Mini.Compiler;

internal class Program
{
    static void Main()
    {
        ConsoleUI.ShowWelcomeScreen();

        bool isRunning = true;

        while (isRunning)
            ProcessUserInput(ref isRunning);

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\nThank you for using Mini Compiler!");
        Console.ResetColor();
    }

    private static void ProcessUserInput(ref bool isRunning)
    {
        #region For input

        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("> ");
        Console.ResetColor();

        string? input = Console.ReadLine()?.TrimStart();

        if (string.IsNullOrWhiteSpace(input)) 
            return;

        if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
        {
            isRunning = false;
            return;
        }

        #endregion

        #region Call services

        ConsoleUI.ShowAnalyzingMessage(input);

        try
        {
            var tokens = StartLexicalAnalysis(input);

            _ = StartParsing(tokens);
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\nError: {ex.Message}");
            Console.ResetColor();
        }

        #endregion
    }

    private static List<Token> StartLexicalAnalysis(string input)
    {
        var lexer = new Lexer(input);
        var tokens = lexer.Tokenize();

        ConsoleUI.ShowTokens(tokens);

        return tokens;
    }

    private static ParseTreeNode StartParsing(List<Token> tokens)
    {
        var parser = new Parser(tokens);
        var parseTree = parser.Parse();

        ConsoleUI.ShowParseTree(parseTree);

        return parseTree;
    }
}
