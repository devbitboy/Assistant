using Assistant.Infrastructure;

namespace Assistant.Application;

public sealed class OpenAdminPowerShellCommand(IAdminShellOpener opener)
{
    public void Execute()
    {
        opener.Open();
    }
}
