#define MyAppName "Regmarks Hotfolder"
#define MyAppVersion "1.1.9"
#define MyAppPublisher "Killians Streibel"
#define MyAppURL "https://github.com/Killians06"
#define MyAppExeName "Hotfolder.exe"
#define MyPath "D:\a\Regmarks-Hotfolder\Regmarks-Hotfolder\bin\Release\net9.0-windows\publish"
#define MyIconPath "D:\a\Regmarks-Hotfolder\Regmarks-Hotfolder\src\hotfolder.ico"
#define MyOutputPath "D:\a\Regmarks-Hotfolder\Regmarks-Hotfolder\InnoSetup"

[Setup]
AppId={{9D7FEA5A-DBEE-4EEC-9013-284CF13A4DB1}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\{#MyAppName}
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
DisableProgramGroupPage=yes
PrivilegesRequired=lowest
OutputDir={#MyOutputPath}
OutputBaseFilename=RegMarks Hotfolder Installer
SetupIconFile={#MyIconPath}
UninstallDisplayIcon={app}\Hotfolder.exe
Compression=lzma
SolidCompression=yes
WizardStyle=modern

[Languages]
Name: "french"; MessagesFile: "compiler:Languages\French.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "{#MyPath}\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#MyPath}\Hotfolder.deps.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#MyPath}\Hotfolder.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#MyPath}\Hotfolder.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#MyPath}\Hotfolder.pdb"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#MyPath}\Hotfolder.runtimeconfig.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#MyPath}\settings.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#MyPath}\hotfolder.ico"; DestDir: "{app}"; Flags: ignoreversion

[UninstallDelete]
Type: files; Name: "{app}\settings.json"
Type: files; Name: "{app}\hotfolder.ico"

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

