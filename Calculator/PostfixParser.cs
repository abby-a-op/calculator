using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;

namespace Calculator;

public class PostfixParser
{
    private const string OPERATORS = "+-*/^";

    public Token[] Expression = new Token[0];

    public double Evaluate()
    {
        Stack<double> NumberStack = new Stack<double>();
        Stack<Token> OperatorStack = new Stack<Token>();

        foreach (Token token in Expression)
        {
            if (token.Type == TokenType.Number)
            {
                NumberStack.Push(double.Parse(token.Value));
            }
            else
            {
                OperatorStack.Push(token);
            }
            
            while (
                OperatorStack.TryPeek(out Token top) && (
                    top.Type == TokenType.Function && NumberStack.Count >= 1
                    || top.Type == TokenType.Operator && NumberStack.Count >= 2
                    )
                )
            {
                Token popped = OperatorStack.Pop();

                if (popped.Type == TokenType.Function)
                {
                    double n = NumberStack.Pop();

                    double result = Functions.EvaluateFunction(popped.Value, n);

                    NumberStack.Push(result);
                }
                else if (popped.Type == TokenType.Operator)
                {
                    Operator op = (Operator)popped.Value[0];

                    double rhs = NumberStack.Pop();
                    double lhs = NumberStack.Pop();

                    double result = ApplyOperation(lhs, rhs, op);

                    NumberStack.Push(result);
                }
            }
        }
        return NumberStack.Pop();
    }

    private double ApplyOperation(double a, double b, Operator op)
    {
        if (op == Operator.Divide && b == 0)
        {
            throw new DivideByZeroException();
        }

        return op switch
        {
            Operator.Plus => a+b,
            Operator.Minus => a-b,
            Operator.Multiply => a*b,
            Operator.Divide => a/b,
            Operator.Exponentiate => Math.Pow(a, b),
            Operator.Modulo => a%b,
            _ => -1
        };
    }
}

public enum Operator
{
    Plus = '+',
    Minus = '-',
    Multiply = '*',
    Divide = '/',
    Exponentiate = '^',
    Modulo = '%'
}
