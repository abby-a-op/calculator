namespace Calculator.Frontend;

public class Program
{
    public static void Main()
    {
        InfixEvaluator evaluator = new InfixEvaluator();

        while (true)
        {
            string input = Console.ReadLine();
            evaluator.Expression = input;

            Console.WriteLine(evaluator.Evaluate());
        }
    }
}