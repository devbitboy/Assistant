using Assistant.Application.Abstractions;
using Assistant.Application.Common;

namespace Assistant.Application.Commands;

public sealed class OpenVsCodeFolderCommand(IVsCodeOpener opener)
{
    public Result Execute(string folderPath)
    {
        try
        {
            opener.OpenFolder(folderPath);
            return Result.Success("Opening project in VS Code...");
        }
        catch (Exception ex)
        {
            return Result.Failure(
                "VS Code was not found or could not be started.\n" +
                "Tip: Install VS Code and enable the 'code' command in PATH.\n\n" +
                ex.Message
            );
        }
    }
}
