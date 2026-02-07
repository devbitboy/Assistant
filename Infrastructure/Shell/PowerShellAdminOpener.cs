using System.Diagnostics;
using Assistant.Application.Abstractions;
using Assistant.Infrastructure.Windows;

namespace Assistant.Infrastructure.Shell;

public sealed class PowerShellAdminOpener : IAdminShellOpener
{
    public void Open()
    {
        var shell = ResolveShell();

        System.Diagnostics.Process.Start(new ProcessStartInfo
        {
            FileName = shell,
            UseShellExecute = true,
            Verb = "runas",
        });
    }

    private string ResolveShell()
    {
        if (WhereResolver.ExistsOnPath("pwsh")) return "pwsh";
        if (WhereResolver.ExistsOnPath("powershell")) return "powershell";

        throw new InvalidOperationException(
            "No PowerShell executable found (pwsh/powershell). Install PowerShell or fix PATH."
        );
    }
}
