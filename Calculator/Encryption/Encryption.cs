using System.Text;

namespace Calculator;

public static class Encryption
{
    const string LOWERCASE = "abcdefghijklmnopqrstuvwxyz";
    const string UPPERCASE = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    private static Dictionary<int, int> InvMod = new Dictionary<int, int>()
    {
        { 1, 1 },
        { 3, 9 },
        { 5, 21 },
        { 7, 15 },
        { 9, 3 },
        { 11, 19 },
        { 15, 7 },
        { 17, 23 },
        { 19, 11 },
        { 21, 5 },
        { 23, 17 },
        { 25, 25 }
    };

    public static int GetLetterNumber(char c, out bool isUpper)
    {
        isUpper = false;
        
        if (char.IsUpper(c))
        {
            isUpper = true;
            return c - 'A';
        }
        
        if (char.IsLower(c))
        {
            return c - 'a';
        }

        return -1;
    }

    public static char GetLetterFromNumber(int n, bool isUpper)
    {
        if (isUpper)
        {
            return (char)(n + 'A');
        }
        else
        {
            return (char)(n + 'a');
        }
    }

    private static char ModChar(char c, int divisor, bool uppercase)
    {
        if (uppercase)
        {
            while (!UPPERCASE.Contains(c))
            {
                c -= (char)divisor;
            }
        }
        else
        {
            while (!LOWERCASE.Contains(c))
            {
                c -= (char)divisor;
            }
        }

        return c;
    }

    public static Text CaesarDe(Text input)
    {
        string plaintext = "";
        string cipherText = input.Value;

        foreach (char c in cipherText)
        {
            char p;
            if (LOWERCASE.Contains(c))
            {
                p = (char)(c - 3);

                p = ModChar(p, 26, false);
            }
            else if (UPPERCASE.Contains(c))
            {
                p = (char)(c - 3);
                
                p = ModChar(p, 26, true);
            }
            else
            {
                p = c;
            }

            plaintext += p;
        }

        return new Text(plaintext);
    }
    
    public static Text CaesarEn(Text input)
    {
        string cipherText = "";
        string plaintext = input.Value;

        foreach (char p in plaintext)
        {
            char c;
            if (LOWERCASE.Contains(p))
            {
                c = (char)(p + 3);

                // Adding a check then subtracting is easier to me than calculating the mod and then adding the ASCII offset, and achieves the same result
                if (c > 'z')
                {
                    c -= 'a';
                }
            }
            else if (UPPERCASE.Contains(p))
            {
                c = (char)(p + 3);
                if (c > 'Z')
                {
                    c -= 'A';
                }
            }
            else
            {
                c = p;
            }

            cipherText += c;
        }

        return new Text(cipherText);
    }

    public static Text AffineEn(int a, int b, Text plaintextToken)
    {
        if (!InvMod.ContainsKey(a))
        {
            throw new ArgumentException($"{a} has no inverse mod 26");
        }

        string plaintext = plaintextToken.Value;

        string cipherText = "";

        foreach (char p in plaintext)
        {
            int n = GetLetterNumber(p, out bool isUpper);

            int cipherIndex = a * n + b;
            cipherIndex %= 26;

            if (cipherIndex < 0) cipherIndex += 26;
            
            char c = GetLetterFromNumber(cipherIndex, isUpper);

            cipherText += c;
        }

        return new Text(cipherText);
    }

    public static Text AffineDe(int a, int b, Text ciphertextToken)
    {
        if (!InvMod.ContainsKey(a))
        {
            throw new ArgumentException(a + " has no inverse mod 26");
        }

        string ciphertext = ciphertextToken.Value;
        string plaintext = "";

        foreach (char c in ciphertext)
        {
            int n = GetLetterNumber(c, out bool isUpper);
            
            int plainIndex = InvMod[a] * (n-b);
            plainIndex %= 26;

            if (plainIndex < 0) plainIndex += 26;

            char p = GetLetterFromNumber(plainIndex, isUpper);
            p = ModChar(p, 26, isUpper);

            plaintext += p;
        }

        return new Text(plaintext);
    }

    public static Text BruteAffine(Text ciphertextToken)
    {
        DateTime initial = DateTime.Now;
        StringBuilder stringBuilder = new StringBuilder();

        foreach (var kvp in InvMod)
        {
            int a = kvp.Key;
            for (int b=0; b<26; b++)
            {
                stringBuilder.Append('"');
                stringBuilder.Append(AffineDe(a, b, ciphertextToken).Value);
                stringBuilder.Append("\" a=");
                stringBuilder.Append(a);
                stringBuilder.Append(", b=");
                stringBuilder.Append(b);
                stringBuilder.Append('\n');
            }
        }
        DateTime final = DateTime.Now;
        TimeSpan timespanTaken = (final - initial).Duration();

        stringBuilder.Append($"\nCompleted in {timespanTaken.TotalMilliseconds} milliseconds");

        return new Text(stringBuilder.ToString());
    }
}