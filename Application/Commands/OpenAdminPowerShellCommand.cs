using Assistant.Application.Abstractions;
using Assistant.Application.Common;

namespace Assistant.Application.Commands;

public sealed class OpenAdminPowerShellCommand(IAdminShellOpener opener)
{
    public Result Execute()
    {
        try
        {
            opener.Open();
            return Result.Success("Opening PowerShell as Administrator...");
        }
        catch (Exception ex)
        {
            return Result.Failure(
                "PowerShell could not be started as Administrator.\n" +
                "Tip: Ensure PowerShell 7 (pwsh) is installed and available in PATH.\n\n" +
                ex.Message
            );
        }
    }
}
