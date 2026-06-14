namespace Calculator;

public struct Function: IToken<string>
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
        if (rhs.Type == TokenType.Text)
        {
            string a = Value;
            string b = ((Function)rhs).Value;

            return new Function(a+b);
        }

        throw new InvalidOperationException($"{Type} {(char)op} {rhs.Type} is invalid");
    }
}