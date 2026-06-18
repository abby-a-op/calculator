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

    public IToken ApplyOperation(IToken? rhs, OperatorType op)
    {
        if (rhs == null) throw new InvalidOperationException($"Operation {op} is invalid on text");

        if (rhs.Type == TokenType.Text && op == OperatorType.Plus)
        {
            string a = Value;
            string b = ((Text)rhs).Value;

            return new Text(a+b);
        }

        throw new InvalidOperationException($"Operation {op} is invalid on text");
    }
}