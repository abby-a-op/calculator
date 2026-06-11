namespace Calculator;

public static class Functions
{
    public static double EvaluateFunction(string name, double x)
    {
        return name switch
        {
            "sqrt" => Sqrt(x),
            "!" => Factorial(x),
            _ => -1
        };
    }

    public static double Factorial(double x)
    {
        int n = (int)Math.Floor(x);

        return Factorial(n);
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

    public static double Sqrt(double x) => Math.Sqrt(x);
}