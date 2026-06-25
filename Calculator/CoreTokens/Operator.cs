namespace Calculator;

// Token for operator
public struct Operator: IToken
{
    public TokenType Type => TokenType.Operator;

    public OperatorType Value { get; set; }

    public Operator(char symbol)
    {
        Value = symbol switch
        {
            '+' => OperatorType.Plus,
            '-' => OperatorType.Minus,
            '*' => OperatorType.Multiply,
            '/' => OperatorType.Divide,
            '%' => OperatorType.Modulo,
            '^' => OperatorType.Exponentiate,
            '(' => OperatorType.OpeningBracket,
            ')' => OperatorType.ClosingBracket,
            _ => throw new ArgumentException($"{symbol} is not a valid operator")
        };
    }

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

    public string Output() => Value.ToString();
}

// An enum for checking the type of operator, useful as some operators share symbols (unary and binary minus, for example)
public enum OperatorType
{
    Plus,
    Minus,
    Multiply,
    Divide,
    Exponentiate,
    Modulo,
    OpeningBracket,
    ClosingBracket,
    Equals,
    UnaryPlus,
    UnaryMinus,

}
