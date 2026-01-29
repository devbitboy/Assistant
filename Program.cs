using System;
using Assistant.ConsoleApp;

var menu = new List<MenuOption>
{
    new("Settings", ShowSettings),
    new("Commands", ShowCommands),
};

while (true)
{
    Console.Clear();
    Console.WriteLine("=== Assistant ===");

    for (var i = 0; i < menu.Count; i++)
        Console.WriteLine($"{i + 1}) {menu[i].Title}");

    Console.WriteLine("0) Exit");
    Console.WriteLine();

    var option = ConsoleUi.ReadOption("Select an option: ");
    if (option is null)
    {
        Console.WriteLine("Invalid input. Please enter a number.");
        ConsoleUi.Pause();
        continue;
    }

    if (option == 0)
    {
        Console.WriteLine("Bye!");
        return;
    }

    var index = option.Value - 1;
    if (index < 0 || index >= menu.Count)
    {
        Console.WriteLine("Unknown option.");
        ConsoleUi.Pause();
        continue;
    }

    menu[index].Action();
}

static void ShowSettings()
{
    Console.Clear();
    Console.WriteLine("=== Settings ===");
    Console.WriteLine("(Coming soon)");
    ConsoleUi.Pause();
}

static void ShowCommands()
{
    Console.Clear();
    Console.WriteLine("=== Commands ===");
    Console.WriteLine("(Coming soon)");
    ConsoleUi.Pause();
}
