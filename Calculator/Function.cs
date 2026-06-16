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

    public IToken ApplyOperation(IToken rhs, OperatorType op)
    {
        throw new InvalidOperationException($"{Type} {(char)op} {rhs.Type} is invalid");
    }
}