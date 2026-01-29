using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    var settingsMenu = new List<MenuOption>
    {
        new("Open Project in VS Code", OpenProjectInVsCode),
    };

    while (true)
    {
        Console.Clear();
        Console.WriteLine("=== Settings ===");

        for (var i = 0; i < settingsMenu.Count; i++)
            Console.WriteLine($"{i + 1}) {settingsMenu[i].Title}");

        Console.WriteLine("0) Back");
        Console.WriteLine();

        var option = ConsoleUi.ReadOption("Select an option: ");
        if (option is null)
        {
            Console.WriteLine("Invalid input. Please enter a number.");
            ConsoleUi.Pause();
            continue;
        }

        if (option == 0)
            return;

        var index = option.Value - 1;
        if (index < 0 || index >= settingsMenu.Count)
        {
            Console.WriteLine("Unknown option.");
            ConsoleUi.Pause();
            continue;
        }

        settingsMenu[index].Action();
    }
}

static void OpenProjectInVsCode()
{
    var projectPath = @"C:\dev\Assistant";

    // Ruta estándar de VS Code en Windows
    var vsCodePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        @"Programs\Microsoft VS Code\Code.exe"
    );

    try
    {
        if (!File.Exists(vsCodePath))
        {
            Console.WriteLine("VS Code was not found.");
            Console.WriteLine("Please make sure Visual Studio Code is installed.");
            ConsoleUi.Pause();
            return;
        }

        Process.Start(new ProcessStartInfo
        {
            FileName = vsCodePath,
            Arguments = $"\"{projectPath}\"",
            UseShellExecute = true
        });

        Console.WriteLine("Opening project in VS Code...");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Failed to open VS Code.");
        Console.WriteLine(ex.Message);
    }

    ConsoleUi.Pause();
}

static void ShowCommands()
{
    Console.Clear();
    Console.WriteLine("=== Commands ===");
    Console.WriteLine("(Coming soon)");
    ConsoleUi.Pause();
}
