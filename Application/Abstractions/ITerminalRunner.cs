namespace Assistant.Application.Abstractions;

public interface ITerminalRunner
{
    void OpenAndRun(string workingDirectory, string script);
}
