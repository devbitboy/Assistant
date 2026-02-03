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

    if (!Directory.Exists(projectPath))
    {
        Console.WriteLine($"Project path not found: {projectPath}");
        ConsoleUi.Pause();
        return;
    }

    var candidates = new[]
    {
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            @"Programs\Microsoft VS Code\Code.exe"
        ),
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
            @"Microsoft VS Code\Code.exe"
        ),
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
            @"Microsoft VS Code\Code.exe"
        ),
    };

    var vsCodeExe = candidates.FirstOrDefault(File.Exists);

    if (vsCodeExe is null)
    {
        Console.WriteLine("VS Code was not found.");
        ConsoleUi.Pause();
        return;
    }

    Process.Start(new ProcessStartInfo
    {
        FileName = vsCodeExe,
        Arguments = $"\"{projectPath}\"",
        UseShellExecute = true
    });

    Console.WriteLine("Opening project in VS Code...");
    ConsoleUi.Pause();
}

static void ShowCommands()
{
    Console.Clear();
    Console.WriteLine("=== Commands ===");
    Console.WriteLine("(Coming soon)");
    ConsoleUi.Pause();
}
