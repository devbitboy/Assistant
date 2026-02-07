using Assistant.Application.Abstractions;
using Assistant.Application.Commands;
using Assistant.ConsoleApp;
using Assistant.Infrastructure.Apps;
using Assistant.Infrastructure.Shell;

IAdminShellOpener adminShellOpener = new PowerShellAdminOpener();
IChatGptOpener chatGptOpener = new WindowsChatGptOpener();
IVsCodeOpener vsCodeOpener = new WindowsVsCodeOpener();

var openAdminPs = new OpenAdminPowerShellCommand(adminShellOpener);
var openChatGpt = new OpenChatGptCommand(chatGptOpener);
var openVsCode = new OpenVsCodeFolderCommand(vsCodeOpener);

var app = new App(
    openAdminPs,
    openChatGpt,
    openVsCode,
    projectRootPath: @"C:\dev\Assistant"
);

app.Run();
