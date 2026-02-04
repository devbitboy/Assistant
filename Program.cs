using System.Diagnostics;
using Assistant.ConsoleApp;

List<MenuOption> mainMenu =
[
    new("Settings", ShowSettings),
    new("Commands", ShowCommands),
];

RunMenu("Assistant", mainMenu, "Exit", onExit: () => Console.WriteLine("Bye!"));

static void ShowSettings()
{
    List<MenuOption> settingsMenu =
    [
        new("Open Project in VS Code", OpenProjectInVsCode),
    ];

    RunMenu("Settings", settingsMenu, "Back");
}

static void RunMenu(string title, IReadOnlyList<MenuOption> options, string exitLabel, Action? onExit = null)
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine($"=== {title} ===");
        Console.WriteLine();

        for (var i = 0; i < options.Count; i++)
            Console.WriteLine($"{i + 1}) {options[i].Title}");

        Console.WriteLine($"0) {exitLabel}");
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
            onExit?.Invoke();
            return;
        }

        var index = option.Value - 1;
        if (index < 0 || index >= options.Count)
        {
            Console.WriteLine("Unknown option.");
            ConsoleUi.Pause();
            continue;
        }

        options[index].Action();
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

    // Prefer code.exe to avoid spawning a cmd window
    var candidates = new List<string>
    {
        // User install
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            @"Programs\Microsoft VS Code\Code.exe"
        ),
        // System installs
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
            @"Microsoft VS Code\Code.exe"
        ),
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
            @"Microsoft VS Code\Code.exe"
        ),
    };

    // Try to resolve "code" from PATH and map it to code.exe when possible
    var fromPath = TryResolveFromPath("code")
                ?? TryResolveFromPath("code.cmd")
                ?? TryResolveFromPath("code.exe");

    if (!string.IsNullOrWhiteSpace(fromPath))
    {
        // If PATH points to code.cmd, try to infer sibling code.exe:
        // e.g. ...\Microsoft VS Code\bin\code.cmd -> ...\Microsoft VS Code\Code.exe
        var pathLower = fromPath.ToLowerInvariant();
        if (pathLower.EndsWith(@"\bin\code.cmd") || pathLower.EndsWith(@"\bin\code"))
        {
            var maybeExe = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(fromPath)!, @"..\Code.exe"));
            candidates.Insert(0, maybeExe);
        }

        // If PATH already is code.exe, use it
        if (pathLower.EndsWith(@"\code.exe"))
            candidates.Insert(0, fromPath);
    }

    var vsCodeExe = candidates.FirstOrDefault(File.Exists);

    if (vsCodeExe is null)
    {
        // Last resort: try "code" (may open a cmd window depending on installation)
        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "code",
                Arguments = $"\"{projectPath}\"",
                UseShellExecute = true
            });

            Console.WriteLine("Opening project in VS Code (via PATH)...");
            ConsoleUi.Pause();
            return;
        }
        catch
        {
            Console.WriteLine("VS Code was not found.");
            Console.WriteLine("Tip: Install VS Code and enable the 'code' command in PATH.");
            ConsoleUi.Pause();
            return;
        }
    }

    Process.Start(new ProcessStartInfo
{
    FileName = vsCodeExe,
    UseShellExecute = true,
    ArgumentList =
    {
        "--reuse-window",
        projectPath
    }
});


    Console.WriteLine("Opening project in VS Code...");
    ConsoleUi.Pause();
}

static string? TryResolveFromPath(string fileName)
{
    try
    {
        var psi = new ProcessStartInfo
        {
            FileName = "where",
            Arguments = fileName,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var p = Process.Start(psi);
        if (p is null) return null;

        var output = p.StandardOutput.ReadToEnd();
        p.WaitForExit();

        var firstLine = output
            .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
            .FirstOrDefault();

        return string.IsNullOrWhiteSpace(firstLine) ? null : firstLine.Trim();
    }
    catch
    {
        return null;
    }
}

static void ShowCommands()
{
    Console.Clear();
    Console.WriteLine("=== Commands ===");
    Console.WriteLine("(Coming soon)");
    ConsoleUi.Pause();
}
