# CursorCloak

A Windows utility that allows you to hide and show the system mouse cursor using hotkeys.

## Features

- **Cursor Hiding**: Hide/show system cursor with Alt+H/Alt+S hotkeys
- **Persistent Settings**: Remembers your preferences between sessions
- **Startup Integration**: Option to start with Windows
- **Modern UI**: Clean, dark-themed WPF interface

## Quick Start

1. Build the solution in Release mode:
   ```
   dotnet build --configuration Release
   ```

2. Run the UI application:
   ```
   CursorCloak.UI\bin\Release\net9.0-windows\CursorCloak.UI.exe
   ```

## Hotkeys

- **Alt + H**: Hide cursor
- **Alt + S**: Show cursor

## Architecture

The solution consists of two projects:

### CursorCloak.UI
- WPF application with the user interface
- Settings management and persistence
- Windows startup integration
- Global hotkey handling

### CursorCloak.Engine
- Console application with core cursor manipulation logic
- Windows API integration for cursor management
- Low-level system interactions

## Recent Fixes (Error 0xc000041d Resolution)

The application was experiencing fatal crashes with exception code `0xc000041d`. The following fixes were implemented:

### 1. Global Exception Handling
- Added application-wide exception handlers in `App.xaml.cs`
- Implemented graceful error reporting and recovery
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
