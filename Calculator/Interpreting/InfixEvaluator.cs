namespace Calculator;

// Class for evaluating the result of an infix expression, used to abstract away the complexity of the reverse polish notation
public class InfixEvaluator
{
    private InfixToPostfixParser _infixParser = new InfixToPostfixParser();
    private PostfixParser _postfixParser = new PostfixParser();

    public IToken[] Expression = new IToken[] { };

    // Evaluates the expression by first parsing it to postfix, then evaluating the postfix expression
    public IToken Evaluate()
    {
        _infixParser.Expression = Expression;
        _postfixParser.Expression = _infixParser.Parse();
        
        return _postfixParser.Evaluate();
    }
}