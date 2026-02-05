using System.Diagnostics;
using Assistant.ConsoleApp;

List<MenuOption> mainMenu =
[
    new("Settings", ShowSettings),
    new("Projects", ShowProjects),
    new("Commands", ShowCommands),
];

RunMenu("Assistant", mainMenu, "Exit", onExit: () => Console.WriteLine("Bye!"));

static void ShowSettings()
{
    List<MenuOption> settingsMenu =
    [
        new("Open Project in VS Code", () => OpenProjectInVsCode(@"C:\dev\Assistant")),
        new("Update Assistant", () => ShowMessageComingSoon("Update Assistant")),
    ];

    RunMenu("Settings", settingsMenu, "Back");
}

static void ShowProjects()
{
    List<MenuOption> projectsMenu =
    [
        new("Project Intelliflow", () => ShowMessageComingSoon("Project Intelliflow")),
    ];

    RunMenu("Projects", projectsMenu, "Back");
}

static void ShowCommands() => ShowMessageComingSoon("Commands");

static void RunMenu(string title, IReadOnlyList<MenuOption> options, string exitLabel, Action? onExit = null)
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine($"=== {title} ===\n");

        for (var i = 0; i < options.Count; i++)
            Console.WriteLine($"{i + 1}) {options[i].Title}");

        Console.WriteLine($"0) {exitLabel}\n");

        var option = ConsoleUi.ReadOption("Select an option: ");
        if (option is null)
        {
            PrintAndPause("Invalid input. Please enter a number.");
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
            PrintAndPause("Unknown option.");
            continue;
        }

        options[index].Action();
    }
}

static void OpenProjectInVsCode(string projectPath)
{
    if (!Directory.Exists(projectPath))
    {
        PrintAndPause($"Project path not found: {projectPath}");
        return;
    }

    var codeExeCandidates = new List<string>
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

    // Try to resolve "code" from PATH and map it to Code.exe when possible
    var fromPath = TryResolveFromPath("code")
                ?? TryResolveFromPath("code.cmd")
                ?? TryResolveFromPath("code.exe");

    if (!string.IsNullOrWhiteSpace(fromPath))
    {
        var pathLower = fromPath.ToLowerInvariant();

        if (pathLower.EndsWith(@"\bin\code.cmd") || pathLower.EndsWith(@"\bin\code"))
        {
            var maybeExe = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(fromPath)!, @"..\Code.exe"));
            codeExeCandidates.Insert(0, maybeExe);
        }

        if (pathLower.EndsWith(@"\code.exe"))
            codeExeCandidates.Insert(0, fromPath);
    }

    var codeExe = codeExeCandidates.FirstOrDefault(File.Exists);

    try
    {
        if (codeExe is not null)
        {
            // Best path: invoke Code.exe directly (no cmd window)
            Process.Start(new ProcessStartInfo
            {
                FileName = codeExe,
                UseShellExecute = true,
                ArgumentList = { "--reuse-window", projectPath }
            });
        }
        else
        {
            // Fallback: "code" from PATH (sometimes code.cmd)
            Process.Start(new ProcessStartInfo
            {
                FileName = "code",
                UseShellExecute = true,
                Arguments = $"--reuse-window \"{projectPath}\""
            });
        }

        PrintAndPause("Opening project in VS Code...");
    }
    catch (Exception ex)
    {
        PrintAndPause(
            "VS Code was not found or could not be started.\n" +
            "Tip: Install VS Code and enable the 'code' command in PATH.\n\n" +
            ex.Message
        );
    }
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

        return output
            .Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries)
            .FirstOrDefault()
            ?.Trim();
    }
    catch
    {
        return null;
    }
}

static void ShowMessageComingSoon(string title)
{
    Console.Clear();
    Console.WriteLine($"=== {title} ===");
    Console.WriteLine("(Coming soon)");
    ConsoleUi.Pause();
}

static void PrintAndPause(string message)
{
    Console.WriteLine(message);
    ConsoleUi.Pause();
}
