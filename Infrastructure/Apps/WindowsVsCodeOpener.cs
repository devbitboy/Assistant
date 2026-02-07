using System.Diagnostics;
using Assistant.Application.Abstractions;
using Assistant.Infrastructure.Windows;

namespace Assistant.Infrastructure.Apps;

public sealed class WindowsVsCodeOpener : IVsCodeOpener
{
    public void OpenFolder(string folderPath)
    {
        if (!Directory.Exists(folderPath))
            throw new DirectoryNotFoundException($"Project path not found: {folderPath}");

        var codeExeCandidates = new List<string>
        {
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                @"Programs\Microsoft VS Code\Code.exe"
            ),
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                @"Microsoft VS Code\Code.exe"
            ),
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                @"Microsoft VS Code\Code.exe"
            ),
        };

        var fromPath = WhereResolver.TryResolveFirst("code")
            ?? WhereResolver.TryResolveFirst("code.cmd")
            ?? WhereResolver.TryResolveFirst("code.exe");


        if (!string.IsNullOrWhiteSpace(fromPath))
        {
            var pathLower = fromPath.ToLowerInvariant();

            if (pathLower.EndsWith(@"\bin\code.cmd") || pathLower.EndsWith(@"\bin\code"))
            {
                var maybeExe = Path.GetFullPath(
                    Path.Combine(Path.GetDirectoryName(fromPath)!, @"..\Code.exe")
                );
                codeExeCandidates.Insert(0, maybeExe);
            }

            if (pathLower.EndsWith(@"\code.exe"))
                codeExeCandidates.Insert(0, fromPath);
        }

        var codeExe = codeExeCandidates.FirstOrDefault(File.Exists);

        if (codeExe is not null)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = codeExe,
                UseShellExecute = true,
                ArgumentList = { "--reuse-window", folderPath }
            });
            return;
        }

        // fallback: "code" por PATH
        Process.Start(new ProcessStartInfo
        {
            FileName = "code",
            UseShellExecute = true,
            Arguments = $"--reuse-window \"{folderPath}\""
        });
    }
}
