using Assistant.Application.Commands;

namespace Assistant.ConsoleApp;

public sealed class App(
    OpenAdminPowerShellCommand openAdminPs,
    OpenChatGptCommand openChatGpt,
    OpenVsCodeFolderCommand openVsCodeFolder,
    RestartAssistantCommand restartAssistant,
    ShutdownMenuCommand shutdownMenu,
    UpdateAssistantCommand updateAssistant,
    string projectRootPath)

{
    public void Run()
    {
        List<MenuOption> mainMenu =
[
    new("Settings", ShowSettings),
    new("Projects", ShowProjects),
    new("ChatGPT", OpenChatGpt),
    new("Commands", ShowCommands),
    new("Open PowerShell (Admin)", OpenPowerShellAdmin),
    new("Shutdown", ShowShutdown),
];


        RunMenu("Assistant", mainMenu, "Exit", onExit: () => Console.WriteLine("Bye!"));
    }

    // ================================
    // Screens (UI)
    // ================================
    private void ShowShutdown() =>
        RunMenu("Shutdown",
        [
            new("Restart PC (try close apps first)", () => RestartPc(true)),
        new("Shutdown PC (try close apps first)", () => ShutdownPc(true)),
        new("Sign out (try close apps first)", () => SignOut(true)),

        // Opcionales “sin intentar cerrar apps”
        new("Restart PC (immediate)", () => RestartPc(false)),
        new("Shutdown PC (immediate)", () => ShutdownPc(false)),
        ],
        "Back");

    private void RestartPc(bool tryCloseAppsFirst)
    {
        var result = shutdownMenu.RestartPc(tryCloseAppsFirst);
        PrintAndPause(result.Message ?? "Restarting PC...");
    }

    private void ShutdownPc(bool tryCloseAppsFirst)
    {
        var result = shutdownMenu.ShutdownPc(tryCloseAppsFirst);
        PrintAndPause(result.Message ?? "Shutting down PC...");
    }

    private void SignOut(bool tryCloseAppsFirst)
    {
        var result = shutdownMenu.SignOut(tryCloseAppsFirst);
        PrintAndPause(result.Message ?? "Signing out...");
    }

    private void ShowSettings() =>
    RunMenu("Settings",
    [
        new("Open Project in VS Code", () => OpenVsCode(projectRootPath)),
        new("Restart Assistant (clean + run)", RestartAssistant),
        new("Update Assistant (publish + swap + restart)", UpdateAssistant),
    ],
    "Back");

    private void UpdateAssistant()
    {
        var result = updateAssistant.Execute(projectRootPath);
        PrintAndPause(result.Message ?? "Updating Assistant...");
    }
    private void ShowProjects() =>
        RunMenu("Projects",
        [
            new("Project Intelliflow", () => ShowMessageComingSoon("Project Intelliflow")),
        ],
        "Back");

    private void ShowCommands() =>
        ShowMessageComingSoon("Commands");

    // ================================
    // Actions (UI -> Application)
    // ================================

    private void OpenPowerShellAdmin()
    {
        var result = openAdminPs.Execute();
        if (result.IsSuccess)
            PrintAndPause(result.Message ?? "Opening PowerShell as Administrator...");
        else
            PrintAndPause(result.Message ?? "PowerShell could not be started as Administrator.");
    }

    private void OpenChatGpt()
    {
        var result = openChatGpt.Execute();
        if (result.IsSuccess)
            PrintAndPause(result.Message ?? "Opening ChatGPT...");
        else
            PrintAndPause(result.Message ?? "ChatGPT could not be started.");
    }

    private void OpenVsCode(string path)
    {
        var result = openVsCodeFolder.Execute(path);
        if (result.IsSuccess)
            PrintAndPause(result.Message ?? "Opening project in VS Code...");
        else
            PrintAndPause(result.Message ?? "VS Code could not be started.");
    }
    private void RestartAssistant()
    {
        var result = restartAssistant.Execute(projectRootPath);

        if (result.IsSuccess)
            PrintAndPause(result.Message ?? "Restarting Assistant...");
        else
            PrintAndPause(result.Message ?? "Could not restart Assistant.");
    }
    // ================================
    // Menu Engine
    // ================================

    private static void RunMenu(
        string title,
        IReadOnlyList<MenuOption> options,
        string exitLabel,
        Action? onExit = null)
    {
        while (true)
        {
            PrintHeader(title);

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

    // ================================
    // UI Helpers
    // ================================

    private static void ShowMessageComingSoon(string title)
    {
        PrintHeader(title);
        Console.WriteLine("(Coming soon)");
        ConsoleUi.Pause();
    }

    private static void PrintHeader(string title)
    {
        Console.Clear();
        Console.WriteLine($"=== {title} ===\n");
    }

    private static void PrintAndPause(string message)
    {
        Console.WriteLine(message);
        ConsoleUi.Pause();
    }
}
