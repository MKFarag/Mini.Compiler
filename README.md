# Mini Compiler

A simple compiler implementation that includes lexical analysis (tokenization) and syntax analysis (parsing) for a small programming language.

## Overview

Mini Compiler is a demonstration project that implements the fundamental components of a compiler:

- **Lexical Analysis**: Converts source code into tokens
- **Syntax Analysis**: Validates and creates a parse tree from tokens

The compiler supports basic programming constructs and provides detailed error messages.

## Project Structure

- **Program.cs**: Main entry point with the REPL (Read-Eval-Print-Loop) interface
- **Models/**
  - **Token.cs**: Represents tokens produced by the lexical analyzer
  - **ParseTreeNode.cs**: Represents nodes in the parse tree produced by the parser
- **Services/**
  - **Lexer.cs**: Implements the lexical analyzer that tokenizes input code
  - **Parser.cs**: Implements the syntax analyzer that creates a parse tree
  - **ConsoleUI.cs**: Handles console UI formatting and display

## Supported Features

- Variable declarations (int, float, string)
- Variable assignments with type checking
- Arithmetic operations (+, -, *, /) with type validation
- Comparison operations (==, !=, <, >, <=, >=)
- If-else statements with condition checking
- Parentheses for expression grouping
- Detailed error reporting

## Language Grammar

The compiler supports a C-like syntax for variable declarations and control structures:

```
// Variable declarations
int x = 10;
float y = 5.5;
string message = "Hello world";

// Arithmetic expressions
int result = (x + 5) * 2;

// If-else structures
if (x > 10) {
    result = 100;
} else {
    result = 200;
}
```

## Error Handling

The compiler performs the following validations:

- Type checking for variable assignments
- Validation of declaration syntax
- Verification of proper expression structure
- Checking for missing semicolons and braces
- Validation of if-else conditions

## Implementation Details

### Lexer
The lexical analyzer splits the input code into tokens, identifying:
- Keywords: `int`, `float`, `double`, `string`, `if`, `else`
- Operators: `+`, `-`, `*`, `/`, `=`, `==`, `!=`, `<`, `>`, `<=`, `>=`
- Separators: `;`, `(`, `)`, `{`, `}`, `,`
- Identifiers, numbers, and string literals

### Parser
The parser implements a recursive descent parsing algorithm to:
- Create a hierarchical parse tree
- Validate the syntactic structure of the code
- Support nested expressions and statements

### Console UI
The console interface provides:
- A welcome screen with project information
- Token visualization with proper formatting
- Parse tree display with a hierarchical structure

## How to Use

1. Run the application
2. Enter code at the prompt
3. Use `exit` to quit the application

Example:
```
> int x = 10;
```

The compiler will display:
- The tokens identified
- The parse tree structure

## Development

Built with .NET 9.0 as a console application.

## Team Gener8

- Mohamed Khaled Farag
- Ahmed Abdel Fatah
- Ashrqat Ali Fawzy
- Alzahraa Mohy Abdelaty
- Kareem Ahmed Morsi
- Amgad Aly Mohamed
- Mohamed Wael Fathy
- Omar Mohamed Moustafa
- Abdelrahman Ibrahim Kamel 