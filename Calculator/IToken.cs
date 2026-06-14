namespace Calculator;

public interface IToken
{
    public IToken ApplyOperation(IToken rhs, OperatorType op);

    static TokenType _Type { get; }

    public TokenType Type => _Type;

    public string Output();
}