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
        "vec",
        "addVec",
        "subVec",
        "scalVec",
        "dotVec",
        "Line",
        "lengthLine",
        "midpointLine",
        "gradientLine",
        "mat",
        "addMat",
        "dotMat",
        "scalMat",
        "detMat",
        "invMat"
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
            TokenType.Variable => new Variable(tokenText, Variables),
            _ => throw new ArgumentException("Unable to create token of type " + tokenType)
        };
    }

    // Breaks up the user's input into the various "tokens" and describes the type of them
    // For example, "2+2!" becomes 2 (Integer), Plus (Operator), 2 (Integer), ! (Function).
    // An explicit tokenisation method was necessary as a raw string has very little semantic information,
    // and would make it difficult to tell when a number ends and an operator begins
    public IToken[] Tokenise()
    {
        List<IToken> tokens = new List<IToken>();

        // The text for the current token. Parsed when a new token starts
        string currentTokenText = "";

        // At the first iteration, the
        TokenType currentTokenType = TokenType.Undefined;

        IToken currentTokenData;

        // Iterates through characters in the command
        foreach (var c in Command)
        {
            // Spaces between operators should be ignored
            if (c == ' ' || c == ',')
            {
                // If the current token is an operand, a space after means the token is complete and the next iteration should begin
                if ((currentTokenType & TokenType.Operand) != TokenType.Undefined)
                {
                    currentTokenData = CompleteToken(ref currentTokenText, ref currentTokenType);
                    tokens.Add(currentTokenData);

                    // As the type of the next token has not been determined yet, set it to undefined
                    currentTokenType = TokenType.Undefined;
                }

                // If the current token is text, add the character
                else if (currentTokenType == TokenType.Text)
                {
                    currentTokenText += c;
                }

                continue;
            }

            // The token type that corresponds to the current character
            // If the current character's type is different to the current token's, a new token has started
            TokenType currentCharTokenType;

            // Double quotes means start of text, but if the current token is already text, it signifies the end of the token
            if (c == '"')
            {
                if (currentTokenType == TokenType.Text)
                {
                    IToken data = ParseTokenText(currentTokenText, currentTokenType);
                    tokens.Add(data);
                    
                    continue;
                }
                else
                {
                    currentCharTokenType = TokenType.Text;
                }
            }
            // If the current token is text and the new character isn't double quotes, the token has not ended
            else if (currentTokenType == TokenType.Text)
            {
                currentCharTokenType = TokenType.Text;
            }
            else if (DIGITS.Contains(c))
            {
                // If the interpreter has seen a decimal place in the current token, the token type is a real
                // If it hasn't, it's an integer
                currentCharTokenType = currentTokenType == TokenType.Real ? TokenType.Real : TokenType.Integer;
            }
            else if (OPERATORS.Contains(c))
            {
                currentCharTokenType = TokenType.Operator;
            }
            // A decimal point in a number means the number is a real, so the token type is changed
            else if (c == '.' && currentTokenType == TokenType.Integer)
            {
                currentTokenType = TokenType.Real;
                currentCharTokenType = TokenType.Real;
            }

            // For every other case, assume it is a variable (functions are determined as the token text is parsed)
            else
            {
                currentCharTokenType = TokenType.Variable;
            }
            
            // If the token type is currently unset, it is the start of a new token,
            // so set the current token type to the current character
            if (currentTokenType == TokenType.Undefined)
            {
                currentTokenType = currentCharTokenType;
            }

            // If a new token has started, parse the current token and add it to the list
            // The new token will have the type of the current character
            else if (currentCharTokenType != currentTokenType || currentCharTokenType == TokenType.Operator)
            {
                currentTokenData = CompleteToken(ref currentTokenText, ref currentTokenType);
                tokens.Add(currentTokenData);

                currentTokenType = currentCharTokenType;
            }
            
            // For every character other than a double quote, add the current character to the token text
            if (c != '"')
            {
                currentTokenText += c;
            }
        }

        // Parses the last token
        currentTokenData = CompleteToken(ref currentTokenText, ref currentTokenType);
        tokens.Add(currentTokenData);

        // Looks for a place where an implicit multiplication is happening and inserts a multiplication operator
        // e.g 2(2-3) becomes 2*(2-3)
        for (int i = 0; i < tokens.Count-1; i++)
        {
            if ((tokens[i].Type & TokenType.Operand) == TokenType.Undefined && (tokens[i].Type != TokenType.Operator ||
                                                                              ((Operator)tokens[i]).Value !=
                                                                              OperatorType.ClosingBracket))
            {
                continue;
            }
            if (tokens[i+1].Type == TokenType.Variable
                || tokens[i + 1].Type == TokenType.Function && ((Function)tokens[i + 1]).Value != "!"
                || tokens[i + 1].Type == TokenType.Operator && ((Operator)tokens[i+1]).Value == OperatorType.OpeningBracket)
            {
                tokens.Insert(i + 1, new Operator(OperatorType.Multiply));
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

    private IToken CompleteToken(ref string currentTokenText, ref TokenType currentTokenType)
    {
        IToken currentTokenData;

        if (currentTokenType == TokenType.Variable && (Functions.FunctionNames.Contains(currentTokenText) || CommandNames.Contains(currentTokenText)))
        {            
            currentTokenType = TokenType.Function;
        }
        currentTokenData = ParseTokenText(currentTokenText, currentTokenType);
        currentTokenText = "";

        return currentTokenData;
    }

    public IToken Run()
    {
        IToken[] tokens = Tokenise();

        if (tokens[0].Type == TokenType.Function)
        {
            switch (((Function)tokens[0]).Value)
            {
                case "caesarEn":
                {
                    Text plain = (Text)tokens[1].CastTo(TokenType.Text);
                    return Encryption.CaesarEn(plain);
                }
                case "caesarDe":
                {
                    Text cipher = (Text)tokens[1].CastTo(TokenType.Text);
                    return Encryption.CaesarDe(cipher);
                }
                case "numRand":
                {
                    int a, x, c, m;

                    a = ((Integer)tokens[1].CastTo(TokenType.Integer)).Value;
                    x = ((Integer)tokens[2].CastTo(TokenType.Integer)).Value;
                    c = ((Integer)tokens[3].CastTo(TokenType.Integer)).Value;
                    m = ((Integer)tokens[4].CastTo(TokenType.Integer)).Value;

                    return NumberTheory.NumRand(a, x, c, m);
                }
                case "isPrime":
                {
                    int a = ((Integer)tokens[1].CastTo(TokenType.Integer)).Value;

                    return new Text(NumberTheory.IsPrime(a).ToString());
                }
                case "numCheckDigit":
                {
                    Text digits = (Text)tokens[1].CastTo(TokenType.Text);

                    return NumberTheory.NumCheckDigit(digits);
                }
                case "vec":
                {
                    Variable @var = (Variable)tokens[1];
                    
                    Real x = (Real)tokens[4].CastTo(TokenType.Real);
                    Real y = (Real)tokens[5].CastTo(TokenType.Real);
                    
                    Vec2 vec = new Vec2(x.Value, y.Value);
                    Variables[@var.Name] = vec;
                    
                    return vec;
                }
                case "addVec":
                {
                    Vec2 a = (Vec2)tokens[1].CastTo(TokenType.Vec2);
                    Vec2 b = (Vec2)tokens[2].CastTo(TokenType.Vec2);

                    return a.Add(b);
                }
                case "subVec":
                {
                    Vec2 a = (Vec2)tokens[1].CastTo(TokenType.Vec2);
                    Vec2 b = (Vec2)tokens[2].CastTo(TokenType.Vec2);

                    return a.Minus(b);
                }
                case "dotVec":
                {
                    Vec2 a = (Vec2)tokens[1].CastTo(TokenType.Vec2);
                    Vec2 b = (Vec2)tokens[2].CastTo(TokenType.Vec2);

                    return a.Dot(b);
                }
                case "scalVec":
                {
                    Real s = (Real)tokens[1].CastTo(TokenType.Real);
                    Vec2 a = (Vec2)tokens[2].CastTo(TokenType.Vec2);

                    return a.Scale(s.Value);
                }
                case "Line":
                {
                    Variable @var = (Variable)tokens[1];

                    Real x1 = (Real)tokens[4].CastTo(TokenType.Real);
                    Real y1 = (Real)tokens[5].CastTo(TokenType.Real);
                    Real x2 = (Real)tokens[6].CastTo(TokenType.Real);
                    Real y2 = (Real)tokens[7].CastTo(TokenType.Real);

                    Variables[@var.Name] = new Line(new Vec2(x1.Value, y1.Value), new Vec2(x2.Value, y2.Value));

                    return Variables[@var.Name];
                }
                case "lengthLine":
                {
                    return ((Line)tokens[1].CastTo(TokenType.Line)).Length();
                }
                case "midpointLine":
                {
                    return ((Line)tokens[1].CastTo(TokenType.Line)).Midpoint();
                }
                case "gradientLine":
                {
                    return ((Line)tokens[1].CastTo(TokenType.Line)).Gradient();   
                }
                case "mat":
                {
                    Variable @var = (Variable)tokens[1];

                    Real a = (Real)tokens[4].CastTo(TokenType.Real);
                    Real b = (Real)tokens[5].CastTo(TokenType.Real);
                    Real c = (Real)tokens[6].CastTo(TokenType.Real);
                    Real d = (Real)tokens[7].CastTo(TokenType.Real);

                    Variables[@var.Name] = new Matrix(a.Value, b.Value, c.Value, d.Value);

                    return Variables[@var.Name];
                }
                case "addMat":
                {
                    Matrix a = (Matrix)tokens[1].CastTo(TokenType.Matrix);
                    Matrix b = (Matrix)tokens[2].CastTo(TokenType.Matrix);

                    return a.Add(b);       
                }
                case "dotMat":
                {
                    Matrix a = (Matrix)tokens[1].CastTo(TokenType.Matrix);
                    Matrix b = (Matrix)tokens[2].CastTo(TokenType.Matrix);

                    return a.Dot(b);       
                }
                case "scalMat":
                {
                    Real s = (Real)tokens[1].CastTo(TokenType.Real);
                    Matrix a = (Matrix)tokens[2].CastTo(TokenType.Matrix);

                    return a.Scale(s);       
                }
                case "detMat":
                {
                    Matrix a = (Matrix)tokens[1].CastTo(TokenType.Matrix);

                    return a.Det();
                }
                case "invMat":
                {
                    Matrix a = (Matrix)tokens[1].CastTo(TokenType.Matrix);

                    return a.Inv();
                }
                default:
                {
                    _Evaluator.Expression = tokens;
                    return _Evaluator.Evaluate();
                }
            }
        }
        
        if (tokens[0].Type != TokenType.Text)
        {
            _Evaluator.Expression = tokens;
            return _Evaluator.Evaluate();
        }
        else
        {
            return tokens[0];
        }
    }
}