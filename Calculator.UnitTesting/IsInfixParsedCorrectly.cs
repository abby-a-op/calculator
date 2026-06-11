using System.Security;
using Newtonsoft.Json.Bson;

namespace Calculator.UnitTesting;

[TestClass]
public class IsInfixParsedCorrectly
{
    InfixToPostfixParser _parser;    
    public IsInfixParsedCorrectly()
    {
        _parser = new InfixToPostfixParser();
    }

    private bool ExpressionEqual(Token[] expected, Token[] actual)
    {
        if (expected.Length != actual.Length)
        {
            return false;
        }

        for (int i=0; i<actual.Length; i++)
        {
            if (!(expected[i].Type==actual[i].Type && expected[i].Value==actual[i].Value))
            {
                return false;
            }
        }

        return true;

    }

    [TestMethod]
    public void IsMultiplicationParsedCorrectly()
    {
        _parser.Expression = "2*3";

        Token[] expectedOutput = new Token[]
        { 
            new Token()
            {
                Value="2",
                Type=TokenType.Integer
            },
            new Token()
            {
                Value="3",
                Type=TokenType.Integer
            },
            new Token()
            {
                Value="*",
                Type=TokenType.Operator
            }
        };
        
        bool isEqual = ExpressionEqual(_parser.Parse(), expectedOutput);

        Assert.IsTrue(isEqual);
    }

    [TestMethod]
    public void IsNegativeNumberParsedCorrectly()
    {
        _parser.Expression = "-2";

        Token[] expectedOutput = new Token[]
        {
            new Token()
            {
                Value="0",
                Type=TokenType.Integer
            },
            new Token()
            {
                Value="2",
                Type=TokenType.Integer
            },
            new Token()
            {
                Value="-",
                Type=TokenType.Operator
            }
        };
        
        bool isEqual = ExpressionEqual(_parser.Parse(), expectedOutput);

        Assert.IsTrue(isEqual);
    }

    [TestMethod]
    public void IsJuxtapositionParsedCorrectly()
    {
        _parser.Expression = "2(4^3)";

        Token[] expectedOutput = new Token[]
        {
            new Token()
            {
                Value="2",
                Type=TokenType.Integer
            },
            new Token()
            {
                Value="4",
                Type=TokenType.Integer
            },
            new Token()
            {
                Value="3",
                Type=TokenType.Integer
            },
            new Token()
            {
                Value = "^",
                Type=TokenType.Operator
            },
            new Token()
            {
                Value = "*",
                Type=TokenType.Operator
            }
        };
        
        bool isEqual = ExpressionEqual(_parser.Parse(), expectedOutput);

        Assert.IsTrue(isEqual);
    }

    [TestMethod]
    public void IsAdditionAfterBracketParsedCorrectly()
    {
        _parser.Expression = "2(4^3)+2";

        Token[] expectedOutput = new Token[]
        {
            new Token()
            {
                Value="2",
                Type=TokenType.Integer
            },
            new Token()
            {
                Value="4",
                Type=TokenType.Integer
            },
            new Token()
            {
                Value="3",
                Type=TokenType.Integer
            },
            new Token()
            {
                Value = "^",
                Type=TokenType.Operator
            },
            new Token()
            {
                Value = "*",
                Type = TokenType.Operator
            },
            new Token()
            {
                Value = "2",
                Type=TokenType.Integer
            },
            new Token()
            {
                Value = "+",
                Type=TokenType.Operator
            }
        };

        bool isEqual = ExpressionEqual(_parser.Parse(), expectedOutput);

        Assert.IsTrue(isEqual);
    }

    [TestMethod]
    public void IsMultiplicationAtEndOfBrackedParsedCorrectly()
    {
        _parser.Expression = "(3)2";

        Token[] expected = new Token[]
        {
            new Token()
            {
                Value = "3",
                Type = TokenType.Integer
            },
            new Token()
            {
                Value = "2",
                Type = TokenType.Integer
            },
            new Token()
            {
                Value = "*",
                Type = TokenType.Operator
            }
        };

        bool isCorrect = ExpressionEqual(expected, _parser.Parse());

        Assert.IsTrue(isCorrect);
    }
}