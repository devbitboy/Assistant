using Assistant.Application.Abstractions;
using Assistant.Application.Common;

namespace Assistant.Application.Commands;

public sealed class OpenChatGptCommand(IChatGptOpener opener)
{
    public Result Execute()
    {
        try
        {
            opener.Open();
            return Result.Success("Opening ChatGPT...");
        }
        catch (Exception ex)
        {
            return Result.Failure(
                "ChatGPT could not be started.\n" +
                "Opening browser fallback...\n\n" +
                ex.Message
            );
        }
    }
}
