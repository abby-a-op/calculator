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
    public void IsAdditionCorrect()
    {
        _Parser.Expression = new IToken[]
        {
            new Integer(2),
            new Integer(3),
            new Operator(OperatorType.Plus)
        };
        IToken result = _Parser.Evaluate();

        Assert.AreEqual(5, ((Integer)result).Value);
    }

    [TestMethod]
    public void IsMultiplicationCorrect()
    {
        _Parser.Expression = new IToken[]
        {
            new Integer(5),
            new Integer(3),
            new Operator(OperatorType.Multiply)
        };
        Integer result = (Integer)_Parser.Evaluate();

        Assert.AreEqual(15, result.Value);
    }

    [TestMethod]
    public void IsExponentiationCorrect()
    {
        _Parser.Expression = new IToken[]
        {
            new Integer(10),
            new Integer(4),
            new Operator(OperatorType.Exponentiate)
        };
        Integer result = (Integer)_Parser.Evaluate();

        Assert.AreEqual(10000, result.Value);
    }
    
    [TestMethod]
    public void IsUnaryMinusCorrect()
    {
        _Parser.Expression = new IToken[]
        {
            new Integer(2),
            new Integer(3),
            new Operator(OperatorType.UnaryMinus),
            new Operator(OperatorType.Plus)
        };
        Integer result = (Integer)_Parser.Evaluate();
        
        Assert.AreEqual(-1, result.Value);
    }

    [TestMethod]
    public void IsFunctionParsedCorrectly()
    {
        _Parser.Expression = new IToken[]
        {
            new Integer(2),
            new Integer(3),
            new Function("!"),
            new Operator(OperatorType.Plus)
        };

        Integer result = (Integer)_Parser.Evaluate();

        Assert.AreEqual(8, result.Value);
    }
}