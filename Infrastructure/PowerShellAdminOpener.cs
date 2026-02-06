using System.Diagnostics;

namespace Assistant.Infrastructure;

public sealed class PowerShellAdminOpener : IAdminShellOpener
{
    public void Open()
    {
        var shell = ResolveShell();

        Process.Start(new ProcessStartInfo
        {
            FileName = shell,
            UseShellExecute = true,
            Verb = "runas",
        });
    }

    private static string ResolveShell()
    {
        // Prefer PowerShell 7
        if (ExistsOnPath("pwsh")) return "pwsh";

        // Fallback to Windows PowerShell 5.1
        if (ExistsOnPath("powershell")) return "powershell";

        throw new InvalidOperationException(
            "No PowerShell executable found (pwsh/powershell). Install PowerShell or fix PATH."
        );
    }

    private static bool ExistsOnPath(string exe)
    {
        try
        {
            var p = Process.Start(new ProcessStartInfo
            {
                FileName = "where",
                Arguments = exe,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            });

            if (p is null) return false;
            p.WaitForExit();
            return p.ExitCode == 0;
        }
        catch
        {
            return false;
        }
    }
}
