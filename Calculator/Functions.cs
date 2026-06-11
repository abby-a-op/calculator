namespace Calculator;

public static class Functions
{
    public static int EvaluateFunction(string name, int n)
    {
        return name switch
        {
            "sqrt" => Sqrt(n),
            "!" => Factorial(n)
        };
    }

    public static int Factorial(int n)
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

    public static int Sqrt(int n) => (int)Math.Sqrt(n);
}