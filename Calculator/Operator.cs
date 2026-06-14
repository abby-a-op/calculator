namespace Calculator;

public struct Operator: IToken
{
    private static TokenType _Type => TokenType.Operator;

    public OperatorType Value;

    public Operator(OperatorType value)
    {
        Value = value;
    }

    public IToken ApplyOperation(IToken rhs, OperatorType op)
    {
        throw new InvalidOperationException("Cannot perform operation between two operations");
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
    ClosingBracket = ')'
}
