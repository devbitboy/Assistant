namespace Assistant.Application.Abstractions;

public interface ISystemPower
{
    /// <summary>Best-effort graceful close of user apps (asks nicely, waits).</summary>
    void TryCloseUserApplicationsGracefully(TimeSpan timeoutPerApp);

    void Restart();
    void Shutdown();
    void SignOut();
}
