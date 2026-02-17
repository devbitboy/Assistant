using System.Diagnostics;
using System.Text;
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
        var encoded = Encode(script);

        Process.Start(new ProcessStartInfo
        {
            FileName = shell,
            UseShellExecute = true,
            Arguments = $"-NoLogo -NoExit -EncodedCommand {encoded}",
            WorkingDirectory = workingDirectory
        });
    }

    private static string Encode(string script)
    {
        // PowerShell requiere UTF16-LE
        var bytes = Encoding.Unicode.GetBytes(script);
        return Convert.ToBase64String(bytes);
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
