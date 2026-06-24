using System.Net.Http.Headers;

namespace Calculator;

public struct Integer: IToken
{
    public int Value { get; set; }

    public Integer(int value)
    {
        this.Value = value;
    }

    public TokenType Type => TokenType.Integer;

    public IToken ApplyOperation(IToken? rhs, OperatorType op)
    {
        if (rhs == null)
        {
            return op switch
            {
                OperatorType.UnaryPlus => new Integer(Value),
                OperatorType.UnaryMinus => new Integer(-Value),
                _ => throw new InvalidOperationException($"Operator {op} is not valid for one integer")
            };
        }
        else
        {
            if (rhs.Type == TokenType.Integer)
            {
                int a = this.Value;
                int b = ((Integer)rhs).Value;

                if (op == OperatorType.Divide && b == 0)
                {
                    throw new DivideByZeroException();
                }

                if (op == OperatorType.Exponentiate && a == 0 && b == 0)
                {
                    throw new ArithmeticException("0^0 is undefined");
                }

                return op switch
                {
                    OperatorType.Plus => new Integer(a+b),
                    OperatorType.Minus => new Integer(a-b),
                    OperatorType.Multiply => new Integer(a*b),
                    OperatorType.Divide => new Integer(a/b),
                    OperatorType.Exponentiate => new Integer((int)Math.Pow(a, b)),
                    OperatorType.Modulo => new Integer(Mod(a, b)),
                    _ => throw new InvalidOperationException($"Operator {op} is not valid for two ints")
                };
            }
            else if (rhs.Type == TokenType.Real)
            {
                return CastTo(TokenType.Real).ApplyOperation(rhs, op);
            }
        }

        throw new InvalidOperationException($"Operation {op} is invalid for Integer and {rhs.Type}");
    }

    private int Mod(int a, int b)
    {
        int res = a%b;

        if (res < 0) res += b;
        return res;
    }

    public IToken CastTo(TokenType castTo)
    {
        if (castTo == Type) return this;

        if (castTo == TokenType.Real)
        {
            return new Real(Value);
        }

        throw new InvalidCastException("Cannot cast integer to " + castTo);
    }

    public string Output() => Value.ToString();
}