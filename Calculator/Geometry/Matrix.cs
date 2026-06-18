namespace Calculator;

public struct Matrix: IToken
{
    public double X1, Y1, X2, Y2;

    public TokenType Type => TokenType.Matrix;

    public Matrix(double x1, double y1, double x2, double y2)
    {
        X1 = x1;
        Y1 = y1;
        X2 = x2;
        Y2 = y2;
    }

    public Real Det()
    {
        return new Real(X1 * Y2 - X2 * Y1);
    }

    public Matrix Scale(Real s)
    {
        double sValue = s.Value;

        return new Matrix(sValue * X1, sValue * Y1, sValue * X2, sValue * Y2);
    }

    public Matrix Add(Matrix b)
    {
        return new Matrix(
            b.X1 + X1, b.Y1 + Y1,
            b.X2 + X2, b.Y2 + Y2
        );
    }

    public Matrix Dot(Matrix b)
    {
        double x1, y1, x2, y2;

        x1 = X1*b.X1 + X2*b.Y1;
        y1 = Y1*b.X1 + Y2*b.Y1;
        x2 = X1*b.X2 + X2*b.Y2;
        y2 = Y1*b.X2 + Y2*b.Y2;

        return new Matrix(x1, y1, x2, y2);
    }

    public Matrix UnaryMinus()
    {
        return new Matrix(-X1, -Y1, -X2, -Y2);
    }

    public Matrix Sub(Matrix b)
    {
        return Add(b.UnaryMinus());
    }

    public string Output()
    {
        return $"""
        {X1}, {X2}
        {Y1}, {Y2}
        """;
    }

    public IToken ApplyOperation(IToken? rhs, OperatorType op)
    {
        if (rhs == null)
        {
            return op switch
            {
                OperatorType.UnaryPlus => this,
                OperatorType.UnaryMinus => UnaryMinus(),
                _ => throw new InvalidOperationException($"Operation {op} is not valid on a single matrix")
            };
        }

        if (rhs.Type == TokenType.Matrix)
        {
            Matrix b = (Matrix)rhs;

            return op switch
            {
                OperatorType.Plus => Add(b),
                OperatorType.Minus => Sub(b),
                OperatorType.Multiply => Dot(b),
                _ => throw new InvalidOperationException($"Operator {op} is not valid on two matrices")
            };
        }

        if (rhs.Type == TokenType.Real)
        {
            Real s = (Real)rhs;
            return op switch
            {
                OperatorType.Multiply => Scale(s),
                _ => throw new InvalidOperationException($"Operator {op} is not valid on Matrix and Real")
            };
        }

        if (rhs.Type == TokenType.Integer)
        {
            Real x = new Real(((Integer)rhs).Value);

            return ApplyOperation(x, op);
        }

        throw new InvalidOperationException($"Operator {op} is not valid on {rhs.Type} and Matrix");
    }
}