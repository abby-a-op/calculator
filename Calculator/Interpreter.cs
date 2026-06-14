using System.Net.Sockets;

namespace Calculator;

public class Interpreter
{
    public string Command = "";

    private InfixEvaluator _Evaluator = new InfixEvaluator();

    const string DIGITS = "0123456789.";
    const string OPERATORS = "+-/*()^%";

    public Token[] Tokenise()
    {
        List<Token> tokens = new List<Token>();

        Token currentToken = new Token()
        {
            Type = TokenType.Invalid
        };

        for (int i=0; i<Command.Length; i++)
        {
            char c = Command[i];

            if (c == ' ')
            {
                if (currentToken.Type == TokenType.Number)
                {
                    tokens.Add(currentToken);
                    currentToken = new Token()
                    {
                        Type = TokenType.Invalid
                    };
                }
                else if (currentToken.Type != TokenType.Text)
                {
                    continue;
                }
            }

            TokenType currentCharTokenType;


            if (DIGITS.Contains(c))
            {
                currentCharTokenType = TokenType.Number;
            }
            else if (OPERATORS.Contains(c))
            {
                currentCharTokenType = TokenType.Operator;
            }
            else if (c == '"')
            {
                if (currentToken.Type == TokenType.Text)
                {
                    tokens.Add(currentToken);
                    currentToken = new Token()
                    {
                        Type = TokenType.Invalid
                    };
                    continue;
                }
                currentCharTokenType = TokenType.Text;
            }
            else if (currentToken.Type == TokenType.Text)
            {
                currentCharTokenType = TokenType.Text;
            }
            else
            {
                currentCharTokenType = TokenType.Function;
            }
            
            if (currentToken.Type == TokenType.Invalid)
            {
                currentToken.Type = currentCharTokenType;
            }
            else if (currentCharTokenType != currentToken.Type || currentCharTokenType == TokenType.Operator)
            {
                tokens.Add(currentToken);
                currentToken = new Token();
                currentToken.Type = currentCharTokenType;
            }
            
            if (c != '"')
            {
                currentToken.Value += c;
            }
        }

        tokens.Add(currentToken);

        Token multiplication = new Token()
        {
            Type = TokenType.Operator,
            Value = "*"
        };

        Token zero = new Token()
        {
            Type = TokenType.Number,
            Value = "0"
        };

        if (tokens[0].Value == "-" || tokens[0].Value == "+")
        {
            tokens.Insert(0, zero);
        }

        for (int i = 0; i < tokens.Count-1; i++)
        {
            if (tokens[i].Type == TokenType.Number || tokens[i].Value == ")")
            {
                if (tokens[i + 1].Type == TokenType.Function || tokens[i+1].Value == "(")
                {
                    tokens.Insert(i + 1, multiplication);
                }
            }
        }

        for (int i = 0; i < tokens.Count-1; i++)
        {
            if (tokens[i].Type == TokenType.Operator && tokens[i].Value != ")")
            {
                if (tokens[i + 1].Value == "+" || tokens[i + 1].Value == "-")
                {
                    tokens.Insert(i + 1, zero);
                }
            }
        }

        return tokens.ToArray();
    }

    public string Run()
    {
        Token[] tokens = Tokenise();

        if (tokens[0].Type == TokenType.Function)
        {
            switch (tokens[0].Value)
            {
                case "caesarEn":
                {
                    string plaintext = tokens[1].Value;
                    return Encryption.CaesarEn(plaintext);
                }
                case "caesarDe":
                {
                    string plaintext = tokens[1].Value;
                    return Encryption.CaesarDe(plaintext);
                }
                case "numRand":
                {
                    int a, x, c, m;

                    a = int.Parse(tokens[1].Value);
                    x = int.Parse(tokens[2].Value);
                    c = int.Parse(tokens[3].Value);
                    m = int.Parse(tokens[4].Value);

                    return NumberTheory.NumRand(a, x, c, m).ToString();
                }
                case "isPrime":
                {
                    int a = int.Parse(tokens[1].Value);

                    return NumberTheory.IsPrime(a).ToString();
                }
                case "numCheckDigit":
                {
                    string digits = tokens[1].Value;

                    return NumberTheory.NumCheckDigit(digits).ToString();
                }
                default:
                {
                    return "";
                }
            }
        }
        else if (tokens[0].Type != TokenType.Text)
        {
            _Evaluator.Expression = tokens;
            return _Evaluator.Evaluate().ToString();
        }
        else
        {
            return "";
        }
    }
}