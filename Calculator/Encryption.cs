namespace Calculator;

public static class Encryption
{
    const string LOWERCASE = "abcdefghijklmnopqrstuvwxyz";
    const string UPPERCASE = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

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
}