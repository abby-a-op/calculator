namespace Calculator;

// Struct for function token, used to allow functions to be in an expression
public struct Function: IToken
{
    public string Name { get; set; }

    public TokenType Type => TokenType.Function;

    public Function(string value)
    {
        this.Name = value;
    }

    public string Output() => Name;

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