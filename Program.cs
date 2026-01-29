using System;

while (true)
{
    Console.Clear();
    Console.WriteLine("=== Assistant ===");
    Console.WriteLine("1) Settings");
    Console.WriteLine("2) Commands");
    Console.WriteLine("0) Exit");
    Console.WriteLine();
    Console.Write("Select an option: ");

    var input = Console.ReadLine()?.Trim();

    if (!int.TryParse(input, out var option))
    {
        Console.WriteLine("Invalid input. Please enter a number.");
        Pause();
        continue;
    }

    switch (option)
    {
        case 1:
            ShowSettings();
            break;

        case 2:
            ShowCommands();
            break;

        case 0:
            Console.WriteLine("Bye!");
            return;

        default:
            Console.WriteLine("Unknown option.");
            Pause();
            break;
    }
}

static void ShowSettings()
{
    Console.Clear();
    Console.WriteLine("=== Settings ===");
    Console.WriteLine("(Coming soon)");
    Pause();
}

static void ShowCommands()
{
    Console.Clear();
    Console.WriteLine("=== Commands ===");
    Console.WriteLine("(Coming soon)");
    Pause();
}

static void Pause()
{
    Console.WriteLine();
    Console.Write("Press any key to continue...");
    Console.ReadKey(true);
}
