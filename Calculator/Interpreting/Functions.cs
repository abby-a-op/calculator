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
        if (input.Type == TokenType.Integer)
        {
            int n = ((Integer)input).Value;

            Integer output = new Integer();
            output.Value = Factorial(n);
            return output;
        }

        else if (input.Type == TokenType.Real)
        {
            double x = ((Real)input).Value;

            Integer output = new Integer();
            output.Value = Factorial((int)x);
            return output;
        }

        throw new InvalidOperationException("Cannot take factorial of " + input.Type);
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
        
        if (input.Type == TokenType.Real)
        {
            double x = ((Real)input).Value;
            return new Real(Math.Sqrt(x));
        }

        throw new InvalidOperationException("Cannot take square root of " + input.Type);
    }
}