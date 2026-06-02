using System.Reflection;
using System.Security.Cryptography;

namespace Calculator;

public class PostfixParser
{
    private const string OPERATORS = "+-*/^";

    public string[] Expression { get; set; }

    public PostfixParser(string[] expression)
    {
        this.Expression = expression;
    }

    public int Evaluate()
    {
        Stack<int> NumberStack = new Stack<int>();
        Stack<Operator> OperatorStack = new Stack<Operator>();

        foreach (string token in Expression)
        {
            if (OPERATORS.Contains(token))
            {
                OperatorStack.Push((Operator)token[0]);
            }
            else
            {
                NumberStack.Push(int.Parse(token));
            }

            while (NumberStack.Count >= 2 && OperatorStack.Count >= 1)
            {
                int rhs = NumberStack.Pop();
                int lhs = NumberStack.Pop();

                Operator op = OperatorStack.Pop();

                int c = ApplyOperation(lhs, rhs, op);

                NumberStack.Push(c);
            }
        }
        return NumberStack.Pop();
    }

    private int ApplyOperation(int a, int b, Operator op)
    {
        return op switch
        {
            Operator.Plus => a+b,
            Operator.Minus => a-b,
            Operator.Multiply => a*b,
            Operator.Divide => a/b,
            Operator.Exponentiate => (int)Math.Pow(b,a),
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
    Exponentiate = '^'
}
