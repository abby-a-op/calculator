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

    public Vec2 Minus(Vec2 b)
    {
        return new Vec2(X - b.X, Y - b.Y);
    }

    public Vec2 Scale(double s)
    {
        return new Vec2(X * s, Y * s);
    }

    public double Length()
    {
        return Math.Sqrt(X*X + Y*Y);
    }

    public IToken ApplyOperation(IToken rhs, OperatorType op)
    {
        if (rhs.Type == TokenType.Vec2)
        {
            Vec2 b = (Vec2)rhs;

            return op switch
            {
                OperatorType.Plus => Add(b),
                OperatorType.Minus => Minus(b),
                _ => new Text("Invalid")
            };
        }

        return new Text("Invalid");
    }

    public string Output() => $"({X}, {Y})";
}