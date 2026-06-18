namespace Calculator;

public struct Variable: IToken
{
    public string Name;
    private readonly Dictionary<string, IToken> VariableTable;

    public Variable(string name, Dictionary<string, IToken> varTable)
    {
        Name = name;
        VariableTable = varTable;
    }
    
    public TokenType Type => TokenType.Variable;

    public IToken ApplyOperation(IToken? rhs, OperatorType op)
    {
        if (rhs == null) throw new InvalidOperationException($"Operation {op} is not valid on unitialised variables");

        if (op == OperatorType.Equals)
        {
            VariableTable[Name] = rhs;
            return rhs;
        }

        throw new InvalidOperationException("Only equals is valid on uninitialised variables");
    }
    
    public string Output() => Name;
}