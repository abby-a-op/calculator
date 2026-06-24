namespace Calculator.Frontend;

public class Program
{
    public static void Main()
    {
        Interpreter interpreter = new Interpreter();
        Console.WriteLine("Type ? or 'help' for a list of commands");

        while (true)
        {
            string input = Console.ReadLine() ?? "";
            interpreter.Command = input;

            try
            {
                IToken result = interpreter.Run();

                Console.WriteLine(result.Output());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}