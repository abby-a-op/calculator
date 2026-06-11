using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Calculator.UnitTesting;

[TestClass]
public class IsPostFixParsedCorrectly
{
    private PostfixParser _Parser;

    public IsPostFixParsedCorrectly()
    {
        _Parser = new PostfixParser();
    }

    [TestMethod]
    [DataRow(new object[] { 1, 1, 2 })]
    [DataRow(new object[] { 0, 0, 0 })]
    [DataRow(new object[] { 3, 6, 9 })]
    [DataRow(new object[] { 10, 11, 21 })]
    [DataRow(new object[] { 100, 1002, 1102 })]
    public void IsAdditionCorrect(int a, int b, int expectedResult)
    {
        _Parser.Expression = new Token[]
        {
            new Token()
            {
                Value = a.ToString(),
                Type = TokenType.Integer
            },
            new Token()
            {
                Value = b.ToString(),
                Type = TokenType.Integer
            },
            new Token()
            {
                Value = "+",
                Type = TokenType.Operator
            }
        };
        int result = _Parser.Evaluate();

        Assert.AreEqual(expectedResult, result);
    }

    [TestMethod]
    [DataRow(new object[] { 1, 1, 1 })]
    [DataRow(new object[] { 0, 0, 0 })]
    [DataRow(new object[] { 10, 10, 100 })]
    [DataRow(new object[] { 3, 2, 6 })]
    public void IsMultiplicationCorrect(int a, int b, int expectedResult)
    {
        _Parser.Expression = new Token[]
        {
            new Token()
            {
                Value = a.ToString(),
                Type = TokenType.Integer
            },
            new Token()
            {
                Value = b.ToString(),
                Type = TokenType.Integer
            },
            new Token()
            {
                Value = "*",
                Type = TokenType.Operator
            }
        };
        int result = _Parser.Evaluate();

        Assert.AreEqual(expectedResult, result);
    }
}