; ===================================================================
;  Inno Setup Script for CursorCloak
; ===================================================================

[Setup]
; NOTE: The AppId is a unique identifier for your application.
; Generate a new GUID if you make another app: https://www.guidgenerator.com/
AppId={{11e15daa-a0a7-437c-af53-73b31ab26d83}
AppName=CursorCloak
AppVersion=1.0
AppPublisher=CursorCloak Project
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
; Uncomment the line below to add an icon (place icon file in Assets directory)
; SetupIconFile=Assets\app-icon.ico

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
; This adds a checkbox on the final page of the installer to launch the app.
Name: "launchafterinstall"; Description: "{cm:LaunchProgram,CursorCloak}"; GroupDescription: "{cm:AdditionalIcons}"

[Files]
; This is the most important line. It tells the installer where to find your compiled .exe
; and where to put it on the user's computer.
; Updated to use the correct publish path
Source: "CursorCloak.UI\bin\Release\net9.0-windows\CursorCloak.UI.exe"; DestDir: "{app}"; Flags: ignoreversion
; Include any necessary runtime files
Source: "CursorCloak.UI\bin\Release\net9.0-windows\*.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "CursorCloak.UI\bin\Release\net9.0-windows\*.json"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
; Creates a shortcut in the Start Menu.
Name: "{group}\CursorCloak"; Filename: "{app}\CursorCloak.UI.exe"
; Creates a shortcut to the uninstaller.
Name: "{group}\Uninstall CursorCloak"; Filename: "{uninstallexe}"
; Desktop shortcut
Name: "{autodesktop}\CursorCloak"; Filename: "{app}\CursorCloak.UI.exe"; Tasks: desktopicon

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "launchafterinstall"; Description: "{cm:LaunchProgram,CursorCloak}"; GroupDescription: "{cm:AdditionalIcons}"

[Run]
; This runs the application after the installation is complete if the user checked the box.
; Using runas verb to ensure it runs as administrator
Filename: "{app}\CursorCloak.UI.exe"; Description: "{cm:LaunchProgram,CursorCloak}"; Flags: nowait postinstall skipifsilent runascurrentuser; Tasks: launchafterinstall
