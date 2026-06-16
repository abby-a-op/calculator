namespace Calculator.UnitTesting;

[TestClass]
public class IsExpressionTokenisedCorrectly
{
    Interpreter _interpreter;

    public IsExpressionTokenisedCorrectly()
    {
        _interpreter = new Interpreter();
    }

    bool TokensEqual(IToken[] expected, IToken[] actual)
    {
        if (expected.Length != actual.Length)
        {
            return false;
        }

        foreach (var token in expected)
        {
            Console.Write($"{token.Output()} ({token.Type}) ");
        }
        Console.WriteLine();

        foreach (var token in actual)
        {
            Console.Write($"{token.Output()} ({token.Type}) ");
        }
        Console.WriteLine();

        for (int i=0; i<expected.Length; i++)
        {
            if (expected[i].Type != actual[i].Type || expected[i].Output() != actual[i].Output())
            {
                return false;
            }
        }

        return true;
    }

    [TestMethod]
    public void AreAllOperatorsTokenisedCorrectly()
    {
        _interpreter.Command = "2+3-3*2%2!^2*2*(2-3)";

        IToken[] expected = new IToken[]
        {
            new Integer(2),
            new Operator(OperatorType.Plus),
            new Integer(3),
            new Operator(OperatorType.Minus),
            new Integer(3),
            new Operator(OperatorType.Multiply),
            new Integer(2),
            new Operator(OperatorType.Modulo),
            new Integer(2),
            new Function("!"),
            new Operator(OperatorType.Exponentiate),
            new Integer(2),
            new Operator(OperatorType.Multiply),
            new Integer(2),
            new Operator(OperatorType.Multiply),
            new Operator(OperatorType.OpeningBracket),
            new Integer(2),
            new Operator(OperatorType.Minus),
            new Integer(3),
            new Operator(OperatorType.ClosingBracket)
        };

        Assert.IsTrue(TokensEqual(expected, _interpreter.Tokenise()));
    }
    
    [TestMethod]
    public void IsExponentInBracketTokenisedCorrectly()
    {
        _interpreter.Command = "10^(53-2)";

        IToken[] expected = new IToken[]
        {
            new Integer(10),
            new Operator(OperatorType.Exponentiate),
            new Operator(OperatorType.OpeningBracket),
            new Integer(53),
            new Operator(OperatorType.Minus),
            new Integer(2),
            new Operator(OperatorType.ClosingBracket)
        };

        Assert.IsTrue(TokensEqual(expected, _interpreter.Tokenise()));
    }

    [TestMethod]
    public void IsUnaryMinusTokenisedCorrectly()
    {
        _interpreter.Command = "-2";

        IToken[] expected = new IToken[]
        {
            new Operator(OperatorType.UnaryMinus),
            new Integer(2)
        };

        Assert.IsTrue(TokensEqual(expected, _interpreter.Tokenise()));
    }

    [TestMethod]
    public void IsJuxtapositionTokenisedCorrectly()
    {
        _interpreter.Command = "2(4^3)";

        IToken[] expected = new IToken[]
        {
            new Integer(2),
            new Operator(OperatorType.Multiply),
            new Operator(OperatorType.OpeningBracket),
            new Integer(4),
            new Operator(OperatorType.Exponentiate),
            new Integer(3),
            new Operator(OperatorType.ClosingBracket)
        };

        Assert.IsTrue(TokensEqual(expected, _interpreter.Tokenise()));
    }

    [TestMethod]
    public void IsAdditionAfterBracketTokenisedCorrectly()
    {
        _interpreter.Command = "2(4^3)+2";

        IToken[] expected = new IToken[]
        {
            new Integer(2),
            new Operator(OperatorType.Multiply),
            new Operator(OperatorType.OpeningBracket),
            new Integer(4),
            new Operator(OperatorType.Exponentiate),
            new Integer(3),
            new Operator(OperatorType.ClosingBracket),
            new Operator(OperatorType.Plus),
            new Integer(2)
        };
        Assert.IsTrue(TokensEqual(expected, _interpreter.Tokenise()));
    }

    [TestMethod]
    public void IsUnaryMinusInBracketsTokenisedCorrectly()
    {
        _interpreter.Command = "(-2)^2";

        IToken[] expected = new IToken[]
        {
            new Operator(OperatorType.OpeningBracket),
            new Operator(OperatorType.UnaryMinus),
            new Integer(2),
            new Operator(OperatorType.ClosingBracket),
            new Operator(OperatorType.Exponentiate),
            new Integer(2)
        };

        Assert.IsTrue(TokensEqual(expected, _interpreter.Tokenise()));
    }

    [TestMethod]
    public void IsNegativeDenominatorTokenisedCorrectly()
    {
        _interpreter.Command = "2/-3";

        IToken[] expected = new IToken[]
        {
            new Integer(2),
            new Operator(OperatorType.Divide),
            new Operator(OperatorType.OpeningBracket),
            new Integer(0),
            new Operator(OperatorType.Minus),
            new Integer(3),
            new Operator(OperatorType.ClosingBracket)
        };

        Assert.IsTrue(TokensEqual(expected, _interpreter.Tokenise()));
    }
}