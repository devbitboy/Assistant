using System.Diagnostics;

namespace Assistant.Infrastructure.Windows;

public sealed class WhereResolver
{
    public static string? TryResolveFirst(string fileName)
    {
        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = "where",
                Arguments = fileName,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var p = Process.Start(psi);
            if (p is null) return null;

            // Leer primero (evita deadlocks por buffers) y luego esperar.
            var output = p.StandardOutput.ReadToEnd();
            var error = p.StandardError.ReadToEnd();

            if (!p.WaitForExit(2000))
            {
                try { p.Kill(entireProcessTree: true); } catch { /* ignore */ }
                return null;
            }

            // Si no hubo output, no resolvió nada (o falló)
            if (string.IsNullOrWhiteSpace(output))
                return null;

            return output
                .Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries)
                .FirstOrDefault()
                ?.Trim();
        }
        catch
        {
            return null;
        }
    }

    public static bool ExistsOnPath(string exe) =>
        TryResolveFirst(exe) is not null;
}
