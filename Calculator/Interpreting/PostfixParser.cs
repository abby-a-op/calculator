using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;

namespace Calculator;

public class PostfixParser
{
    private const string OPERATORS = "+-*/^";

    public IToken[] Expression = new IToken[] { };

    public IToken Evaluate()
    {
        Stack<IToken> NumberStack = new Stack<IToken>();
        Stack<IToken> OperatorStack = new Stack<IToken>();

        foreach (IToken token in Expression)
        {
            if ((token.Type & TokenType.Operand) != TokenType.Undefined)
            {
                NumberStack.Push(token);
            }
            else
            {
                OperatorStack.Push(token);
            }
            
            while (
                OperatorStack.Count > 0
                && NumberStack.Count > 0
                )
            {
                IToken top = OperatorStack.Pop();

                if (top.Type == TokenType.Function)
                {
                    IToken n = NumberStack.Pop();
                    IToken result = Functions.EvaluateFunction((Function)top, n);

                    NumberStack.Push(result);
                }
                else if (top.Type == TokenType.Operator)
                {
                    OperatorType op = ((Operator)top).Value;

                    if (op == OperatorType.UnaryPlus || op == OperatorType.UnaryMinus)
                    {
                        IToken lhs = NumberStack.Pop();

                        IToken result = lhs.ApplyOperation(null, op);
                        NumberStack.Push(result);
                    }
                    else if (NumberStack.Count > 1)
                    {
                        IToken rhs = NumberStack.Pop();
                        IToken lhs = NumberStack.Pop();

                        IToken result = lhs.ApplyOperation(rhs, op);
                        NumberStack.Push(result);
                    }
                    else
                    {
                        throw new FormatException("Not a valid mathematical expression");
                    }
                }
            }
        }

        if (NumberStack.Count != 1 || OperatorStack.Count != 0)
        {
            throw new FormatException("Not a valid mathematical expression");
        }

        return NumberStack.Pop();
    }
}

