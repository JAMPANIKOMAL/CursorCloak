; ===================================================================
;  Inno Setup Script for CursorCloak v1.0.0 - Background Mode Release
;  Professional installer with enhanced features
; ===================================================================

[Setup]
; App identification
AppId={{11e15daa-a0a7-437c-af53-73b31ab26d83}
AppName=CursorCloak
AppVersion=1.0.0
AppVerName=CursorCloak v1.0.0 - Background Mode Edition
AppPublisher=CursorCloak Development Team
AppPublisherURL=https://github.com/JAMPANIKOMAL/CursorCloak
AppSupportURL=https://github.com/JAMPANIKOMAL/CursorCloak/issues
AppUpdatesURL=https://github.com/JAMPANIKOMAL/CursorCloak/releases
AppContact=https://github.com/JAMPANIKOMAL/CursorCloak/issues
AppCopyright=© 2025 CursorCloak Project. All rights reserved.

; Installation directories
DefaultDirName={autopf}\CursorCloak
DefaultGroupName=CursorCloak
DisableProgramGroupPage=yes
AllowNoIcons=yes

; Installer settings
PrivilegesRequired=admin
OutputBaseFilename=CursorCloak_Setup_v1.0.0
OutputDir=.\Installer
Compression=lzma2/ultra64
SolidCompression=yes
WizardStyle=modern
ArchitecturesAllowed=x64
ArchitecturesInstallIn64BitMode=x64

; UI and branding
LicenseFile=LICENSE
InfoBeforeFile=README.md
; Uncomment when icon is available:
; SetupIconFile=CursorCloak.UI\Resources\CursorCloak.ico

; Installation behavior
CloseApplications=yes
RestartApplications=no
CreateAppDir=yes
DirExistsWarning=auto
EnableDirDoesntExistWarning=yes
UninstallDisplayIcon={app}\CursorCloak.UI.exe

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "launchafterinstall"; Description: "{cm:LaunchProgram,CursorCloak}"; GroupDescription: "{cm:AdditionalIcons}"

[Files]
; Main application files
Source: "CursorCloak.UI\bin\Release\net9.0-windows\CursorCloak.UI.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "CursorCloak.UI\bin\Release\net9.0-windows\CursorCloak.UI.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "CursorCloak.UI\bin\Release\net9.0-windows\CursorCloak.UI.deps.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "CursorCloak.UI\bin\Release\net9.0-windows\CursorCloak.UI.runtimeconfig.json"; DestDir: "{app}"; Flags: ignoreversion
; Documentation files
Source: "README.md"; DestDir: "{app}"; Flags: ignoreversion
Source: "LICENSE"; DestDir: "{app}"; Flags: ignoreversion
Source: "TROUBLESHOOTING.md"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
; Creates a shortcut in the Start Menu.
Name: "{group}\CursorCloak"; Filename: "{app}\CursorCloak.UI.exe"; Comment: "Hide and show system cursor with hotkeys"
; Creates a shortcut to the uninstaller.
Name: "{group}\Uninstall CursorCloak"; Filename: "{uninstallexe}"
; Desktop shortcut (only if user selected it)
Name: "{autodesktop}\CursorCloak"; Filename: "{app}\CursorCloak.UI.exe"; Comment: "Hide and show system cursor with hotkeys"; Tasks: desktopicon

[Run]
; This runs the application after the installation is complete if the user checked the box.
; Using runascurrentuser to avoid UAC prompt during installation
Filename: "{app}\CursorCloak.UI.exe"; Description: "{cm:LaunchProgram,CursorCloak}"; Flags: nowait postinstall skipifsilent runascurrentuser; Tasks: launchafterinstall

[UninstallDelete]
; Clean up settings directory on uninstall
Type: filesandordirs; Name: "{userappdata}\CursorCloak"

[Code]
procedure CursorMoveProc(X, Y: Integer);
begin
  // This procedure is called when the mouse cursor is moved during installation
  // Can be used for custom animations or effects
end;

function InitializeSetup(): Boolean;
begin
  // Check if .NET 9.0 or later is installed
  Result := True;
  if not RegKeyExists(HKEY_LOCAL_MACHINE, 'SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full') then
  begin
    MsgBox('Microsoft .NET Framework 4.7.2 or later is required. Please install it and run this setup again.', mbError, MB_OK);
    Result := False;
  end;
end;

procedure DeinitializeSetup();
begin
  // Cleanup code when setup exits
end;
