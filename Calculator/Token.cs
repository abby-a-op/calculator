namespace Calculator;

public struct Token
{
    public string Value;
    public TokenType Type;
}

public enum TokenType
{
    Invalid = -1,
    Number,
    Operator,
    Function,
}