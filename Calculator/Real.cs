namespace Calculator;

public class Real: IToken<double>
{
    public double Value { get; set; }

    public TokenType Type => TokenType.Real;

    public Real(double value)
    {
        this.Value = value;
    }

    public IToken ApplyOperation(IToken rhs, OperatorType op)
    {
        if (rhs.Type == TokenType.Real)
        {
            double x = ((Real)rhs).Value; 
            double res = op switch
            {
                OperatorType.Multiply => Value * x,
                OperatorType.Divide => Value / x,
                OperatorType.Plus => Value + x,
                OperatorType.Minus => Value - x,
                OperatorType.Exponentiate => Math.Pow(Value, x),
                OperatorType.Modulo => Mod(Value, x)
            };

            return new Real(res);
        }
        else if (rhs.Type == TokenType.Integer)
        {
            int n = ((Integer)rhs).Value;

            Real token = new Real(n);
            return ApplyOperation(token, op);
        }

        return new Text("Invalid");
    }

    static double Mod(double a, double b)
    {
        double res = a % b;

        if (res < 0.0) res += b;
        return res;
    }

    public string Output() => Value.ToString();
}