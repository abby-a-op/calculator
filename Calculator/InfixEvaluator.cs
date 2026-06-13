namespace Calculator;

public class InfixEvaluator
{
    private InfixToPostfixParser _infixParser = new InfixToPostfixParser();
    private PostfixParser _postfixParser = new PostfixParser();

    public Token[] Expression = new Token[] { };

    public double Evaluate()
    {
        _infixParser.Expression = Expression;
        _postfixParser.Expression = _infixParser.Parse();
        
        return _postfixParser.Evaluate();
    }
}