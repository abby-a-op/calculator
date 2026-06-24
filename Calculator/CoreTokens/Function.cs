namespace Calculator;

public struct Function: IToken
{
    public string Value { get; set; }

    public TokenType Type => TokenType.Function;

    public Function(string value)
    {
        this.Value = value;
    }

    public string Output() => Value;

    public IToken ApplyOperation(IToken? rhs, OperatorType op)
    {
        throw new InvalidOperationException($"Operator {op} is invalid on functions");
    }

    public IToken CastTo(TokenType castTo)
    {
        if (castTo == Type) return this;

        throw new InvalidCastException("Cannot cast Function to other types");
    }
}