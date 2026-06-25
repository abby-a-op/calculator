namespace Calculator;

public static class Functions
{
    public static IToken EvaluateFunction(Function func, IToken x)
    {
        return func.Name switch
        {
            "sqrt" => Sqrt(x),
            "!" => Factorial(x),
            _ => new Text($"No function named {func}")
        };
    }

    public static readonly string[] FunctionNames = new string[]
    {
        "!",
        "sqrt"
    };

    public static IToken Factorial(IToken input)
    {
        int n = ((Integer)input.CastTo(TokenType.Integer)).Value;

        Integer output = new Integer();
        output.Value = Factorial(n);

        if (output.Value == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(@input), "Result exceeded 32 bit integer limit");
        }

        return output;
    }

    static int Factorial(int n)
    {
        if (n == 1 || n == 0)
        {
            return 1;
        }

        if (n < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n), "n! is only defined for n>=0");
        }

        return n * Factorial(n-1);
    }

    public static IToken Sqrt(IToken input)
    {
        double x = ((Real)input.CastTo(TokenType.Real)).Value;
        return new Real(Math.Sqrt(x));
    }
}