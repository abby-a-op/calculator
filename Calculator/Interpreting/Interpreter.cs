namespace Calculator;

// Main class for the backend of the calculator
// Used for tokenising expressions and running commands
public class Interpreter
{
    public string Command = "";

    public static readonly string[] CommandNames = new string[]
    {
        "numRand",
        "caesarEn",
        "caesarDe",
        "bruteAffine",
        "affineEn",
        "affineDe",
        "numPrime",
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
        "invMat",
        "degToRad",
        "radToDeg",
        "help",
        "quit",
        "exit",
        "?"
    };

    private const string HELPTEXT = """
    ABBYKUS - Abby's Basic But Yuseful KalcUlator Software. Author - Abby Ashton

    Basic usage - Enter any basic mathematical expression to get the result.

    Commands:
    quit/exit - Closes the program

    ------------Geometry and vectors-----------
    radToDeg [real theta] - Converts an angle in radians to degrees
    degToRad [real theta] - Converts an angle in degrees to radians

    vec [name] ([real x], [real y]) - Creates a 2D vector and stores it under a
    addVec [vector a] [vector b] - Adds two 2D vectors together (equivilent to a+b)
    subVec [vector a] [vector b] - Subtracts two 2D vectors
    dotVec [vector a] [vector b] - Calculates the dot product of two 2D vectors
    scalVec [real s] [vector a] - Scales a vector by scalar s

    Line [name] ([real x1], [real y1], [real x2], [real y2]) - Creates a line from (x1, y1) to (x2, y2)
    lengthLine [line a] - Calculates the length of line a
    midpointLine [line a] - Calculates the midpoint of line a
    gradientLine [line a] - Calculates the slope of line a
    
    mat [name] ([real a], [real b], [real c], [real d]) - Creates a 2x2 matrix
    addMat [matrix a] [matrix b] - Adds two matrices together
    dotMat [matrix a] [matrix b] - Calculates the product of two matrices
    scalMat [real s] [matrix a] - Scales a matrix by s
    detMat [matrix a] - Calculates the determinate of a
    invMat [matrix a] - Calculates the inverse of a matrix

    -------Number theory and encryption--------
    numRand [int a] [int X] [int c] [int m] - Generates a random number using the linear congruential method
    numPrime [int a] - Returns true if a number less than or equal to 10000 is prime
    numCheckDigit [int digits] - Returns a complete barcode number for UPC, EAN-13, or ISBN barcodes
    
    caesarEn [text plaintext] - Returns a string encoded with the ROT-3 Caesar cipher
    caesarDe [text ciphertext] - Decodes a string encoded with the ROT-3 Caesar cipher

    affineEn [int a] [int b] [string plaintext] - Encrypts a string with the affine cipher, using a and b as the keys
    affineDe [int a] [int b] [string ciphertext] - Decrypts a string with the affine cipher, using a and b as the keys
    bruteAffine [string ciphertext] - Outputs all possible plaintext strings for a given ciphertext, including the keys
    """;

    private InfixEvaluator _Evaluator = new InfixEvaluator();

    const string DIGITS = "0123456789";
    const string OPERATORS = "+-/*()^%=";

    public Dictionary<string, IToken> Variables = new Dictionary<string, IToken>();

    // Method for converting the text for a token into an object for performing operations on
    private IToken ParseTokenText(string tokenText, TokenType tokenType)
    {
        return tokenType switch
        {
            TokenType.Integer => new Integer(int.Parse(tokenText)),
            TokenType.Text => new Text(tokenText),
            TokenType.Operator => new Operator(tokenText[0]),
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

        if (string.IsNullOrEmpty(Command))
        {
            throw new FormatException("Input is empty");
        }

        // Iterates through characters in the command
        foreach (var c in Command)
        {
            // Spaces between operators should be ignored
            if (c == ' ' || c == ',')
            {
                // If the current token is text, add the character
                if (currentTokenType == TokenType.Text)
                {
                    currentTokenText += c;
                }
                // If the current token is an operand, a space after means the token is complete and the next iteration should begin
                else if ((currentTokenType & TokenType.Operand) != TokenType.Undefined)
                {
                    currentTokenData = CompleteToken(ref currentTokenText, ref currentTokenType);
                    tokens.Add(currentTokenData);

                    // As the type of the next token has not been determined yet, set it to undefined
                    currentTokenType = TokenType.Undefined;
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
                    currentTokenData = CompleteToken(ref currentTokenText, ref currentTokenType);
                    tokens.Add(currentTokenData);

                    currentTokenType = TokenType.Undefined;
                    
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

        if (currentTokenType != TokenType.Undefined)
        {
            // Parses the last token
            currentTokenData = CompleteToken(ref currentTokenText, ref currentTokenType);
            tokens.Add(currentTokenData);
        }    

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
                || tokens[i + 1].Type == TokenType.Function && ((Function)tokens[i + 1]).Name != "!"
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

            // Do not substitute if you are reassigning the variable
            if (tokens[i+1].Type == TokenType.Operator && ((Operator)tokens[i+1]).Value == OperatorType.Equals)
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

    // Method called when the tokenisor has reached the end of a token and wants to parse it
    // Code split off as it is used in multiple places in the tokenise method
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

    // Method for parsing and evaluating user input
    // Used to differentiate between two different types of input (calculation and commands)
    public IToken? Run()
    {
        IToken[] tokens = Tokenise();

        // If the first token is a command, run that command
        if (tokens[0].Type == TokenType.Function && CommandNames.Contains(((Function)tokens[0]).Name))
        {
            Function command = (Function)tokens[0].CastTo(TokenType.Function);

            // Gets the remaining tokens and makes them a seperate array for the command's arguments
            IToken[] args = new IToken[tokens.Length - 1];
            Array.Copy(tokens, 1, args, 0, tokens.Length - 1);

            return RunCommand(command, args);
        }

        _Evaluator.Expression = tokens;
        return _Evaluator.Evaluate();
    }

    // Code for parsing a user's input for a command and running it
    private IToken? RunCommand(Function command, IToken[] args)
    {
        switch (command.Name)
        {
            case "?":
            case "help":
                {
                    return new Text(HELPTEXT);
                }
            case "caesarEn":
                {
                    Text plain = (Text)args[0].CastTo(TokenType.Text);
                    return Encryption.CaesarEn(plain);
                }
            case "caesarDe":
                {
                    Text cipher = (Text)args[0].CastTo(TokenType.Text);
                    return Encryption.CaesarDe(cipher);
                }
            case "affineEn":
                {
                    int a, b;

                    a = ((Integer)args[0].CastTo(TokenType.Integer)).Value;
                    b = ((Integer)args[1].CastTo(TokenType.Integer)).Value;

                    Text plain = (Text)args[2].CastTo(TokenType.Text);

                    return Encryption.AffineEn(a, b, plain);
                }
            case "affineDe":
                {
                    int a, b;

                    a = ((Integer)args[0].CastTo(TokenType.Integer)).Value;
                    b = ((Integer)args[1].CastTo(TokenType.Integer)).Value;

                    Text plain = (Text)args[2].CastTo(TokenType.Text);

                    return Encryption.AffineDe(a, b, plain);
                }
            case "bruteAffine":
                {
                    Text cipher = (Text)args[0].CastTo(TokenType.Text);

                    return Encryption.BruteAffine(cipher);
                }
            case "numRand":
                {
                    int a, x, c, m;

                    a = ((Integer)args[0].CastTo(TokenType.Integer)).Value;
                    x = ((Integer)args[1].CastTo(TokenType.Integer)).Value;
                    c = ((Integer)args[2].CastTo(TokenType.Integer)).Value;
                    m = ((Integer)args[3].CastTo(TokenType.Integer)).Value;

                    return NumberTheory.NumRand(a, x, c, m);
                }
            case "numPrime":
                {
                    int a = ((Integer)args[0].CastTo(TokenType.Integer)).Value;

                    return new Text(NumberTheory.IsPrime(a).ToString());
                }
            case "numCheckDigit":
                {
                    Text digits = (Text)args[0].CastTo(TokenType.Text);

                    return NumberTheory.NumCheckDigit(digits);
                }
            case "vec":
                {
                    Variable @var = (Variable)args[0];

                    Real x = (Real)args[3].CastTo(TokenType.Real);
                    Real y = (Real)args[4].CastTo(TokenType.Real);

                    Vec2 vec = new Vec2(x.Value, y.Value);
                    Variables[@var.Name] = vec;

                    return vec;
                }
            case "addVec":
                {
                    Vec2 a = (Vec2)args[0].CastTo(TokenType.Vec2);
                    Vec2 b = (Vec2)args[1].CastTo(TokenType.Vec2);

                    return a.Add(b);
                }
            case "subVec":
                {
                    Vec2 a = (Vec2)args[0].CastTo(TokenType.Vec2);
                    Vec2 b = (Vec2)args[1].CastTo(TokenType.Vec2);

                    return a.Minus(b);
                }
            case "dotVec":
                {
                    Vec2 a = (Vec2)args[0].CastTo(TokenType.Vec2);
                    Vec2 b = (Vec2)args[1].CastTo(TokenType.Vec2);

                    return a.Dot(b);
                }
            case "scalVec":
                {
                    Real s = (Real)args[0].CastTo(TokenType.Real);
                    Vec2 a = (Vec2)args[1].CastTo(TokenType.Vec2);

                    return a.Scale(s.Value);
                }
            case "Line":
                {
                    Variable @var = (Variable)args[0];

                    Real x1 = (Real)args[3].CastTo(TokenType.Real);
                    Real y1 = (Real)args[4].CastTo(TokenType.Real);
                    Real x2 = (Real)args[5].CastTo(TokenType.Real);
                    Real y2 = (Real)args[6].CastTo(TokenType.Real);

                    Variables[@var.Name] = new Line(new Vec2(x1.Value, y1.Value), new Vec2(x2.Value, y2.Value));

                    return Variables[@var.Name];
                }
            case "lengthLine":
                {
                    return ((Line)args[0].CastTo(TokenType.Line)).Length();
                }
            case "midpointLine":
                {
                    return ((Line)args[0].CastTo(TokenType.Line)).Midpoint();
                }
            case "gradientLine":
                {
                    return ((Line)args[0].CastTo(TokenType.Line)).Gradient();
                }
            case "mat":
                {
                    Variable @var = (Variable)args[0];

                    Real a = (Real)args[3].CastTo(TokenType.Real);
                    Real b = (Real)args[4].CastTo(TokenType.Real);
                    Real c = (Real)args[5].CastTo(TokenType.Real);
                    Real d = (Real)args[6].CastTo(TokenType.Real);

                    Variables[@var.Name] = new Matrix(a.Value, b.Value, c.Value, d.Value);

                    return Variables[@var.Name];
                }
            case "addMat":
                {
                    Matrix a = (Matrix)args[0].CastTo(TokenType.Matrix);
                    Matrix b = (Matrix)args[1].CastTo(TokenType.Matrix);

                    return a.Add(b);
                }
            case "dotMat":
                {
                    Matrix a = (Matrix)args[0].CastTo(TokenType.Matrix);
                    Matrix b = (Matrix)args[1].CastTo(TokenType.Matrix);

                    return a.Dot(b);
                }
            case "scalMat":
                {
                    Real s = (Real)args[0].CastTo(TokenType.Real);
                    Matrix a = (Matrix)args[1].CastTo(TokenType.Matrix);

                    return a.Scale(s);
                }
            case "detMat":
                {
                    Matrix a = (Matrix)args[0].CastTo(TokenType.Matrix);

                    return a.Det();
                }
            case "invMat":
                {
                    Matrix a = (Matrix)args[0].CastTo(TokenType.Matrix);

                    return a.Inv();
                }
            case "degToRad":
                {
                    Real theta = (Real)args[0].CastTo(TokenType.Real);

                    return Geometry.DegToRad(theta);
                }
            case "radToDeg":
                {
                    Real theta = (Real)args[0].CastTo(TokenType.Real);

                    return Geometry.RadToDeg(theta);
                }
            case "exit":
            case "quit":
                {
                    return null;
                }
            default:
                {
                    throw new NotImplementedException($"Command {command.Name} is not implemented");
                }
        }
    }
}