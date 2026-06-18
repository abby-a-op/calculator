namespace Calculator;

public struct Vec2: IToken
{
    public double X, Y;

    public TokenType Type => TokenType.Vec2;

    public Vec2(double x, double y)
    {
        X = x;
        Y = y;
    }

    public Vec2 Add(Vec2 b)
    {
        return new Vec2(X + b.X, Y + b.Y);
    }

    public Vec2 UnaryMinus()
    {
        return new Vec2(-X, -Y);
    }

    public Vec2 Minus(Vec2 b)
    {
        return new Vec2(X - b.X, Y - b.Y);
    }

    public Vec2 Scale(double s)
    {
        return new Vec2(X * s, Y * s);
    }

    public Real Dot(Vec2 b)
    {
        return new Real(X * b.X + Y * b.Y);
    }

    public Real Length()
    {
        return (Real)Functions.Sqrt(new Real(X*X + Y*Y));
    }

    public IToken ApplyOperation(IToken? rhs, OperatorType op)
    {
        if (rhs == null)
        {
            return op switch
            {
                OperatorType.UnaryPlus => this,
                OperatorType.UnaryMinus => UnaryMinus(),
                _ => throw new InvalidOperationException($"Operation {op} is not valid on Vec2")
            };
        }

        if (rhs.Type == TokenType.Vec2)
        {
            Vec2 b = (Vec2)rhs;

            return op switch
            {
                OperatorType.Plus => Add(b),
                OperatorType.Minus => Minus(b),
                _ => throw new InvalidOperationException($"Operation {op} is not valid on Vec2")

            };
        }

        throw new InvalidOperationException($"Operation {op} is not valid on Vec2");
    }

    public string Output() => $"({X}, {Y})";
}