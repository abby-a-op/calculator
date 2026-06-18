namespace Calculator;

public class InfixToPostfixParser
{
    static Dictionary<OperatorType, int> OperatorPrecedence = new Dictionary<OperatorType, int>()
    {
        { OperatorType.Plus, 1 },
        { OperatorType.Minus, 1 },
        { OperatorType.Multiply, 2 },
        { OperatorType.Divide, 2 },
        { OperatorType.Modulo, 2 },
        { OperatorType.UnaryMinus, 3 },
        { OperatorType.UnaryPlus, 3 },
        { OperatorType.Exponentiate, 4 },
    };

    private static OperatorType[] RightAssociativeOperators = new OperatorType[]
    {
        OperatorType.Exponentiate,
        OperatorType.UnaryMinus,
        OperatorType.UnaryPlus
    };

    public IToken[] Expression = new IToken[] { };

    // Implementation of Shunting Yard Algorithm (Reference: https://mathcenter.oxford.emory.edu/site/cs171/shuntingYardAlgorithm)
    public IToken[] Parse()
    {
        Stack<IToken> operatorStack = new Stack<IToken>();

        // The parsed postfix expression
        List<IToken> postfix = new List<IToken>();

        foreach (IToken token in Expression)
        {
            // Numbers are added directly to the expression
            if ((token.Type & TokenType.Operand) != TokenType.Undefined)
            {
                postfix.Add(token);
                continue;
            }

            // Functions are added to the stack, and are printed when the following opening bracket is closed
            if (token.Type == TokenType.Function)
            {
                Function functionToken = (Function)token;

                // Since factorials are already a postfix operation, they can be written directly
                if (functionToken.Value == "!")
                {
                    postfix.Add(token);
                    continue;
                }

                operatorStack.Push(token);
                continue;
            }

            if (token.Type == TokenType.Operator)
            {
                Operator operatorToken = (Operator)token;
                switch (operatorToken.Value)
                {
                    case OperatorType.OpeningBracket:
                    {
                        operatorStack.Push(token);
                        break;
                    }
                    case OperatorType.ClosingBracket:
                    {
                        ParseClosingBracket(operatorStack, postfix);
                        break;
                    }
                    default:
                        if (operatorStack.Count == 0)
                        {
                            operatorStack.Push(token);
                            break;
                        }

                        Operator nextOperator = (Operator)operatorStack.Peek();

                        if (nextOperator.Value == OperatorType.OpeningBracket)
                        {
                            operatorStack.Push(token);
                            break;
                        }

                        int currentPrecedence = OperatorPrecedence[operatorToken.Value];
                        int topPrecedence = OperatorPrecedence[nextOperator.Value];
                        
                        bool isRightAssociative = RightAssociativeOperators.Contains(nextOperator.Value);

                        if (
                            currentPrecedence > topPrecedence
                            || currentPrecedence == topPrecedence && isRightAssociative
                            )
                        {
                            operatorStack.Push(token);
                            break;
                        }

                        if (
                            currentPrecedence < topPrecedence
                            || currentPrecedence == topPrecedence && !isRightAssociative
                        )
                        {
                            ParseLowerPrecedence(operatorStack, postfix, operatorToken, currentPrecedence, topPrecedence);
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

    private static void ParseLowerPrecedence(Stack<IToken> operatorStack, List<IToken> postfix, Operator token, int currentPrecedence, int topPrecedence)
    {
        while (
            (currentPrecedence < topPrecedence
            || currentPrecedence == topPrecedence && token.Value != OperatorType.Exponentiate)
            && operatorStack.Count != 0
            )
        {
            postfix.Add(operatorStack.Pop());
            if (operatorStack.Count != 0)
            {
                Operator nextOperator = (Operator)operatorStack.Peek();
                topPrecedence = OperatorPrecedence[nextOperator.Value];
            }
        }

        operatorStack.Push(token);
    }

    private static void ParseClosingBracket(Stack<IToken> operatorStack, List<IToken> postfix)
    {
        IToken poppedOperator;
        bool isNotOpeningBracket;
        do
        {
            poppedOperator = operatorStack.Pop();
            isNotOpeningBracket = poppedOperator.Type == TokenType.Operator && ((Operator)poppedOperator).Value != OperatorType.OpeningBracket;

            if (isNotOpeningBracket)
            {
                postfix.Add(poppedOperator);

                if (operatorStack.TryPeek(out IToken? topToken) && topToken.Type == TokenType.Function)
                {
                    postfix.Add(operatorStack.Pop());
                }
            }
        } while (isNotOpeningBracket && operatorStack.Count > 0);
        return;
    }
}