using Assistant.Application.Abstractions;
using Assistant.Application.Common;

namespace Assistant.Application.Commands;

public sealed class RunDotnetCleanRunCommand(ITerminalRunner terminal)
{
    public Result Execute(string projectRootPath)
    {
        try
        {
            // Puedes ajustar:
            // - Si quieres forzar un proyecto: dotnet run --project "ruta\algo.csproj"
            // - Si quieres configuration: dotnet run -c Debug
            var command = "dotnet clean; if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }; dotnet run";

            terminal.OpenAndRun(projectRootPath, command);

            return Result.Success("Opening PowerShell and running: dotnet clean; dotnet run");
        }
        catch (Exception ex)
        {
            return Result.Failure(
                "Could not open terminal and run dotnet commands.\n" +
                "Tip: Ensure .NET SDK is installed and 'dotnet' is available in PATH.\n\n" +
                ex.Message
            );
        }
    }
}
