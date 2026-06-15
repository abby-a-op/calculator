namespace Calculator;

[Flags]
public enum TokenType
{
    Invalid,
    Real,
    Operator,
    Function,
    Text,
    Variable,
    Integer,
    Operand = Integer | Real
}