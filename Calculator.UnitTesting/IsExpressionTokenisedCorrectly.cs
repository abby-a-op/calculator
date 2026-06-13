namespace Calculator.UnitTesting;

[TestClass]
public class IsExpressionTokenisedCorrectly
{
    Interpreter _interpreter;

    public IsExpressionTokenisedCorrectly()
    {
        _interpreter = new Interpreter();
    }

    bool TokensEqual(Token[] expected, Token[] actual)
    {
        if (expected.Length != actual.Length)
        {
            return false;
        }

        for (int i=0; i<expected.Length; i++)
        {
            if (expected[i].Type != actual[i].Type || expected[i].Value != actual[i].Value)
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

        Token[] expected = new Token[]
        {
            new Token()
            {
                Value = "2",
                Type = TokenType.Number
            },
            new Token()
            {
                Value = "+",
                Type = TokenType.Operator
            },
            new Token()
            {
                Value = "3",
                Type = TokenType.Number
            },
            new Token()
            {
                Value = "-",
                Type = TokenType.Operator
            },
            new Token()
            {
                Value = "3",
                Type = TokenType.Number
            },
            new Token()
            {
                Value = "*",
                Type = TokenType.Operator
            },
            new Token()
            {
                Value = "2",
                Type = TokenType.Number
            },
            new Token()
            {
                Value = "%",
                Type = TokenType.Operator
            },
            new Token()
            {
                Value = "2",
                Type = TokenType.Number
            },
            new Token()
            {
                Value = "!",
                Type = TokenType.Operator
            },
            new Token()
            {
                Value = "^",
                Type = TokenType.Operator
            },
            new Token()
            {
                Value = "2",
                Type = TokenType.Number
            },
            new Token()
            {
                Value = "*",
                Type = TokenType.Operator
            },
            new Token()
            {
                Value = "2",
                Type = TokenType.Number
            },
            new Token()
            {
                Value = "*",
                Type = TokenType.Operator
            },
            new Token()
            {
                Value = "(",
                Type = TokenType.Operator
            },
            new Token()
            {
                Value = "2",
                Type = TokenType.Number
            },
            new Token()
            {
                Value = "-",
                Type = TokenType.Operator
            },
            new Token()
            {
                Value = "3",
                Type = TokenType.Number
            },
            new Token()
            {
                Value = ")",
                Type = TokenType.Operator
            }
        };

        Assert.IsTrue(TokensEqual(expected, _interpreter.Tokenise()));
    }

    [TestMethod]
    public void IsUnaryMinusTokenisedCorrectly()
    {
        _interpreter.Command = "-2";

        Token[] expected = new Token[]
        {
            new Token()
            {
                Value = "0",
                Type = TokenType.Number
            },
            new Token()
            {
                Value = "-",
                Type = TokenType.Operator
            },
            new Token()
            {
                Value = "2",
                Type = TokenType.Number
            }
        };

        Assert.IsTrue(TokensEqual(expected, _interpreter.Tokenise()));
    }

    [TestMethod]
    public void IsJuxtapositionTokenisedCorrectly()
    {
        _interpreter.Command = "2(4^3)";

        Token[] expected = new Token[]
        {
            new Token()
            {
                Value = "2",
                Type = TokenType.Number
            },
            new Token()
            {
                Value = "*",
                Type = TokenType.Operator
            },
            new Token()
            {
                Value = "(",
                Type = TokenType.Operator
            },
            new Token()
            {
                Value = "4",
                Type = TokenType.Number
            },
            new Token()
            {
                Value = "^",
                Type = TokenType.Operator
            },
            new Token()
            {
                Value = "3",
                Type = TokenType.Number
            },
            new Token()
            {
                Value = ")",
                Type = TokenType.Operator
            }
        };

        Assert.IsTrue(TokensEqual(expected, _interpreter.Tokenise()));
    }

    [TestMethod]
    public void IsAdditionAfterBracketTokenisedCorrectly()
    {
        _interpreter.Command = "2(4^3)+2";

        Token[] expected = new Token[]
        {
            new Token()
            {
                Value = "2",
                Type = TokenType.Number
            },
            new Token()
            {
                Value = "*",
                Type = TokenType.Operator
            },
            new Token()
            {
                Value = "(",
                Type = TokenType.Operator
            },
            new Token()
            {
                Value = "4",
                Type = TokenType.Number
            },
            new Token()
            {
                Value = "^",
                Type = TokenType.Operator
            },
            new Token()
            {
                Value = "3",
                Type = TokenType.Number
            },
            new Token()
            {
                Value = ")",
                Type = TokenType.Operator
            },
            new Token()
            {
                Value = "+",
                Type = TokenType.Operator
            },
            new Token()
            {
                Value = "2",
                Type = TokenType.Number
            }
        };

        Assert.IsTrue(TokensEqual(expected, _interpreter.Tokenise()));
    }

    [TestMethod]
    public void IsUnaryMinusInBracketsTokenisedCorrectly()
    {
        _interpreter.Command = "(-2)^2";

        Token[] expected = new Token[]
        {
            new Token()
            {
                Value = "(",
                Type = TokenType.Operator
            },
            new Token()
            {
                Value = "0",
                Type = TokenType.Number
            },
            new Token()
            {
                Value = "-",
                Type = TokenType.Operator
            },
            new Token()
            {
                Value = "2",
                Type = TokenType.Number
            },
            new Token()
            {
                Value = ")",
                Type = TokenType.Operator
            },
            new Token()
            {
                Value = "^",
                Type = TokenType.Operator
            },
            new Token()
            {
                Value = "2",
                Type = TokenType.Number
            },
        };

        Assert.IsTrue(TokensEqual(expected, _interpreter.Tokenise()));
    }
}