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
            if ((token.Type & TokenType.Operand) != TokenType.Invalid)
            {
                NumberStack.Push(token);
            }
            else
            {
                OperatorStack.Push(token);
            }
            
            while (
                OperatorStack.TryPeek(out IToken top)
                && NumberStack.Count > 0
                && (top.Type == TokenType.Function || top.Type == TokenType.Operator)
                )
            {
                IToken popped = OperatorStack.Pop();

                if (popped.Type == TokenType.Function)
                {
                    IToken n = NumberStack.Pop();

                    IToken result = Functions.EvaluateFunction((Function)popped, n);

                    NumberStack.Push(result);
                }
                else if (popped.Type == TokenType.Operator)
                {
                    OperatorType op = ((Operator)popped).Value;

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
                }
            }
        }
        return NumberStack.Pop();
    }
}

