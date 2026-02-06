using System.Diagnostics;

namespace Assistant.Infrastructure;

public sealed class PowerShellAdminOpener : IAdminShellOpener
{
    public void Open()
    {
        // Start-Process pwsh -Verb RunAs
        Process.Start(new ProcessStartInfo
        {
            FileName = "pwsh",
            UseShellExecute = true,
            Verb = "runas",
        });
    }
}
