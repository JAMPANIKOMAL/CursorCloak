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
; Custom messages for SmartScreen handling and user guidance
WelcomeLabel2=This installer will install CursorCloak v1.2.0 - Self-Contained Edition on your computer.%n%n🔒 WINDOWS SMARTSCREEN NOTICE:%nThis installer may trigger Windows SmartScreen protection because it's not digitally signed. This is normal for open-source applications.%n%nIf you see a SmartScreen warning:%n• Click "More info"%n• Then click "Run anyway"%n%n✅ This software is open source and safe to install.%n📂 Source code: https://github.com/JAMPANIKOMAL/CursorCloak

WizardInfoBefore=Release Information
InfoBeforeLabel=CursorCloak v1.2.0 - Self-Contained Edition%n%n🚀 WHAT'S NEW:%n• No .NET runtime required - works on any Windows PC!%n• Custom app icon with professional design%n• Single file deployment - everything included%n• Enhanced background mode operation%n%n🎮 FEATURES:%n• Alt+H to hide cursor, Alt+S to show cursor%n• Runs in background when window is closed%n• Persistent settings between sessions%n• Administrator privilege handling%n%n⚙️ REQUIREMENTS:%n• Windows 10 or later%n• Administrator privileges%n• No additional software needed%n%n📋 AFTER INSTALLATION:%n• Launch from Start Menu or Desktop%n• Right-click and "Run as administrator"%n• Use Alt+H/Alt+S hotkeys anywhere%n%n📄 Full documentation included in installation folder.

FinishedLabel=Setup has successfully installed CursorCloak v1.2.0 on your computer!%n%n🎉 READY TO USE:%n• Launch CursorCloak as administrator%n• Use Alt+H to hide cursor, Alt+S to show%n• App runs in background when closed%n%n📚 Need help? Check the README.md file in the installation folder or visit:%nhttps://github.com/JAMPANIKOMAL/CursorCloak

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
Source: "SMARTSCREEN-INFO.md"; DestDir: "{app}"; Flags: ignoreversion

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
var
  InfoPage: TOutputMsgMemoWizardPage;

procedure InitializeWizard();
begin
  // Create a custom page to show release information
  InfoPage := CreateOutputMsgMemoPage(wpLicense,
    'Release Information', 'CursorCloak v1.2.0 - Self-Contained Edition',
    'Please review the release information below:', '');
    
  // Add release notes content
  InfoPage.RichEditViewer.Lines.Add('🚀 CURSORCLOAK v1.2.0 - SELF-CONTAINED EDITION');
  InfoPage.RichEditViewer.Lines.Add('Release Date: August 7, 2025');
  InfoPage.RichEditViewer.Lines.Add('');
  InfoPage.RichEditViewer.Lines.Add('✨ WHAT''S NEW IN v1.2.0:');
  InfoPage.RichEditViewer.Lines.Add('• 📦 Self-Contained: No .NET runtime required!');
  InfoPage.RichEditViewer.Lines.Add('• 🎨 Custom App Icon: Professional mouse cursor design');
  InfoPage.RichEditViewer.Lines.Add('• 📁 Single File: Everything bundled into one executable');
  InfoPage.RichEditViewer.Lines.Add('• 🗑️ Optimized: Streamlined for easy distribution');
  InfoPage.RichEditViewer.Lines.Add('• 🔧 Enhanced Build: All dependencies included');
  InfoPage.RichEditViewer.Lines.Add('');
  InfoPage.RichEditViewer.Lines.Add('🎯 KEY BENEFITS:');
  InfoPage.RichEditViewer.Lines.Add('• Zero Dependencies: Works on any Windows 10/11 PC');
  InfoPage.RichEditViewer.Lines.Add('• Portable: Single EXE file you can run anywhere');
  InfoPage.RichEditViewer.Lines.Add('• Professional: Easy to identify with custom icon');
  InfoPage.RichEditViewer.Lines.Add('• Clean: No unnecessary files or clutter');
  InfoPage.RichEditViewer.Lines.Add('');
  InfoPage.RichEditViewer.Lines.Add('🎮 CORE FEATURES:');
  InfoPage.RichEditViewer.Lines.Add('• ⌨️ Global Hotkeys: Alt+H to hide, Alt+S to show cursor');
  InfoPage.RichEditViewer.Lines.Add('• 🔄 Background Mode: Keeps running when window is closed');
  InfoPage.RichEditViewer.Lines.Add('• 🎯 No Tray Icon: Clean operation without clutter');
  InfoPage.RichEditViewer.Lines.Add('• 💾 Persistent Settings: Remembers your preferences');
  InfoPage.RichEditViewer.Lines.Add('• 🔐 Admin Protection: Handles privileges automatically');
  InfoPage.RichEditViewer.Lines.Add('');
  InfoPage.RichEditViewer.Lines.Add('⚡ TECHNICAL DETAILS:');
  InfoPage.RichEditViewer.Lines.Add('• Platform: Windows 10/11 (x64)');
  InfoPage.RichEditViewer.Lines.Add('• Size: ~60MB (includes .NET runtime)');
  InfoPage.RichEditViewer.Lines.Add('• Requirements: Administrator privileges only');
  InfoPage.RichEditViewer.Lines.Add('• No external dependencies needed');
  InfoPage.RichEditViewer.Lines.Add('');
  InfoPage.RichEditViewer.Lines.Add('📚 USAGE:');
  InfoPage.RichEditViewer.Lines.Add('1. Launch CursorCloak as administrator');
  InfoPage.RichEditViewer.Lines.Add('2. Use Alt+H to hide cursor anywhere');
  InfoPage.RichEditViewer.Lines.Add('3. Use Alt+S to show cursor again');
  InfoPage.RichEditViewer.Lines.Add('4. Close window to run in background');
  InfoPage.RichEditViewer.Lines.Add('5. Settings are saved automatically');
  InfoPage.RichEditViewer.Lines.Add('');
  InfoPage.RichEditViewer.Lines.Add('🔗 MORE INFORMATION:');
  InfoPage.RichEditViewer.Lines.Add('• Documentation: README.md in installation folder');
  InfoPage.RichEditViewer.Lines.Add('• Source Code: https://github.com/JAMPANIKOMAL/CursorCloak');
  InfoPage.RichEditViewer.Lines.Add('• Issues/Support: https://github.com/JAMPANIKOMAL/CursorCloak/issues');
  InfoPage.RichEditViewer.Lines.Add('• Latest Releases: https://github.com/JAMPANIKOMAL/CursorCloak/releases');
end;

procedure CursorMoveProc(X, Y: Integer);
begin
  // This procedure is called when the mouse cursor is moved during installation
  // Can be used for custom animations or effects
end;

function InitializeSetup(): Boolean;
begin
  // Since this is self-contained, we don't need .NET checks
  Result := True;
  
  // Check Windows version (Windows 10 or later)
  if GetWindowsVersion < $0A000000 then
  begin
    MsgBox('CursorCloak requires Windows 10 or later. Your current Windows version is not supported.', mbError, MB_OK);
    Result := False;
  end;
end;

procedure DeinitializeSetup();
begin
  // Cleanup code when setup exits
end;
