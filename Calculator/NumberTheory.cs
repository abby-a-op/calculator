namespace Calculator;

public static class NumberTheory
{
    public static int NumRand(int a, int x, int c, int m)
    {
        int next = (a * x + c) % m;

        if (next < 0) next += m;

        return next;
    }

    public static bool IsPrime(int n)
    {
        if (n >= 10000)
        {
            throw new ArgumentOutOfRangeException();
        }

        int upperBound = (int)Math.Sqrt(n);

        for (int i=2; i<=upperBound; i++)
        {
            if (n % i == 0)
            {
                return false;
            }
        }

        return true;
    }

    public static int NumCheckDigit(string digits)
    {
        // UPC
        if (digits.Length == 11)
        {
            int sum = 0;

            for (int i = 0; i < digits.Length; i++)
            {
                int digit = int.Parse(digits[i].ToString());

                sum += digit % 2 == 0 ? 3 : 1;
            }

            int result = -sum % 10;
            if (result < 0) result += 10;

            return result;
        }

        // ISBN
        else if (digits.Length == 9)
        {
            int sum = 0;

            for (int i = 0; i < digits.Length; i++)
            {
                int digit = int.Parse(digits[i].ToString());

                sum += digit * (i+1);
            }

            int result = sum % 11;
            if (result < 0) result += 11;

            return result;
        }

        // EAN-13
        if (digits.Length == 12)
        {
            int sum = 0;

            for (int i = 0; i < digits.Length; i++)
            {
                int digit = int.Parse(digits[i].ToString());

                sum += digit % 2 == 0 ? 1 : 3;
            }

            int result = -sum % 10;
            if (result < 0) result += 10;

            return result;
        }

        return -1;
    }
}