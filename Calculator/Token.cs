namespace Calculator;

[Flags]
public enum TokenType
{
    Invalid = 0,
    Real = 1,
    Operator = 2,
    Function = 4,
    Text = 8,
    Variable = 16,
    Integer = 32,
    Vec2 = 64,
    Matrix = 128,
    Operand = Integer | Real | Vec2,
}