namespace Calculator.Frontend;

public class Program
{
    public static void Main()
    {
        Interpreter interpreter = new Interpreter();

        while (true)
        {
            string input = Console.ReadLine() ?? "";
            interpreter.Command = input;

            string result = interpreter.Run();

            Console.WriteLine(result);
        }
    }
}