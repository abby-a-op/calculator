namespace Calculator;

public struct Matrix: IToken
{
    public double A, B, C, D;

    public TokenType Type => TokenType.Matrix;

    public Matrix(double x1, double y1, double x2, double y2)
    {
        A = x1;
        B = y1;
        C = x2;
        D = y2;
    }

    public Real Det()
    {
        return new Real(A * D - C * B);
    }

    public Matrix Scale(Real s)
    {
        double sValue = s.Value;

        return new Matrix(sValue * A, sValue * B, sValue * C, sValue * D);
    }

    public Matrix Add(Matrix b)
    {
        return new Matrix(
            b.A + A, b.B + B,
            b.C + C, b.D + D
        );
    }

    public Matrix Dot(Matrix b)
    {
        double x1, y1, x2, y2;

        x1 = A*b.A + C*b.B;
        y1 = B*b.A + D*b.B;
        x2 = A*b.C + C*b.D;
        y2 = B*b.C + D*b.D;

        return new Matrix(x1, y1, x2, y2);
    }

    public Matrix UnaryMinus()
    {
        return new Matrix(-A, -B, -C, -D);
    }

    public Matrix Sub(Matrix b)
    {
        return Add(b.UnaryMinus());
    }

    public Matrix Inv()
    {
        Matrix aInv = new Matrix(D, -B, -C, A);
        Real detReciprocal = (Real)new Real(1.0).ApplyOperation(Det(), OperatorType.Divide).CastTo(TokenType.Real);
        return aInv.Scale(detReciprocal);
    }

    public IToken CastTo(TokenType castTo)
    {
        if (castTo == Type) return this;
        if (castTo == TokenType.Text) return new Text(Output());

        throw new InvalidCastException("Cannot cast matrix to " + castTo);
    }

    public string Output()
    {
        return $"""
        {A}, {C}
        {B}, {D}
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