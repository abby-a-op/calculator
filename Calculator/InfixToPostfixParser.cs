namespace Calculator;

public class InfixToPostfixParser
{
    static Dictionary<string, int> OperatorPrecedence = new Dictionary<string, int>()
    {
        { "+", 1 },
        { "-", 1 },
        { "*", 2 },
        { "/", 2 },
        { "%", 2 },
        { "^", 3 },
        { "!", 3 }
    };

    public Token[] Expression = new Token[] { };

    // Implementation of Shunting Yard Algorithm (Reference: https://mathcenter.oxford.emory.edu/site/cs171/shuntingYardAlgorithm)
    public Token[] Parse()
    {
        Stack<Token> operatorStack = new Stack<Token>();

        // The parsed postfix expression
        List<Token> postfix = new List<Token>();

        foreach (Token token in Expression)
        {
            // Numbers are added directly to the expression
            if (token.Type == TokenType.Number)
            {
                postfix.Add(token);
                continue;
            }

            if (token.Value == "!")
            {
                postfix.Add(token);
                continue;
            }

            // Functions are added to the stack, and are printed when the following opening bracket is closed
            if (token.Type == TokenType.Function)
            {
                operatorStack.Push(token);
                continue;
            }

            if (token.Type == TokenType.Operator)
            {
                switch (token.Value)
                {
                    case "(":
                        operatorStack.Push(token);
                        break;
                    case ")":
                        ParseClosingBracket(operatorStack, postfix);
                        break;
                    default:
                        if (operatorStack.Count == 0)
                        {
                            operatorStack.Push(token);
                            break;
                        }

                        Token topOperator = operatorStack.Peek();

                        if (topOperator.Value == "(")
                        {
                            operatorStack.Push(token);
                            break;
                        }

                        int currentPrecedence = OperatorPrecedence[token.Value];
                        int topPrecedence = OperatorPrecedence[topOperator.Value];

                        if (
                            currentPrecedence > topPrecedence
                            || currentPrecedence == topPrecedence && token.Value == "^"
                            || topOperator.Value == "("
                            )
                        {
                            operatorStack.Push(token);
                            break;
                        }

                        if (
                            currentPrecedence < topPrecedence
                            || currentPrecedence == topPrecedence && token.Value != "^"
                        )
                        {
                            ParseLowerPrecedence(operatorStack, postfix, token, currentPrecedence, topPrecedence);
                            break;
                        }

                        break;
                }

            }

        }

        while (operatorStack.Count != 0)
        {
            postfix.Add(operatorStack.Pop());
        }

        return postfix.ToArray();
    }

    private static void ParseLowerPrecedence(Stack<Token> operatorStack, List<Token> postfix, Token token, int currentPrecedence, int topPrecedence)
    {
        while (
            (currentPrecedence < topPrecedence
            || currentPrecedence == topPrecedence && token.Value != "^")
            && operatorStack.Count != 0
            )
        {
            postfix.Add(operatorStack.Pop());
            if (operatorStack.Count != 0)
            {
                topPrecedence = OperatorPrecedence[operatorStack.Peek().Value];
            }
        }

        operatorStack.Push(token);
    }

    private static void ParseClosingBracket(Stack<Token> operatorStack, List<Token> postfix)
    {
        Token poppedOperator;

        do
        {
            poppedOperator = operatorStack.Pop();

            if (poppedOperator.Value != "(")
            {
                postfix.Add(poppedOperator);

                if (operatorStack.TryPeek(out Token topToken) && topToken.Type == TokenType.Function)
                {
                    postfix.Add(operatorStack.Pop());
                }
            }
        } while (poppedOperator.Value != "(");
        return;
    }
}