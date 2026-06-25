namespace Calculator.Frontend;

public class Program
{
    public static void Main()
    {
        Interpreter interpreter = new Interpreter();
        Console.WriteLine("Type ? or 'help' for a list of commands");

        bool continuing = true;

        while (continuing)
        {
            string? input = Console.ReadLine();

            if (input != null)
            {
                interpreter.Command = input;

                try
                {
                    IToken? result = interpreter.Run();

                    if (result != null)
                    {
                        Console.WriteLine(result.Output());
                    }
                    else
                    {
                        continuing = false;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                Console.WriteLine();
            }
            else
            {
                continuing = false;
            }
        }
    }
}