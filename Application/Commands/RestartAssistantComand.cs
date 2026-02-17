using Assistant.Application.Abstractions;
using Assistant.Application.Common;

namespace Assistant.Application.Commands;

public sealed class RestartAssistantCommand(ITerminalRunner terminal)
{
    public Result Execute(string projectRootPath)
    {
        try
        {
            var script = $$"""
            Start-Sleep -Seconds 1

            Get-Process Assistant -ErrorAction SilentlyContinue | Stop-Process -Force

            Set-Location "{{projectRootPath}}"

            dotnet clean
            if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

            dotnet run
            """;

            terminal.OpenAndRun(projectRootPath, script);
            return Result.Success("Restarting Assistant...");
        }
        catch (Exception ex)
        {
            return Result.Failure("Could not restart Assistant.\n\n" + ex.Message);
        }
    }
}
