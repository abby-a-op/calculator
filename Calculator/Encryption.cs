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
        { 11, 9 },
        { 15, 7 },
        { 17, 19 },
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

    public static string CaesarDe(string cipherText)
    {
        string plaintext = "";

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

        return plaintext;
    }
    
    public static string CaesarEn(string plaintext)
    {
        string cipherText = "";

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

        return cipherText;
    }

    public static string AffineEn(int a, int b, string plaintext)
    {
        if (!InvMod.ContainsKey(a))
        {
            return "Invalid value of a";
        }

        string cipherText = "";

        foreach (char p in plaintext)
        {
            int n = GetLetterNumber(p, out bool isUpper);

            int cipherIndex = a * n + b;
            
            char c = GetLetterFromNumber(cipherIndex, isUpper);
            c = ModChar(c, 26, isUpper);

            cipherText += c;
        }

        return cipherText;
    }

    public static string AffineDe(int a, int b, string ciphertext)
    {
        if (!InvMod.ContainsKey(a))
        {
            return "Invalid value of a";
        }

        string plaintext = "";

        foreach (char c in ciphertext)
        {
            int n = GetLetterNumber(c, out bool isUpper);
            
            int plainIndex = InvMod[a] * (n-b);

            char p = GetLetterFromNumber(plainIndex, isUpper);
            p = ModChar(p, 26, isUpper);

            plaintext += p;
        }

        return plaintext;
    }
}