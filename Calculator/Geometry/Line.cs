namespace Calculator;

public struct Line: IToken
{
    public Vec2 Point1, Point2;

    public TokenType Type => TokenType.Line;

    public Line(Vec2 point1, Vec2 point2)
    {
        Point1 = point1;
        Point2 = point2;
    }

    public IToken ApplyOperation(IToken? rhs, OperatorType op)
    {
        throw new InvalidOperationException("Cannot perform operation on Line");
    }

    public Real Length()
    {
        Vec2 displacementVector = Point2.Minus(Point1);

        return displacementVector.Length();
    }

    public Vec2 Midpoint()
    {
        return Point1.Add(Point2).Scale(0.5);
    }

    public Real Gradient()
    {
        double deltaX = Point2.X - Point1.X;
        double deltaY = Point2.Y - Point1.Y;

        return new Real(deltaX/deltaY);
    }

    public IToken CastTo(TokenType castTo)
    {
        if (castTo == Type) return this;

        throw new InvalidCastException("Cannot cast line to " + castTo);
    }

    public string Output() => $"({Point1.X}, {Point1.Y}) - ({Point2.X}, {Point2.Y})";
}