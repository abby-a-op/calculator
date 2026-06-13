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

        Token currentToken = new Token();

        for (int i=0; i<Command.Length; i++)
        {
            char c = Command[i];

            if (c == ' ')
            {
                continue;
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
            else
            {
                currentCharTokenType = TokenType.Function;
            }
            if (i == 0)
            {
                currentToken.Type = currentCharTokenType;
            }
            else if (currentCharTokenType != currentToken.Type || currentCharTokenType == TokenType.Operator)
            {
                tokens.Add(currentToken);
                currentToken = new Token();
                currentToken.Type = currentCharTokenType;
            }

            currentToken.Value += c;
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
        string[] args = Command.Split(' ');

        switch (args[0])
        {
            case "numRand":
            {
                int a, x, c, m;
                a = int.Parse(args[1]);
                x = int.Parse(args[2]);
                c = int.Parse(args[3]);
                m = int.Parse(args[4]);

                return NumberTheory.NumRand(a, x, c, m).ToString();
            }
            case "isPrime":
            {
                int a = int.Parse(args[1]);

                return NumberTheory.IsPrime(a).ToString();
            }
            case "numCheckDigit":
            {
                string digits = args[1];

                return NumberTheory.NumCheckDigit(digits).ToString();
            }
            default:
                _Evaluator.Expression = Command;
                return _Evaluator.Evaluate().ToString();
        }
    }
}