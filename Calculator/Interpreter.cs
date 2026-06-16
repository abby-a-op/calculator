using System.Net.Sockets;

namespace Calculator;

public class Interpreter
{
    public string Command = "";

    private InfixEvaluator _Evaluator = new InfixEvaluator();

    const string DIGITS = "0123456789";
    const string OPERATORS = "+-/*()^%";

    public Dictionary<string, IToken> Variables = new Dictionary<string, IToken>();

    private IToken ParseTokenText(string tokenText, TokenType tokenType)
    {
        return tokenType switch
        {
            TokenType.Integer => new Integer(int.Parse(tokenText)),
            TokenType.Text => new Text(tokenText),
            TokenType.Operator => new Operator((OperatorType)tokenText[0]),
            TokenType.Function => new Function(tokenText),
            TokenType.Real => new Real(double.Parse(tokenText)),
            _ => throw new Exception()
        };
    }

    public IToken[] Tokenise()
    {
        List<IToken> tokens = new List<IToken>();
        string currentTokenText = "";
        TokenType currentTokenType = TokenType.Invalid;

        IToken currentTokenData;

        for (int i=0; i<Command.Length; i++)
        {
            char c = Command[i];

            if (c == ' ')
            {
                if (currentTokenType == TokenType.Integer)
                {
                    currentTokenData = ParseTokenText(currentTokenText, currentTokenType);
                    tokens.Add(currentTokenData);
                    currentTokenText = "";
                }
                if (currentTokenType != TokenType.Text)
                {
                    continue;
                }
            }

            TokenType currentCharTokenType;

            if (DIGITS.Contains(c))
            {
                if (currentTokenType == TokenType.Real)
                {
                    currentCharTokenType = TokenType.Real;
                }
                else
                {
                    currentCharTokenType = TokenType.Integer;
                }
            }
            else if (OPERATORS.Contains(c))
            {
                currentCharTokenType = TokenType.Operator;
            }
            else if (c == '"')
            {
                if (currentTokenType == TokenType.Text)
                {
                    IToken data = ParseTokenText(currentTokenText, currentTokenType);
                    tokens.Add(data);
                    currentTokenText = "";
                    continue;
                }
                currentCharTokenType = TokenType.Text;
            }
            else if (c == '.' && currentTokenType == TokenType.Integer)
            {
                currentTokenType = TokenType.Real;
                currentCharTokenType = TokenType.Real;
            }
            else if (currentTokenType == TokenType.Text)
            {
                currentCharTokenType = TokenType.Text;
            }
            else
            {
                currentCharTokenType = TokenType.Function;
            }
            
            if (currentTokenType == TokenType.Invalid)
            {
                currentTokenType = currentCharTokenType;
            }
            else if (currentCharTokenType != currentTokenType || currentCharTokenType == TokenType.Operator)
            {
                currentTokenData = ParseTokenText(currentTokenText, currentTokenType);

                tokens.Add(currentTokenData);
                currentTokenType = currentCharTokenType;
                currentTokenText = "";
            }
            
            if (c != '"')
            {
                currentTokenText += c;
            }
        }

        currentTokenData = ParseTokenText(currentTokenText, currentTokenType);
        tokens.Add(currentTokenData);

        Operator multiplication = new Operator(OperatorType.Multiply);
        Integer zero = new Integer(0);

        for (int i = 0; i < tokens.Count-1; i++)
        {
            if ((tokens[i].Type & TokenType.Operand) != TokenType.Invalid || tokens[i].Type == TokenType.Operator && ((Operator)tokens[i]).Value == OperatorType.ClosingBracket)
            {
                if (
                    tokens[i + 1].Type == TokenType.Function
                    || tokens[i + 1].Type == TokenType.Operator && ((Operator)tokens[i+1]).Value == OperatorType.OpeningBracket)
                {
                    tokens.Insert(i + 1, multiplication);
                }
            }
        }

        if (tokens[0].Type == TokenType.Operator)
        {
            Operator currentToken = (Operator)tokens[0];
            if (currentToken.Value == OperatorType.Plus) currentToken.Value = OperatorType.UnaryPlus;
            if (currentToken.Value == OperatorType.Minus) currentToken.Value = OperatorType.UnaryMinus;

            tokens[0] = currentToken;
        }

        for (int i = 0; i < tokens.Count-1; i++)
        {
            if (tokens[i].Type == TokenType.Operator && ((Operator)tokens[i]).Value != OperatorType.ClosingBracket)
            {
                if (tokens[i+1].Type == TokenType.Operator)
                {
                    Operator currentToken = (Operator)tokens[i+1];
                    if (currentToken.Value == OperatorType.Plus) currentToken.Value = OperatorType.UnaryPlus;
                    if (currentToken.Value == OperatorType.Minus) currentToken.Value = OperatorType.UnaryMinus;

                    tokens[i+1] = currentToken;
                }
            }
        }

        return tokens.ToArray();
    }

    public string Run()
    {
        IToken[] tokens = Tokenise();

        if (tokens[0].Type == TokenType.Function)
        {
            switch (((Function)tokens[0]).Value)
            {
                case "caesarEn":
                {
                    string plaintext = ((Text)tokens[1]).Value;
                    return Encryption.CaesarEn(plaintext);
                }
                case "caesarDe":
                {
                    string plaintext = ((Text)tokens[1]).Value;
                    return Encryption.CaesarDe(plaintext);
                }
                case "numRand":
                {
                    int a, x, c, m;

                    a = ((Integer)tokens[1]).Value;
                    x = ((Integer)tokens[2]).Value;
                    c = ((Integer)tokens[3]).Value;
                    m = ((Integer)tokens[4]).Value;

                    return NumberTheory.NumRand(a, x, c, m).ToString();
                }
                case "isPrime":
                {
                    int a = ((Integer)tokens[1]).Value;

                    return NumberTheory.IsPrime(a).ToString();
                }
                case "numCheckDigit":
                {
                    string digits = ((Integer)tokens[1]).Output();

                    return NumberTheory.NumCheckDigit(digits).ToString();
                }
                default:
                {
                    _Evaluator.Expression = tokens;
                    return _Evaluator.Evaluate().Output();
                }
            }
        }
        else if (tokens[0].Type != TokenType.Text)
        {
            _Evaluator.Expression = tokens;
            return _Evaluator.Evaluate().Output();
        }
        else
        {
            return "";
        }
    }
}