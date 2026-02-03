namespace Assistant.ConsoleApp;

public static class ConsoleUi
{
    public static void Pause()
    {
        Console.WriteLine();
        Console.Write("Press any key to continue...");
        Console.ReadKey(true);
    }

    public static int? ReadOption(string prompt)
    {
        Console.Write(prompt);
        var input = Console.ReadLine()?.Trim();

        if (int.TryParse(input, out var option))
            return option;

        return null;
    }
}
