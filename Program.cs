using Assistant.Application.Abstractions;
using Assistant.Application.Commands;
using Assistant.ConsoleApp;
using Assistant.Infrastructure.Apps;
using Assistant.Infrastructure.Shell;
using Assistant.Infrastructure.Windows;

IAdminShellOpener adminShellOpener = new PowerShellAdminOpener();
IChatGptOpener chatGptOpener = new WindowsChatGptOpener();
IVsCodeOpener vsCodeOpener = new WindowsVsCodeOpener();

ITerminalRunner terminalRunner = new PowerShellTerminalRunner();

ISystemPower systemPower = new WindowsSystemPower();

var openAdminPs = new OpenAdminPowerShellCommand(adminShellOpener);
var openChatGpt = new OpenChatGptCommand(chatGptOpener);
var openVsCode = new OpenVsCodeFolderCommand(vsCodeOpener);
var restartAssistant = new RestartAssistantCommand(terminalRunner);

var shutdownMenu = new ShutdownMenuCommand(systemPower);

var app = new App(
    openAdminPs,
    openChatGpt,
    openVsCode,
    restartAssistant,
    shutdownMenu,
    projectRootPath: @"C:\dev\Assistant"
);

app.Run();
