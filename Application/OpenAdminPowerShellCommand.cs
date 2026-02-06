using Assistant.Infrastructure;

namespace Assistant.Application;

public sealed class OpenAdminPowerShellCommand
{
    private readonly IAdminShellOpener _opener;

    public OpenAdminPowerShellCommand(IAdminShellOpener opener)
    {
        _opener = opener;
    }

    public void Execute()
    {
        _opener.Open();
    }
}
