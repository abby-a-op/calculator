namespace Calculator;

public class Interpreter
{
    public string Command = "";

    public static readonly string[] CommandNames = new string[]
    {
        "numRand",
        "caesarEn",
        "caesarDe",
        "affineEn",
        "affineDe",
        "isPrime",
        "vec"
    };

    private InfixEvaluator _Evaluator = new InfixEvaluator();

    const string DIGITS = "0123456789";
    const string OPERATORS = "+-/*()^%=";

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
            TokenType.Variable => new Variable(tokenText),
            _ => throw new ArgumentException("Unable to create token of type " + tokenType)
        };
    }

    public IToken[] Tokenise()
    {
        List<IToken> tokens = new List<IToken>();
        string currentTokenText = "";
        TokenType currentTokenType = TokenType.Invalid;

        IToken currentTokenData;

        foreach (var c in Command)
        {
            if (c == ' ' || c == ',')
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
                currentCharTokenType = currentTokenType == TokenType.Real ? TokenType.Real : TokenType.Integer;
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
                currentCharTokenType = TokenType.Variable;
            }
            
            if (currentTokenType == TokenType.Invalid)
            {
                currentTokenType = currentCharTokenType;
            }
            else if (currentCharTokenType != currentTokenType || currentCharTokenType == TokenType.Operator)
            {
                if (currentTokenType == TokenType.Variable && (Functions.FunctionNames.Contains(currentTokenText) || CommandNames.Contains(currentTokenText)))
                {
                    currentTokenType = TokenType.Function;
                }
                
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

        // Looks for a place where an implicit multiplication is happening and inserts a multiplication operator
        // e.g 2(2-3) becomes 2*(2-3)
        for (int i = 0; i < tokens.Count-1; i++)
        {
            if ((tokens[i].Type & TokenType.Operand) == TokenType.Invalid && (tokens[i].Type != TokenType.Operator ||
                                                                              ((Operator)tokens[i]).Value !=
                                                                              OperatorType.ClosingBracket))
            {
                continue;
            }
            if (tokens[i+1].Type == TokenType.Variable
                || tokens[i + 1].Type == TokenType.Function && ((Function)tokens[i + 1]).Value != "!"
                || tokens[i + 1].Type == TokenType.Operator && ((Operator)tokens[i+1]).Value == OperatorType.OpeningBracket)
            {
                tokens.Insert(i + 1, multiplication);
            }
        }
        
        // Next two blocks are to allow input of negative numbers by implementing unary operations

        // Checks to see if the first token is a plus or minus, and converts them to the unary equivalent
        if (tokens[0].Type == TokenType.Operator)
        {
            Operator currentToken = (Operator)tokens[0];
            if (currentToken.Value == OperatorType.Plus) currentToken.Value = OperatorType.UnaryPlus;
            if (currentToken.Value == OperatorType.Minus) currentToken.Value = OperatorType.UnaryMinus;
        
            tokens[0] = currentToken;
        }
        
        // Looks for any places where a plus or minus follows an operator (BUT NOT CLOSING BRACKET, AS LOGICALLY A BRACKET BLOCK IS AN OPERAND)
        // and replaces the plus or minus with unary equivalent
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
        
        // Substitutes value of variables
        for (int i = 0; i < tokens.Count; i++)
        {
            if (tokens[i].Type != TokenType.Variable)
            {
                continue;
            }
            
            string variableName = ((Variable)tokens[i]).Name;

            if (Variables.ContainsKey(variableName))
            {
                tokens[i] = Variables[variableName];
            }
        }

        return tokens.ToArray();
    }

    public string Run()
    {
        IToken[] tokens = Tokenise();

        foreach (var token in tokens)
        {
            Console.Write(token + " ");
        }
        Console.WriteLine();

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

                    return NumberTheory.NumRand(a, x, c, m);
                }
                case "isPrime":
                {
                    int a = ((Integer)tokens[1]).Value;

                    return NumberTheory.IsPrime(a).ToString();
                }
                case "numCheckDigit":
                {
                    string digits = ((Integer)tokens[1]).Output();

                    return NumberTheory.NumCheckDigit(digits);
                }
                case "vec":
                {
                    Variable @var = (Variable)tokens[1];

                    if (((Operator)tokens[2]).Value != OperatorType.Equals)
                    {
                        throw new FormatException();
                    }
                    
                    Real x = (Real)tokens[4];
                    Real y = (Real)tokens[5];
                    
                    Vec2 vec = new Vec2(x.Value, y.Value);
                    Variables[@var.Name] = vec;
                    
                    return vec.ToString();
                    break;
                }
                default:
                {
                    _Evaluator.Expression = tokens;
                    return _Evaluator.Evaluate().Output();
                }
            }
        }
        
        if (tokens[0].Type == TokenType.Variable && tokens[1].Type == TokenType.Operator && ((Operator)tokens[1]).Value == OperatorType.Equals)
        {
            Variable variable = (Variable)tokens[0];
            
            IToken[] expressionForVariable = new IToken[tokens.Length - 2];
            Array.Copy(tokens, 2, expressionForVariable, 0, expressionForVariable.Length);
            _Evaluator.Expression = tokens;
            
            Variables[variable.Name] = _Evaluator.Evaluate();
            
            return Variables[variable.Name].Output();
        }
        
        if (tokens[0].Type != TokenType.Text)
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