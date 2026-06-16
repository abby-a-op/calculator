namespace Calculator;

public interface IToken
{
    public IToken ApplyOperation(IToken? rhs, OperatorType op);

    public TokenType Type { get; }

    public string Output();
}