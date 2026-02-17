using System.Diagnostics;
using Assistant.Application.Abstractions;

namespace Assistant.Infrastructure.Apps;

public sealed class WindowsChatGptOpener : IChatGptOpener
{
    public void Open()
    {
        try
        {
            const string chatGptAppId =
                "shell:AppsFolder\\OpenAI.ChatGPT-Desktop_2p2nqsd0c76g0!ChatGPT";

            Process.Start(new ProcessStartInfo
            {
                FileName = "cmd.exe",
                UseShellExecute = true,
                Arguments = $"/c start \"\" \"{chatGptAppId}\""
            });
        }
        catch
        {
            // fallback navegador (misma conducta que tenías)
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://chatgpt.com/",
                UseShellExecute = true
            });
        }
    }
}
