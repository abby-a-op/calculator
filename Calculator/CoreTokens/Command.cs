namespace Calculator;

// Struct for function token, used to allow functions to be in an expression
public struct Command: IToken
{
    public string Name { get; set; }

    public TokenType Type => TokenType.Command;

    public Command(string value)
    {
        this.Name = value;
    }

    public string Output() => Name;

    public IToken ApplyOperation(IToken? rhs, OperatorType op)
    {
        throw new InvalidOperationException($"Operator {op} is invalid on command");
    }

    public IToken CastTo(TokenType castTo)
    {
        if (castTo == Type) return this;

        throw new InvalidCastException("Cannot cast Function to other types");
    }
}