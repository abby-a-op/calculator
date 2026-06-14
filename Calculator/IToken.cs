namespace Calculator;

public interface IToken
{
    public IToken ApplyOperation(IToken rhs, OperatorType op);

    public TokenType Type { get; }

    public object Value { get; set; }

    public string Output();
}

public interface IToken<T>: IToken
{
    public new T Value { get; set; }
    object IToken.Value { get => Value; set => Value = (T)value; }
}