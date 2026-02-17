using Assistant.Application.Abstractions;
using Assistant.Application.Common;

namespace Assistant.Application.Commands;

public sealed class ShutdownMenuCommand(ISystemPower power)
{
    public Result RestartPc(bool tryCloseAppsFirst)
    {
        try
        {
            if (tryCloseAppsFirst)
                power.TryCloseUserApplicationsGracefully(TimeSpan.FromSeconds(2));

            power.Restart();
            return Result.Success("Restarting PC...");
        }
        catch (Exception ex)
        {
            return Result.Failure("Could not restart PC.\n\n" + ex.Message);
        }
    }

    public Result ShutdownPc(bool tryCloseAppsFirst)
    {
        try
        {
            if (tryCloseAppsFirst)
                power.TryCloseUserApplicationsGracefully(TimeSpan.FromSeconds(2));

            power.Shutdown();
            return Result.Success("Shutting down PC...");
        }
        catch (Exception ex)
        {
            return Result.Failure("Could not shut down PC.\n\n" + ex.Message);
        }
    }

    public Result SignOut(bool tryCloseAppsFirst)
    {
        try
        {
            if (tryCloseAppsFirst)
                power.TryCloseUserApplicationsGracefully(TimeSpan.FromSeconds(2));

            power.SignOut();
            return Result.Success("Signing out...");
        }
        catch (Exception ex)
        {
            return Result.Failure("Could not sign out.\n\n" + ex.Message);
        }
    }
}
