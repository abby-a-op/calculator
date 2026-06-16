namespace Calculator.UnitTesting;

[TestClass]
public class IsInfixParsedCorrectly
{
    InfixToPostfixParser _parser;    
    public IsInfixParsedCorrectly()
    {
        _parser = new InfixToPostfixParser();
    }

    bool ExpressionEqual(IToken[] expected, IToken[] actual)
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
    public void IsMultiplicationParsedCorrectly()
    {
        _parser.Expression = new IToken[]
        {
            new Integer(2),
            new Operator(OperatorType.Multiply),
            new Integer(3)
        };

        IToken[] expectedOutput = new IToken[]
        { 
            new Integer(2),
            new Integer(3),
            new Operator(OperatorType.Multiply)
        };
        
        bool isEqual = ExpressionEqual(_parser.Parse(), expectedOutput);

        Assert.IsTrue(isEqual);
    }

    [TestMethod]
    public void IsExponentInBracketParsedCorrectly()
    {
        _parser.Expression = new IToken[]
        {
            new Integer(10),
            new Operator(OperatorType.Exponentiate),
            new Operator(OperatorType.OpeningBracket),
            new Integer(53),
            new Operator(OperatorType.Minus),
            new Integer(2),
            new Operator(OperatorType.ClosingBracket)
        };

        IToken[] expected = new IToken[]
        {
            new Integer(10),
            new Integer(53),
            new Integer(2),
            new Operator(OperatorType.Minus),
            new Operator(OperatorType.Exponentiate)
        };

        Assert.IsTrue(ExpressionEqual(expected, _parser.Parse()));
    }

    [TestMethod]
    public void IsJuxtapositionParsedCorrectly()
    {
        _parser.Expression = new IToken[]
        {
            new Integer(2),
            new Operator(OperatorType.Multiply),
            new Operator(OperatorType.OpeningBracket),
            new Integer(4),
            new Operator(OperatorType.Exponentiate),
            new Integer(3),
            new Operator(OperatorType.ClosingBracket)
        };

        IToken[] expectedOutput = new IToken[]
        {
            new Integer(2),
            new Integer(4),
            new Integer(3),
            new Operator(OperatorType.Exponentiate),
            new Operator(OperatorType.Multiply)
        };
        
        bool isEqual = ExpressionEqual(_parser.Parse(), expectedOutput);

        Assert.IsTrue(isEqual);
    }

    [TestMethod]
    public void IsAdditionAfterBracketParsedCorrectly()
    {
        _parser.Expression = new IToken[]
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

        IToken[] expectedOutput = new IToken[]
        {
            new Integer(2),
            new Integer(4),
            new Integer(3),
            new Operator(OperatorType.Exponentiate),
            new Operator(OperatorType.Multiply),
            new Integer(2),
            new Operator(OperatorType.Plus)
        };

        bool isEqual = ExpressionEqual(_parser.Parse(), expectedOutput);

        Assert.IsTrue(isEqual);
    }

    [TestMethod]
    public void IsMultiplicationAtEndOfBrackedParsedCorrectly()
    {
        _parser.Expression = new IToken[]
        {
            new Operator(OperatorType.OpeningBracket),
            new Integer(3),
            new Operator(OperatorType.ClosingBracket),
            new Operator(OperatorType.Multiply),
            new Integer(2)
        };

        IToken[] expected = new IToken[]
        {
            new Integer(3),
            new Integer(2),
            new Operator(OperatorType.Multiply)
        };

        bool isCorrect = ExpressionEqual(expected, _parser.Parse());

        Assert.IsTrue(isCorrect);
    }
}