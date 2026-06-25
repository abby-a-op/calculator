namespace Calculator;

// Generic type for tokens (component parts of a user's input)
// A generic interface was used because it allowed for greater variation in the type of variables that could be used while also allowing greater consistency
public interface IToken
{
    // Allows operations to be performed on tokens
    // By having this be generic, it allows greater flexibility
    public IToken ApplyOperation(IToken? rhs, OperatorType op);

    // The type of this token, used as it is very often necessary to know what type of data is being manipulated
    public TokenType Type { get; }

    // Method for casting a token to a different type
    // Used because I was often needing to cast an inputed integer to a float to allow commands to accept integers
    public IToken CastTo(TokenType castTo);

    // The text representation of the token, useful for debugging
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