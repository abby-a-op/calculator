namespace Calculator;

public struct Variable: IToken
{
    public string Name;

    public Variable(string name)
    {
        Name = name;
    }
    
    public TokenType Type => TokenType.Variable;

    public IToken ApplyOperation(IToken? rhs, OperatorType op)
    {
        throw new InvalidOperationException();
    }
    
    public string Output() => "Should never execute";
}