using System.Security;

namespace Calculator.UnitTesting;

[TestClass]
public class IsInfixParsedCorrectly
{
    InfixToPostfixParser _parser;    
    public IsInfixParsedCorrectly()
    {
        _parser = new InfixToPostfixParser();
    }

    [TestMethod]
    public void IsAdditionParsedCorrectly()
    {
        _parser.Expression = "2+2";

        string[] expectedOutput = new string[] { "2", "2", "+" };
        
        bool isEqual = expectedOutput.SequenceEqual(_parser.Parse());

        Assert.IsTrue(isEqual);
    }

    [TestMethod]
    public void IsMultiplicationParsedCorrectly()
    {
        _parser.Expression = "2*3";

        string[] expectedOutput = new string[] { "2", "3", "*" };
        
        bool isEqual = expectedOutput.SequenceEqual(_parser.Parse());

        Assert.IsTrue(isEqual);
    }

    [TestMethod]
    public void IsNonCommutativeOperationParsedCorrectly()
    {
        _parser.Expression = "2^3";
        
        string[] expectedOutput = new string[] { "2", "3", "^" };
        bool isEqual = expectedOutput.SequenceEqual(_parser.Parse());

        Assert.IsTrue(isEqual);
    }

    [TestMethod]
    public void IsOrderOfOperationsParsedCorrectly()
    {
        _parser.Expression = "2*3+5^(2-2*2)";

        string[] expectedOutput = new string[] { "2", "3", "*", "5", "2", "2", "2", "*", "-", "^", "+" };
        bool isEqual = expectedOutput.SequenceEqual(_parser.Parse());

        Assert.IsTrue(isEqual);
    }
}