using System.Runtime.Serialization;
using System.Text;

namespace Calculator;

// Class for the number theory commands
public static class NumberTheory
{
    public static Text NumRand(int a, int x, int c, int m)
    {
        StringBuilder stringBuilder = new StringBuilder();
        int i = 0;

        int seed = x;
        do
        {
            int newSeed = (a * seed + c) % m;
            if (newSeed < 0) newSeed += m;
        
            stringBuilder.AppendLine($"{newSeed} = ({a}*{seed} + {c}) mod {m}");

            seed = newSeed;
        } while (seed != x && i < m);
        stringBuilder.Remove(stringBuilder.Length - 1, 1);

        return new Text(stringBuilder.ToString());
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

    public static Text NumCheckDigit(Text digitsToken)
    {
        string digits = digitsToken.Value;
        int result = -1;
        // UPC
        if (digits.Length == 11)
        {
            int sum = 0;

            for (int i = 0; i < digits.Length; i++)
            {
                int digit = int.Parse(digits[i].ToString());

                sum += digit * (i % 2 == 0 ? 3 : 1);
            }

            result = (-sum) % 10;
            if (result < 0) result += 10;
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

            result = sum % 11;
            if (result < 0) result += 11;
        }

        // EAN-13
        if (digits.Length == 12)
        {
            int sum = 0;

            for (int i = 0; i < digits.Length; i++)
            {
                int digit = int.Parse(digits[i].ToString());

                sum += digit * (i % 2 == 0 ? 3 : 1);
            }

            result = (-sum) % 10;
            if (result < 0) result += 10;
        }

        return new Text(digits + result.ToString());
    }
}