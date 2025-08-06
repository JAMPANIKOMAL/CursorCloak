; ===================================================================
;  Inno Setup Script for CursorCloak
; ===================================================================

[Setup]
; NOTE: The AppId is a unique identifier for your application.
; Generate a new GUID if you make another app: https://www.guidgenerator.com/
AppId={{11e15daa-a0a7-437c-af53-73b31ab26d83}
AppName=CursorCloak
AppVersion=1.0
AppPublisher=Your Name
DefaultDirName={autopf}\CursorCloak
DefaultGroupName=CursorCloak
DisableProgramGroupPage=yes
; The final name of the installer file.
OutputBaseFilename=CursorCloak_Setup
; Where the installer will be created.
OutputDir=.\Installer
Compression=lzma
SolidCompression=yes
WizardStyle=modern

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
; This adds a checkbox on the final page of the installer to launch the app.
Name: "launchafterinstall"; Description: "{cm:LaunchProgram,CursorCloak}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
; This is the most important line. It tells the installer where to find your compiled .exe
; and where to put it on the user's computer.
; FIX: Updated the source path to match the .NET 9.0 output directory.
Source: "CursorCloak.UI\bin\Release\net9.0-windows\win-x64\publish\CursorCloak.UI.exe"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
; Creates a shortcut in the Start Menu.
Name: "{group}\CursorCloak"; Filename: "{app}\CursorCloak.UI.exe"
; Creates a shortcut to the uninstaller.
Name: "{group}\Uninstall CursorCloak"; Filename: "{uninstallexe}"

[Run]
; This runs the application after the installation is complete if the user checked the box.
Filename: "{app}\CursorCloak.UI.exe"; Description: "{cm:LaunchProgram,CursorCloak}"; Flags: nowait postinstall skipifsilent; Tasks: launchafterinstall
