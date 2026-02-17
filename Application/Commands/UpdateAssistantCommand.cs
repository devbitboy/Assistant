using Assistant.Application.Abstractions;
using Assistant.Application.Common;

namespace Assistant.Application.Commands;

public sealed class UpdateAssistantCommand(ITerminalRunner terminal)
{
    public Result Execute(string projectRootPath)
    {
        try
        {
            var script = $$"""
            Set-Location "{{projectRootPath}}"
            powershell -NoLogo -ExecutionPolicy Bypass -File ".\update-and-restart.ps1"
            """;

            terminal.OpenAndRun(projectRootPath, script);
            return Result.Success("Updating Assistant (publish + swap + restart)...");
        }
        catch (Exception ex)
        {
            return Result.Failure("Could not update Assistant.\n\n" + ex.Message);
        }
    }
}
