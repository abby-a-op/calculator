using System.Text;

namespace Calculator;

// Contains code for the encryption section of the assignment
public static class Encryption
{
    const string LOWERCASE = "abcdefghijklmnopqrstuvwxyz";
    const string UPPERCASE = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    // Table for modular inverses mod 26
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

    // Gives the alphabetical index of a number and whether it's upper or lowercase
    // Used because the numeric value of chars is different to their alphabetical position
    public static int GetLetterNumber(char c, out bool isUpper)
    {
        isUpper = false;
        
        if (UPPERCASE.Contains(c))
        {
            isUpper = true;
            return c - 'A';
        }
        
        if (LOWERCASE.Contains(c))
        {
            return c - 'a';
        }

        return -1;
    }

    // Undoes the conversion from letter to number
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

    // Performs ROT-3 Caesar cipher decoding
    public static Text CaesarDe(Text input)
    {
        string plaintext = "";
        string cipherText = input.Value;

        foreach (char c in cipherText)
        {
            char p;
            int n = GetLetterNumber(c, out bool isUpper);
            if (n == -1)
            {
                p = c;
            }
            else
            {
                int pIndex = n - 3;
                pIndex %= 26;

                if (pIndex < 0) pIndex += 26;

                p = GetLetterFromNumber(pIndex, isUpper);
            }

            plaintext += p;
        }

        return new Text(plaintext);
    }

    // Performs ROT-3 Caesar cipher encoding
    public static Text CaesarEn(Text input)
    {
        string cipherText = "";
        string plaintext = input.Value;

        foreach (char p in plaintext)
        {
            char c;
            int n = GetLetterNumber(p, out bool isUpper);

            if (n == -1)
            {
                c = p;
            }
            else
            {
                int cIndex = n + 3;
                cIndex %= 25;

                c = GetLetterFromNumber(cIndex, isUpper);
            }

            cipherText += c;
        }

        return new Text(cipherText);
    }

    // Encodes a string using the affine cipher
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

            if (n == -1)
            {
                cipherText += p;
            }
            else
            {
                int cipherIndex = a * n + b;
                cipherIndex %= 26;

                if (cipherIndex < 0) cipherIndex += 26;
            
                char c = GetLetterFromNumber(cipherIndex, isUpper);

                cipherText += c;
            }
        }

        return new Text(cipherText);
    }

    // Decodes a string using the affine cipher
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

            if (n == -1)
            {
                plaintext += c;
            }
            else
            {
                int plainIndex = InvMod[a] * (n - b);
                plainIndex %= 26;

                if (plainIndex < 0) plainIndex += 26;

                char p = GetLetterFromNumber(plainIndex, isUpper);

                plaintext += p;
            }

        }

        return new Text(plaintext);
    }

    // Brute forces the decryption of an affine cipher by checking every possible key pair
    public static Text BruteAffine(Text ciphertextToken)
    {
        DateTime initial = DateTime.Now;
        StringBuilder stringBuilder = new StringBuilder();

        foreach (var kvp in InvMod)
        {
            int a = kvp.Key;
            for (int b=0; b<26; b++)
            {
                // Writes the output to a log
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