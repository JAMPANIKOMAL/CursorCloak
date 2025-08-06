# CursorCloak

A Windows utility that allows you to hide and show the system mouse cursor using hotkeys. **Requires administrator privileges.**

## Features

- **Cursor Hiding**: Hide/show system cursor with Alt+H/Alt+S hotkeys
- **Persistent Settings**: Remembers your preferences between sessions
- **Startup Integration**: Option to start with Windows
- **Modern UI**: Clean, dark-themed WPF interface
- **Administrator Protection**: Automatic privilege checking and error handling

## Quick Start

### Option 1: Using Pre-built Installer
1. Download the installer from releases
2. Run `CursorCloak_Setup.exe` as administrator
3. Follow the installation wizard
4. Launch CursorCloak from Start Menu

### Option 2: Building from Source
1. Clone this repository
2. Run as administrator: `build.bat` or `build.ps1`
3. The executable will be in `CursorCloak.UI\bin\Release\net9.0-windows\`
4. Run `CursorCloak.UI.exe` as administrator

## Requirements

- **Windows 10 or later** (Windows 11 recommended)
- **.NET 9.0 Runtime** (or use self-contained build)
- **Administrator privileges** (required for cursor manipulation)

## Hotkeys

- **Alt + H**: Hide cursor
- **Alt + S**: Show cursor

## Architecture

### CursorCloak.UI (WPF Application)
- User interface and settings management
- Windows startup integration
- Global hotkey handling
- Persistent configuration storage

### CursorCloak.Engine (Console Application)
- Core cursor manipulation logic
- Windows API integration for cursor management
- Low-level system interactions
- Standalone hotkey testing

## Building and Deployment

### Prerequisites
- .NET 9.0 SDK
- Windows 10/11
- Administrator privileges
- InnoSetup (for creating installers)

### Build Commands
```bash
# Using build scripts (recommended)
build.bat          # Batch script
build.ps1          # PowerShell script

# Manual build
dotnet clean --configuration Release
dotnet restore
dotnet build --configuration Release

# Create self-contained deployment
dotnet publish CursorCloak.UI\CursorCloak.UI.csproj --configuration Release --runtime win-x64 --self-contained true --output .\Publish\

# Create installer (requires InnoSetup)
# Right-click setup.iss and select "Compile"
```

## Installation and Setup

### Using the Installer
1. Run `CursorCloak_Setup.exe` as administrator
2. Choose installation directory (default: `C:\Program Files\CursorCloak`)
3. Select optional shortcuts (desktop, start menu)
4. Complete installation
5. Launch automatically if selected

### Manual Installation
1. Copy built files to desired directory
2. Ensure all .dll files are in the same directory as the .exe
3. Run as administrator for first-time setup
4. Configure startup options through the UI

## Configuration

Settings are stored in:
```
%APPDATA%\CursorCloak\settings.json
```

Configuration options:
- **IsHidingEnabled**: Whether cursor hiding is active
- **StartWithWindows**: Launch automatically on Windows startup

## Troubleshooting

### Common Issues

**Error 0xc000041d (Fatal Exception)**
- Ensure running as administrator
- Verify .NET 9.0 runtime is installed
- Check Windows Event Viewer for detailed errors
- See `TROUBLESHOOTING.md` for comprehensive solutions

**Hotkeys Not Working**
- Check for conflicts with other applications
- Ensure application has proper focus
- Verify administrator privileges

**Cursor Won't Hide/Show**
- Some applications override cursor visibility
- Try moving mouse to refresh state
- Restart application if cursor gets stuck

For detailed troubleshooting, see `TROUBLESHOOTING.md`.

## Development

### Project Structure
```
CursorCloak/
├── CursorCloak.UI/          # WPF application
├── CursorCloak.Engine/      # Console engine
├── Assets/                  # Icons and resources
├── Installer/              # Generated installers
├── setup.iss              # InnoSetup script
├── build.bat              # Windows build script
├── build.ps1              # PowerShell build script
└── README.md              # This file
```

### Code Quality
- Comprehensive error handling throughout
- Resource management for GDI objects
- Thread-safe operations
- Memory leak prevention
- Administrator privilege checking

### Contributing
1. Ensure builds pass without warnings
2. Test on clean Windows installation
3. Verify administrator privilege handling
4. Test installer creation and deployment
5. Update documentation for any changes

## Security Considerations

- **Administrator Privileges**: Required for system cursor manipulation
- **Code Signing**: Consider signing executables for production
- **Antivirus**: May flag cursor manipulation as suspicious
- **System Integration**: Modifies global cursor state

## License

This project is provided as-is for educational and utility purposes.

## Version History

- **1.0.0**: Initial release with comprehensive error handling
  - Fixed fatal crash issues (error 0xc000041d)
  - Added administrator privilege checking
  - Improved resource management
  - Enhanced build and deployment process
- Added dispatcher unhandled exception handling

### 2. Resource Management
- Fixed memory leaks in cursor handling
- Added proper cleanup for GDI objects and icons
- Implemented thread-safe initialization with locks

### 3. Error Checking for Win32 API Calls
- Added comprehensive error checking for all Windows API calls
- Implemented proper error messages with Win32 error codes
- Added validation for critical operations

### 4. Null Reference Protection
- Fixed all nullable reference warnings
- Added null checks for registry operations
- Protected against invalid file paths and handles

### 5. Better Initialization
- Implemented proper initialization order for WPF components
- Added validation for window handle creation
- Protected against race conditions during startup

## Building and Deployment

### Prerequisites
- .NET 9.0 or later
- Windows 10/11 (for WPF support)

### Build Commands
```bash
# Clean build
dotnet clean

# Restore packages
dotnet restore

# Build in Release mode
dotnet build --configuration Release

# Publish self-contained
dotnet publish -c Release -r win-x64 --self-contained true
```

### Installation
The solution includes an InnoSetup installer script (`setup.iss`) for creating a proper Windows installer.

## Settings Storage

Settings are stored in JSON format at:
```
%APPDATA%\CursorCloak\settings.json
```

## Troubleshooting

### Application Won't Start
1. Ensure you have .NET 9.0 runtime installed
2. Try running as administrator
3. Check Windows Event Viewer for detailed error messages

### Hotkeys Not Working
1. Check if another application is using Alt+H or Alt+S
2. Try restarting the application
3. Ensure the application window was properly initialized

### Cursor Won't Hide/Show
1. Some applications may override cursor visibility
2. Try moving the mouse to refresh cursor state
3. Restart the application if cursor gets stuck

## Development

### Code Structure
- `MainWindow.xaml.cs`: Main UI and event handling
- `CursorEngine`: Low-level cursor manipulation
- `HotKeyManager`: Global hotkey registration
- `SettingsManager`: Configuration persistence
- `StartupManager`: Windows startup integration

### Error Handling Strategy
- Non-critical errors show user-friendly messages
- Critical errors are logged and reported
- Application attempts graceful recovery when possible
- Resource cleanup is guaranteed in finally blocks

## Contributing

When making changes:
1. Test both Debug and Release builds
2. Verify error handling paths
3. Check for memory leaks in cursor operations
4. Test hotkey registration/unregistration
5. Validate startup/shutdown sequences

## License

This project is provided as-is for educational and utility purposes.
