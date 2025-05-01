namespace Mini.Compiler.Services;

/// <summary>
/// Handles all console UI components and display formatting for the Mini Compiler
/// </summary>
public static class ConsoleUI
{
    #region Welcome Screen

    public static void ShowWelcomeScreen()
    {
        Console.Clear();

        DisplayHeader();
        DisplayFeaturesAndTeam();

        Console.WriteLine("\nEnter your program:");
        Console.WriteLine("───────────────────");
    }

    private static void DisplayHeader()
    {
        int consoleWidth = Console.WindowWidth;
        string[] headerLines = [
            "╔═══════════════════════════════════════════╗",
            "║      Mini Compiler - Phase 1 & 2          ║",
            "║     Lexical and Syntax Analysis           ║",
            "╚═══════════════════════════════════════════╝"
        ];

        Console.ForegroundColor = ConsoleColor.Cyan;

        foreach (string line in headerLines)
        {
            int padding = (consoleWidth - line.Length) / 2;
            Console.WriteLine(new string(' ', padding) + line);
        }
        
        Console.ResetColor();
    }

    private static void DisplayFeaturesAndTeam()
    {
        string[] teamLines = [
            "┌─────────────── Team Gener8 ───────────────┐",
            "│                                           │",
            "│  1. Mohamed Khaled Farag                  │",
            "│  2. Ahmed Abdel Fatah                     │",
            "│  3. Ashrqat Ali Fawzy                     │",
            "│  4. Alzahraa Mohy Abdelaty                │",
            "│  5. Kareem Ahmed Morsi                    │",
            "│  6. Amgad Aly Mohamed                     │",
            "│  7. Mohamed Wael Fathy                    │",
            "│  8. Omar Mohamed Moustafa                 │",
            "│  9. Abdelrahman Ibrahim Kamel             │",
            "│                                           │",
            "└───────────────────────────────────────────┘"
        ];

        string[] featuresLines = [
            "┌──────────────── Supported Features ────────────────┐",
            "│                                                    │",
            "│  • Variable declarations (int, float, string)       │",
            "│  • Variable assignments with type checking          │",
            "│  • Arithmetic operations (+, -, *, /) with types    │",
            "│  • If-else statements                               │",
            "│  • Parentheses for expression grouping              │",
            "│                                                    │",
            "└────────────────────────────────────────────────────┘"
        ];

        Console.WriteLine();

        for (int i = 0; i < Math.Max(teamLines.Length, featuresLines.Length); i++)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            if (i < teamLines.Length)
            {
                Console.Write(teamLines[i]);
            }
            else
            {
                Console.Write(new string(' ', 45));
            }
            Console.ResetColor();

            Console.Write("    ");

            Console.ForegroundColor = ConsoleColor.Yellow;
            if (i < featuresLines.Length)
            {
                Console.Write(featuresLines[i]);
            }
            Console.ResetColor();

            Console.WriteLine();
        }
    }

    #endregion

    #region Analyzing Message

    public static void ShowAnalyzingMessage(string input)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("\nAnalyzing: ");
        Console.WriteLine(input);
        Console.WriteLine(new string('-', input.Length + 11));
        Console.ResetColor();
    }

    #endregion

    #region Tokens

    public static void ShowTokens(List<Token> tokens)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\nTokens:");
        foreach (var token in tokens)
        {
            string tokenType = token.Type;
            string separator = " | ";
            string value = token.Value;

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("  ");
            Console.ForegroundColor = GetTokenColor(token.Type);
            Console.Write($"{tokenType,-15}");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(separator);
            Console.WriteLine(value);
        }
    }

    private static ConsoleColor GetTokenColor(string tokenType)
    {
        return tokenType switch
        {
            "KEYWORD" => ConsoleColor.Cyan,
            "IDENTIFIER" => ConsoleColor.Cyan,
            "OPERATOR" => ConsoleColor.Cyan,
            "NUMBER" => ConsoleColor.Cyan,
            "STRING" => ConsoleColor.Cyan,
            "SEPARATOR" => ConsoleColor.Cyan,
            "ASSIGNMENT" => ConsoleColor.Cyan,
            "INTEGER" => ConsoleColor.Cyan,
            _ => ConsoleColor.White
        };
    }

    #endregion

    #region Parse Tree

    public static void ShowParseTree(ParseTreeNode parseTree)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\nParse Tree:");
        Console.Write(parseTree.ToString());
        Console.ResetColor();
        Console.WriteLine("\n───────────────────");
    }

    #endregion
}
