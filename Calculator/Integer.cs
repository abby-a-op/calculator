namespace Calculator;

public struct Integer: IToken<int>
{
    public int Value { get; set; }

    public Integer(int value)
    {
        this.Value = value;
    }

    public TokenType Type => TokenType.Integer;

    public IToken ApplyOperation(IToken rhs, OperatorType op)
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
                throw new ArithmeticException();
            }

            int result = op switch
            {
                OperatorType.Plus => a+b,
                OperatorType.Minus => a-b,
                OperatorType.Multiply => a*b,
                OperatorType.Divide => a/b,
                OperatorType.Exponentiate => (int)Math.Pow(a, b),
                OperatorType.Modulo => a%b,
                _ => -1
            };

            return new Integer(result);
        }

        return new Integer(-1);
    }

    public string Output() => Value.ToString();
}