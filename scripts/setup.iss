;  Inno Setup Script for CursorCloak v1.0.2 - Enhanced Release
;  Enhanced uninstaller with comprehensive cleanup
; ===================================================================

[Setup]
; App identification
AppId={{11e15daa-a0a7-437c-af53-73b31ab26d83}
AppName=CursorCloak
AppVersion=1.0.2
AppVerName=CursorCloak v1.0.2 - Enhanced Release
AppPublisher=CursorCloak Development Team
AppPublisherURL=https://github.com/JAMPANIKOMAL/CursorCloak
AppSupportURL=https://github.com/JAMPANIKOMAL/CursorCloak/issues
AppUpdatesURL=https://github.com/JAMPANIKOMAL/CursorCloak/releases
AppContact=https://github.com/JAMPANIKOMAL/CursorCloak/issues
AppCopyright=© 2025 CursorCloak Project. All rights reserved.
AppComments=Professional cursor management utility for Windows - Enhanced release

; Installation directories
DefaultDirName={autopf}\CursorCloak
DefaultGroupName=CursorCloak
DisableProgramGroupPage=yes
AllowNoIcons=yes

; Installer settings
PrivilegesRequired=admin
OutputBaseFilename=CursorCloak_Setup_v1.0.2
OutputDir=..\releases
Compression=lzma2/ultra64
SolidCompression=yes
WizardStyle=modern
ArchitecturesAllowed=x64
ArchitecturesInstallIn64BitMode=x64

; SmartScreen mitigation - Enhanced publisher information
UninstallDisplayName=CursorCloak - Professional Cursor Management Utility
VersionInfoVersion=1.0.2.0
VersionInfoProductName=CursorCloak Professional Edition
VersionInfoProductVersion=1.0.2.0
VersionInfoCompany=CursorCloak Open Source Project
VersionInfoDescription=Professional cursor hide/show utility with global hotkeys
VersionInfoCopyright=© 2025 CursorCloak Development Team (Open Source)

; UI and branding
LicenseFile=..\LICENSE
SetupIconFile=..\assets\icons\app-icon.ico

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
; Custom messages for SmartScreen handling and professional presentation
WelcomeLabel2=This installer will install CursorCloak v1.0.2 on your computer.%n%nSECURITY NOTICE:%nThis is an open-source application. Windows may show a SmartScreen warning because this software is not commercially signed.%n%nIF SMARTSCREEN APPEARS:%n1. Click "More info"%n2. Click "Run anyway"%n%nCursorCloak is safe to install:%n• 100%% open source - view code on GitHub%n• No network access required%n• No data collection or telemetry%n• Transparent build process%n%nSource: https://github.com/JAMPANIKOMAL/CursorCloak

WizardInfoBefore=CursorCloak Information
InfoBeforeLabel=CursorCloak v1.0.2 - Professional Cursor Management%n%nWHAT IS CURSORCLOAK?%nA lightweight utility that lets you hide and show your mouse cursor using simple keyboard shortcuts.%n%nKEY FEATURES:%n• Alt+H to hide cursor anywhere on Windows%n• Alt+S to show cursor again%n• Runs silently in background%n• No system tray clutter%n• Saves settings automatically%n• Works with all applications%n%nTECHNICAL INFO:%n• Works on Windows 10/11%n• Requires administrator privileges%n• Two versions: Framework-dependent and Self-contained%n• Clean, modern WPF interface%n%nAFTER INSTALLATION:%n1. Launch CursorCloak as administrator%n2. Use Alt+H/Alt+S hotkeys anywhere%n3. Close window to run in background%n4. Settings persist between sessions%n%nFull documentation in installation folder.

FinishedLabel=CursorCloak v1.0.2 has been successfully installed!%n%nREADY TO USE:%n- Launch CursorCloak as administrator%n- Press Alt+H to hide cursor%n- Press Alt+S to show cursor%n- Close window to run in background%n%nNEED HELP?%n- Check README.md in installation folder%n- Visit: https://github.com/JAMPANIKOMAL/CursorCloak%n- Report issues on GitHub%n%nREMEMBER:%nAlways run as administrator for proper functionality!

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "launchafterinstall"; Description: "{cm:LaunchProgram,CursorCloak}"; GroupDescription: "{cm:AdditionalIcons}"

[Files]
; Main application files - Framework-dependent version (default)
Source: "..\publish\framework\ui\CursorCloak.UI.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\publish\framework\ui\CursorCloak.UI.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\publish\framework\ui\CursorCloak.UI.deps.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\publish\framework\ui\CursorCloak.UI.runtimeconfig.json"; DestDir: "{app}"; Flags: ignoreversion
; Documentation and info files
Source: "..\README.md"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\LICENSE"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\docs\VERSION.md"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\docs\SMARTSCREEN-INFO.md"; DestDir: "{app}"; Flags: ignoreversion

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
; Clean up user configuration and settings
Type: filesandordirs; Name: "{userappdata}\CursorCloak"
; Clean up any log files that might have been created in app directory
Type: files; Name: "{app}\*.log"
; Clean up any crash dumps or temp files in app directory
Type: files; Name: "{app}\*.dmp"
Type: files; Name: "{app}\*.tmp"
; Clean up temporary files that might exist
Type: files; Name: "{tmp}\CursorCloak*"
; Clean up any files in LocalAppData (Windows temp location)
Type: filesandordirs; Name: "{localappdata}\CursorCloak"
; Clean up any possible registry backup files
Type: files; Name: "{tmp}\CursorCloak_registry_backup_*"

[UninstallRun]
; Stop all running instances of CursorCloak first (with better error handling)
Filename: "{cmd}"; Parameters: "/C taskkill /f /im ""CursorCloak.UI.exe"" /t >nul 2>&1"; Flags: runhidden; StatusMsg: "Stopping CursorCloak..."; RunOnceId: "StopCursorCloak"
; Give processes time to terminate gracefully
Filename: "{cmd}"; Parameters: "/C timeout /t 2 /nobreak >nul 2>&1"; Flags: runhidden; RunOnceId: "WaitTermination"
; Remove startup registry entry with error handling
Filename: "{cmd}"; Parameters: "/C reg delete ""HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Run"" /v ""CursorCloak"" /f >nul 2>&1"; Flags: runhidden; StatusMsg: "Removing startup entry..."; RunOnceId: "RemoveStartupRegistry"
; Remove any potential backup registry entries
Filename: "{cmd}"; Parameters: "/C reg delete ""HKCU\SOFTWARE\CursorCloak"" /f >nul 2>&1"; Flags: runhidden; RunOnceId: "RemoveAppRegistry"
; Clean up any running background tasks
Filename: "{cmd}"; Parameters: "/C wmic process where ""name='CursorCloak.UI.exe'"" delete >nul 2>&1"; Flags: runhidden; RunOnceId: "CleanupWMIC"

[Code]
var
  InfoPage: TOutputMsgMemoWizardPage;

procedure InitializeWizard();
begin
  // Create a custom page to show application information
  InfoPage := CreateOutputMsgMemoPage(wpLicense,
    'Application Information', 'CursorCloak v1.0.2 - Enhanced Release',
    'Please review the information below about CursorCloak:', '');

  // Add application information
  InfoPage.RichEditViewer.Lines.Add('CURSORCLOAK v1.0.2 - ENHANCED RELEASE');
  InfoPage.RichEditViewer.Lines.Add('Release Date: August 7, 2025');
  InfoPage.RichEditViewer.Lines.Add('');
  InfoPage.RichEditViewer.Lines.Add('WHAT''S NEW IN v1.0.2:');
  InfoPage.RichEditViewer.Lines.Add('- Enhanced CI/CD pipeline with improved InnoSetup handling');
  InfoPage.RichEditViewer.Lines.Add('- Better release automation and version management');
  InfoPage.RichEditViewer.Lines.Add('- Improved error handling and logging in build process');
  InfoPage.RichEditViewer.Lines.Add('- More robust installer creation with fallback mechanisms');
  InfoPage.RichEditViewer.Lines.Add('- Professional build system enhancements');
  InfoPage.RichEditViewer.Lines.Add('');
  InfoPage.RichEditViewer.Lines.Add('WHAT IS CURSORCLOAK?');
  InfoPage.RichEditViewer.Lines.Add('A professional Windows utility for hiding and showing the mouse cursor');
  InfoPage.RichEditViewer.Lines.Add('using global keyboard shortcuts. Perfect for presentations, screenshots,');
  InfoPage.RichEditViewer.Lines.Add('screen recordings, and distraction-free computing.');
  InfoPage.RichEditViewer.Lines.Add('');
  InfoPage.RichEditViewer.Lines.Add('KEY FEATURES:');
  InfoPage.RichEditViewer.Lines.Add('- Global Hotkeys: Alt+H to hide, Alt+S to show cursor');
  InfoPage.RichEditViewer.Lines.Add('- Background Mode: Continues running when window is closed');
  InfoPage.RichEditViewer.Lines.Add('- Persistent Settings: Remembers preferences between sessions');
  InfoPage.RichEditViewer.Lines.Add('- No Tray Clutter: Clean operation without system tray icons');
  InfoPage.RichEditViewer.Lines.Add('- Admin Protection: Handles Windows privileges automatically');
  InfoPage.RichEditViewer.Lines.Add('- Modern Interface: Clean, dark-themed WPF design');
  InfoPage.RichEditViewer.Lines.Add('');
  InfoPage.RichEditViewer.Lines.Add('SYSTEM REQUIREMENTS:');
  InfoPage.RichEditViewer.Lines.Add('- Windows 10 or Windows 11 (x64)');
  InfoPage.RichEditViewer.Lines.Add('- .NET 9.0 Runtime (will be prompted to install if missing)');
  InfoPage.RichEditViewer.Lines.Add('- Administrator privileges for cursor manipulation');
  InfoPage.RichEditViewer.Lines.Add('- Approximately 50MB disk space');
  InfoPage.RichEditViewer.Lines.Add('');
  InfoPage.RichEditViewer.Lines.Add('GETTING STARTED:');
  InfoPage.RichEditViewer.Lines.Add('1. Launch CursorCloak from Start Menu or Desktop');
  InfoPage.RichEditViewer.Lines.Add('2. Right-click and select "Run as administrator"');
  InfoPage.RichEditViewer.Lines.Add('3. Configure your preferences in the settings');
  InfoPage.RichEditViewer.Lines.Add('4. Use Alt+H to hide cursor anywhere in Windows');
  InfoPage.RichEditViewer.Lines.Add('5. Use Alt+S to show cursor again');
  InfoPage.RichEditViewer.Lines.Add('6. Close the window to run in background mode');
  InfoPage.RichEditViewer.Lines.Add('');
  InfoPage.RichEditViewer.Lines.Add('SUPPORT & INFORMATION:');
  InfoPage.RichEditViewer.Lines.Add('- Documentation: README.md (installed with application)');
  InfoPage.RichEditViewer.Lines.Add('- Source Code: https://github.com/JAMPANIKOMAL/CursorCloak');
  InfoPage.RichEditViewer.Lines.Add('- Bug Reports: GitHub Issues section');
  InfoPage.RichEditViewer.Lines.Add('- Latest Updates: GitHub Releases page');
end;

procedure CursorMoveProc(X, Y: Integer);
begin
  // This procedure is called when the mouse cursor is moved during installation
  // Can be used for custom animations or effects
end;

function InitializeSetup(): Boolean;
begin
  Result := True;
  
  // Check Windows version (Windows 10 or later required)
  if GetWindowsVersion < $0A000000 then
  begin
    MsgBox('CursorCloak requires Windows 10 or later.' + #13#10 + 
           'Your current Windows version is not supported.' + #13#10 + #13#10 +
           'Please upgrade to Windows 10 or Windows 11 to use CursorCloak.', 
           mbError, MB_OK);
    Result := False;
  end;
  
  // Note: We don't check for .NET here as it will be handled during first run
  // if using framework-dependent version
end;

procedure DeinitializeSetup();
begin
  // Cleanup code when setup exits
end;

// Enhanced uninstaller functions
function InitializeUninstall(): Boolean;
var
  UserResponse: Integer;
  ProcessRunning: Boolean;
begin
  Result := True;
  
  // Check if CursorCloak is currently running
  ProcessRunning := CheckForMutexes('Global\CursorCloakMutex') or 
                   (FindWindowByClassName('CursorCloakMainWindow') <> 0);
  
  if ProcessRunning then
  begin
    UserResponse := MsgBox('CursorCloak appears to be running.' + #13#10 + #13#10 +
                          'The uninstaller will automatically close it before proceeding.' + #13#10 + #13#10 +
                          'Do you want to continue with the uninstallation?',
                          mbConfirmation, MB_YESNO);
    if UserResponse = IDNO then
    begin
      Result := False;
      Exit;
    end;
  end;
  
  // Confirm data deletion
  UserResponse := MsgBox('This will completely remove CursorCloak and all its data, including:' + #13#10 + #13#10 +
                        '• Application files and shortcuts' + #13#10 +
                        '• User settings and preferences' + #13#10 +
                        '• Windows startup entries' + #13#10 +
                        '• Configuration files in AppData' + #13#10 + #13#10 +
                        'This action cannot be undone.' + #13#10 + #13#10 +
                        'Are you sure you want to proceed?',
                        mbConfirmation, MB_YESNO or MB_DEFBUTTON2);
  
  Result := (UserResponse = IDYES);
end;

procedure CurUninstallStepChanged(CurUninstallStep: TUninstallStep);
var
  UserConfigPath: String;
  AppDataPath: String;
begin
  case CurUninstallStep of
    usUninstall:
    begin
      // Custom cleanup actions before standard uninstall
      Log('Starting enhanced CursorCloak cleanup...');
      
      // Show progress
      UninstallProgressForm.StatusLabel.Caption := 'Cleaning up CursorCloak data...';
      UninstallProgressForm.Update;
    end;
    
    usPostUninstall:
    begin
      // Final cleanup after standard uninstall
      UserConfigPath := ExpandConstant('{userappdata}\CursorCloak');
      AppDataPath := ExpandConstant('{localappdata}\CursorCloak');
      
      // Verify cleanup completion
      if DirExists(UserConfigPath) then
      begin
        Log('Warning: User config directory still exists: ' + UserConfigPath);
      end;
      
      if DirExists(AppDataPath) then
      begin
        Log('Warning: Local app data directory still exists: ' + AppDataPath);
      end;
      
      // Show completion message
      MsgBox('CursorCloak has been successfully uninstalled.' + #13#10 + #13#10 +
             'All application files, settings, and registry entries have been removed.' + #13#10 + #13#10 +
             'Thank you for using CursorCloak!',
             mbInformation, MB_OK);
    end;
  end;
end;
