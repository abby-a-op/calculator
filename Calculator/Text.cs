using System.Runtime.CompilerServices;

namespace Calculator;

public struct Text: IToken
{
    public string Value { get; set; }

    public TokenType Type => TokenType.Text;

    public Text(string value)
    {
        this.Value = value;
    }

    public string Output() => Value;

    public IToken ApplyOperation(IToken rhs, OperatorType op)
    {
        if (rhs.Type == TokenType.Text)
        {
            string a = Value;
            string b = ((Text)rhs).Value;

            return new Text(a+b);
        }

        throw new InvalidOperationException($"{Type} {(char)op} {rhs.Type} is invalid");
    }
}