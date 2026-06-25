using System.Runtime.CompilerServices;

namespace Calculator;

// Class for text tokens, used for encryption module
public struct Text: IToken
{
    public string Value { get; set; }

    public TokenType Type => TokenType.Text;

    public Text(string value)
    {
        this.Value = value;
    }

    public string Output() => Value;

    // Allows concatenation of strings
    public IToken ApplyOperation(IToken? rhs, OperatorType op)
    {
        if (rhs == null) throw new InvalidOperationException($"Operation {op} is invalid on text");

        if (op == OperatorType.Plus)
        {
            string a = Value;
            string b = ((Text)rhs.CastTo(TokenType.Text)).Value;

            return new Text(a+b);
        }

        throw new InvalidOperationException($"Operation {op} is invalid on text");
    }

    public IToken CastTo(TokenType castTo)
    {
        if (castTo == Type) return this;

        throw new InvalidCastException("Cannot cast text to " + castTo);
    }
}