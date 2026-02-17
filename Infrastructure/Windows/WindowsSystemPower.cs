using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

using Assistant.Application.Abstractions;

namespace Assistant.Infrastructure.Windows;

public sealed class WindowsSystemPower : ISystemPower
{
    public void TryCloseUserApplicationsGracefully(TimeSpan timeoutPerApp)
    {
        // Best-effort:
        // - Enumerate processes with a MainWindowHandle
        // - Send CloseMainWindow
        // - Wait a little for exit
        //
        // We avoid services/system processes and avoid killing anything.
        // This won't close background apps without a main window.

        var currentPid = Environment.ProcessId;

        foreach (var p in Process.GetProcesses())
        {
            try
            {
                if (p.Id == currentPid) continue;
                if (p.HasExited) continue;

                // only apps with UI
                if (p.MainWindowHandle == IntPtr.Zero) continue;

                // Skip some known shells (optional)
                var name = p.ProcessName.ToLowerInvariant();
                if (name is "explorer" or "svchost" or "services") continue;

                // Ask nicely
                p.CloseMainWindow();

                // Wait for exit (per app)
                if (timeoutPerApp > TimeSpan.Zero)
                    p.WaitForExit((int)timeoutPerApp.TotalMilliseconds);
            }
            catch
            {
                // ignore and continue
            }
        }
    }

    public void Restart()
        => RunShutdownExe("/r /t 5");

    public void Shutdown()
        => RunShutdownExe("/s /t 5");

    public void SignOut()
        => RunShutdownExe("/l");

    private static void RunShutdownExe(string args)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = "shutdown.exe",
            Arguments = args,
            UseShellExecute = true,
            CreateNoWindow = true
        });
    }
}
