using Assistant.Application.Abstractions;
using Assistant.Application.Commands;
using Assistant.ConsoleApp;
using Assistant.Infrastructure.Apps;
using Assistant.Infrastructure.Shell;

IAdminShellOpener adminShellOpener = new PowerShellAdminOpener();
IChatGptOpener chatGptOpener = new WindowsChatGptOpener();
IVsCodeOpener vsCodeOpener = new WindowsVsCodeOpener();

ITerminalRunner terminalRunner = new PowerShellTerminalRunner();

var openAdminPs = new OpenAdminPowerShellCommand(adminShellOpener);
var openChatGpt = new OpenChatGptCommand(chatGptOpener);
var openVsCode = new OpenVsCodeFolderCommand(vsCodeOpener);
var restartAssistant = new RestartAssistantCommand(terminalRunner);

var app = new App(
    openAdminPs,
    openChatGpt,
    openVsCode,
    restartAssistant,
    projectRootPath: @"C:\dev\Assistant"
);

app.Run();
