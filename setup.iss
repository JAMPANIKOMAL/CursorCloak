; ===================================================================
;  Inno Setup Script for CursorCloak v1.2.0 - Self-Contained Release
;  Professional installer with enhanced features and app icon
; ===================================================================

[Setup]
; App identification
AppId={{11e15daa-a0a7-437c-af53-73b31ab26d83}
AppName=CursorCloak
AppVersion=1.2.0
AppVerName=CursorCloak v1.2.0 - Self-Contained Edition
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
OutputBaseFilename=CursorCloak_Setup_v1.2.0
OutputDir=.\Installer
Compression=lzma2/ultra64
SolidCompression=yes
WizardStyle=modern
ArchitecturesAllowed=x64
ArchitecturesInstallIn64BitMode=x64

; UI and branding
LicenseFile=LICENSE
InfoBeforeFile=README.md
SetupIconFile=CursorCloak.UI\Resources\app-icon.ico

; Installation behavior
CloseApplications=yes
RestartApplications=no
CreateAppDir=yes
DirExistsWarning=auto
EnableDirDoesntExistWarning=yes
UninstallDisplayIcon={app}\CursorCloak.UI.exe

; Enhanced installer experience
ShowLanguageDialog=no
ShowUndisplayableLanguages=no
AppReadmeFile={app}\README.md
UsePreviousAppDir=yes
UsePreviousGroup=yes
AlwaysRestart=no
RestartIfNeededByRun=no

[Messages]
; Custom messages for SmartScreen handling
WelcomeLabel2=This installer may trigger Windows SmartScreen protection. This is normal for new applications.%n%nIf you see a SmartScreen warning:%n• Click "More info"%n• Then click "Run anyway"%n%nThis setup will install [name/ver] on your computer.
FinishedLabel=Setup has finished installing [name] on your computer.%n%nThe application is ready to use with enhanced background mode features!

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "launchafterinstall"; Description: "{cm:LaunchProgram,CursorCloak}"; GroupDescription: "{cm:AdditionalIcons}"

[Files]
; Main application files - Self-contained single file
Source: "CursorCloak.UI\bin\Release\net9.0-windows\win-x64\publish\CursorCloak.UI.exe"; DestDir: "{app}"; Flags: ignoreversion
; Documentation files
Source: "README.md"; DestDir: "{app}"; Flags: ignoreversion
Source: "LICENSE"; DestDir: "{app}"; Flags: ignoreversion
Source: "RELEASE-NOTES-v1.2.0.md"; DestDir: "{app}"; Flags: ignoreversion
Source: "VERSION.md"; DestDir: "{app}"; Flags: ignoreversion

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
