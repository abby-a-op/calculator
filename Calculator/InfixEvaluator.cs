namespace Calculator;

public class InfixEvaluator
{
    private InfixToPostfixParser _infixParser = new InfixToPostfixParser();
    private PostfixParser _postfixParser = new PostfixParser();

    public IToken[] Expression = new IToken[] { };

    public IToken Evaluate()
    {
        _infixParser.Expression = Expression;
        _postfixParser.Expression = _infixParser.Parse();
        
        return _postfixParser.Evaluate();
    }
}