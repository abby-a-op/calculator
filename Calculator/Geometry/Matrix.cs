namespace Calculator;

public struct Matrix: IToken
{
    public double A, B, C, D;

    public TokenType Type => TokenType.Matrix;

    public Matrix(double a, double b, double c, double d)
    {
        A = a;
        B = b;
        C = c;
        D = d;
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

    public Matrix Add(Matrix rhs)
    {
        return new Matrix(
            rhs.A + A, rhs.B + B,
            rhs.C + C, rhs.D + D
        );
    }

    public Matrix Dot(Matrix rhs)
    {
        double a, b, c, d;

        a = A*rhs.A + C*rhs.B;
        b = B*rhs.A + D*rhs.B;
        c = A*rhs.C + C*rhs.D;
        d = B*rhs.C + D*rhs.D;

        return new Matrix(a, b, c, d);
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

    // Allows arithmetic operations to be performed on matrices
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