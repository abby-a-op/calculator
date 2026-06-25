namespace Calculator;

// Struct for uninitialised variable (initialised variables are replaced with their value during tokenisation)
// Used as it is useful to treat variables as a unique operand so that an expression such as "2a" is interpreted as "2*a"
public struct Variable: IToken
{
    public string Name;

    // The table that all variables are stored in, taken from interpreter
    private readonly Dictionary<string, IToken> VariableTable;

    public Variable(string name, Dictionary<string, IToken> varTable)
    {
        Name = name;
        VariableTable = varTable;
    }
    
    public TokenType Type => TokenType.Variable;

    // Allows assignment operation
    public IToken ApplyOperation(IToken? rhs, OperatorType op)
    {
        if (rhs == null) throw new InvalidOperationException($"Operation {op} is not valid on unitialised variables");

        if (op == OperatorType.Equals)
        {
            VariableTable[Name] = rhs;
            return rhs;
        }

        throw new InvalidOperationException($"Variable {Name} is uninitiliased, did you spell it correctly?");
    }

    public IToken CastTo(TokenType castTo)
    {
        if (castTo == Type) return this;

        throw new InvalidCastException("Cannot cast variable to " + castTo);
    }
    
    public string Output() => Name;
}