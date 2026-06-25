namespace Calculator;

// Generic type for tokens (component parts of a user's input)
// A generic interface was used because it allowed for greater variation in the type of variables that could be used while also allowing greater consistency
public interface IToken
{
    public IToken ApplyOperation(IToken? rhs, OperatorType op);

    public TokenType Type { get; }

    public IToken CastTo(TokenType castTo);

    public string Output();
}

// Enum for specifying the type of a given token,
// Has the flags attribute so that multiple types can be grouped as simply "Operand"
[Flags]
public enum TokenType
{
    Undefined = 0,
    Real = 1,
    Operator = 2,
    Function = 4,
    Text = 8,
    Variable = 16,
    Integer = 32,
    Vec2 = 64,
    Matrix = 128,
    Line = 256,
    Operand = Integer | Real | Vec2 | Variable | Matrix | Text
}