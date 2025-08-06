# CursorCloak Version Information

This file tracks version information and release notes for CursorCloak.

## Current Version: 1.0.0 - Professional Release

### Release Date: August 7, 2025

### What's New in v1.0.0:
- ✅ **Professional Cursor Management**: Hide and show mouse cursor with global hotkeys
- ✅ **Background Mode**: Continues running when window is closed
- ✅ **Global Hotkeys**: Alt+H to hide, Alt+S to show cursor anywhere in Windows
- ✅ **Persistent Settings**: Automatically saves and restores user preferences
- ✅ **Administrator Handling**: Smart privilege detection and management
- ✅ **Modern Interface**: Clean, dark-themed WPF design
- ✅ **Multiple Deployment Options**: Framework-dependent and self-contained builds

### Core Features:
- ✅ **Global Hotkey Support**: Works system-wide in any application
- ✅ **Background Operation**: No system tray clutter, runs silently
- ✅ **Smart Privilege Management**: Handles administrator requirements automatically
- ✅ **Persistent Configuration**: Settings survive restarts and updates
- ✅ **Windows Integration**: Seamless Windows 10/11 compatibility
- ✅ **Professional Interface**: Modern WPF with intuitive controls
- ✅ **Memory Efficient**: Minimal system resource usage
- ✅ **Error Handling**: Comprehensive error management and user feedback
- ✅ **Professional Installer**: Enhanced InnoSetup installer with SmartScreen guidance
- ✅ **Multiple Formats**: Both framework-dependent and self-contained versions

### System Requirements:
- Windows 10 or later (Windows 11 recommended)
- Administrator privileges required for cursor manipulation
- .NET 9.0 Runtime (framework-dependent version) or included (self-contained)
- Approximately 50MB disk space (framework-dependent) or 60MB (self-contained)

### Installation Options:
1. **Windows Installer (Recommended)**: Professional installer with shortcuts and uninstaller
2. **Portable Framework-Dependent**: Requires .NET 9.0 runtime (~5MB download)
3. **Portable Self-Contained**: Includes .NET runtime (~60MB download)

### Available Downloads (4 Files):
1. **CursorCloak_Setup_v1.0.0.exe** - Windows installer (framework-dependent)
2. **CursorCloak-v1.0.0-win-x64.zip** - Portable framework-dependent version
3. **CursorCloak_Setup_v1.0.0_SelfContained.exe** - Windows installer (self-contained)
4. **CursorCloak-v1.0.0-win-x64-selfcontained.zip** - Portable self-contained version

### Hotkeys:
- **Alt + H**: Hide cursor
- **Alt + S**: Show cursor

### Known Issues:
- Some full-screen applications may override cursor visibility settings
- Requires administrator privileges for system-wide cursor manipulation
- First run may require .NET 9.0 runtime installation (framework-dependent version only)

### SmartScreen Notice:
Windows SmartScreen may show warnings because this application is not commercially code-signed. This is normal for open-source software. Click "More info" then "Run anyway" if prompted. See SMARTSCREEN-INFO.md for detailed information.

### Future Roadmap:
- [ ] Custom hotkey configuration
- [ ] System tray integration
- [ ] Multiple cursor hiding modes
- [ ] Game-specific profiles
- [ ] Multi-monitor support enhancements

### Technical Details:
- Built with .NET 9.0 and WPF
- Uses Windows API for cursor manipulation
- Persistent JSON-based configuration
- Thread-safe operations
- Comprehensive resource management

---

For detailed installation and usage instructions, see [README.md](README.md).
For troubleshooting, see [TROUBLESHOOTING.md](TROUBLESHOOTING.md).
