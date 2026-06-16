namespace Calculator;

public static class Functions
{
    public static IToken EvaluateFunction(Function func, IToken x)
    {
        return func.Value switch
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
        int n;
        if (input.Type == TokenType.Integer)
        {
            n = ((Integer)input).Value;

            Integer output = new Integer();
            output.Value = Factorial(n);
            return output;
        }

        return new Text("Invalid type for factorial");
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
        if (input.Type == TokenType.Integer)
        {
            int n = ((Integer)input).Value;
            return new Integer((int)Math.Sqrt(n));
        }
        
        return new Text("Invalid");
    }
}