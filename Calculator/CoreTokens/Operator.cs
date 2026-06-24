namespace Calculator;

public struct Operator: IToken
{
    public TokenType Type => TokenType.Operator;

    public OperatorType Value { get; set; }

    public Operator(OperatorType value)
    {
        Value = value;
    }

    public IToken ApplyOperation(IToken? rhs, OperatorType op)
    {
        throw new InvalidOperationException("Cannot perform operation between two operations");
    }

    public IToken CastTo(TokenType castTo)
    {
        return castTo switch
        {
            TokenType.Operator => this,
            TokenType.Text => new Text(Output()),
            _ => throw new InvalidCastException("Cannot cast operator to other type")
        };
    }

    public string Output() => ((char)Value).ToString();
}

public enum OperatorType
{
    Plus = '+',
    Minus = '-',
    Multiply = '*',
    Divide = '/',
    Exponentiate = '^',
    Modulo = '%',
    OpeningBracket = '(',
    ClosingBracket = ')',
    Equals = '=',
    UnaryPlus = '$',
    UnaryMinus = '@',

}
