; ===================================================================
;  Inno Setup Script for CursorCloak - Optimized
; ===================================================================

[Setup]
; NOTE: The AppId is a unique identifier for your application.
; Generate a new GUID if you make another app: https://www.guidgenerator.com/
AppId={{11e15daa-a0a7-437c-af53-73b31ab26d83}
AppName=CursorCloak
AppVersion=1.0.0
AppVerName=CursorCloak 1.0.0
AppPublisher=CursorCloak Project
AppPublisherURL=https://github.com/JAMPANIKOMAL/CursorCloak
AppSupportURL=https://github.com/JAMPANIKOMAL/CursorCloak/issues
AppUpdatesURL=https://github.com/JAMPANIKOMAL/CursorCloak/releases
DefaultDirName={autopf}\CursorCloak
DefaultGroupName=CursorCloak
DisableProgramGroupPage=yes
; This line ensures the installer will request administrator privileges.
PrivilegesRequired=admin
; The final name of the installer file.
OutputBaseFilename=CursorCloak_Setup
; Where the installer will be created.
OutputDir=.\Installer
Compression=lzma
SolidCompression=yes
WizardStyle=modern
; License and info files
LicenseFile=LICENSE
InfoBeforeFile=README.md
; Uncomment the line below to add an icon (place icon file in Assets directory)
; SetupIconFile=Assets\app-icon.ico

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
