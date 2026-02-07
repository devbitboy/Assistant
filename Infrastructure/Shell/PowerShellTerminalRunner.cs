using System.Diagnostics;
using Assistant.Application.Abstractions;
using Assistant.Infrastructure.Windows;

namespace Assistant.Infrastructure.Shell;

public sealed class PowerShellTerminalRunner : ITerminalRunner
{
    public void OpenAndRun(string workingDirectory, string script)
    {
        if (!Directory.Exists(workingDirectory))
            throw new DirectoryNotFoundException($"Working directory not found: {workingDirectory}");

        var shell = ResolveShell();

        Process.Start(new ProcessStartInfo
        {
            FileName = shell,
            UseShellExecute = true,
            Arguments = $"-NoLogo -NoExit -Command \"{script}\""
        });
    }

    private static string ResolveShell()
    {
        if (WhereResolver.ExistsOnPath("pwsh")) return "pwsh";
        if (WhereResolver.ExistsOnPath("powershell")) return "powershell";

        throw new InvalidOperationException(
            "No PowerShell executable found (pwsh/powershell)."
        );
    }
}
