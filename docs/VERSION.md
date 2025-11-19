# CursorCloak Version Information

This document tracks all version releases and their notes for CursorCloak.

## Versioning Rule
**Whenever a new version is released, move the previous 'Current Version' section to 'Previous Versions' below, and add the new version at the top as 'Current Version'. Always keep the latest version at the top.**

---

## Current Version: 1.0.2 (Enhanced Patch Release)
**Release Date:** August 11, 2025

### What's New in v1.0.2

## Current Version: v2.0.0 (Professional Release)
**Release Date:** November 19, 2025

### What's New in v2.0.0
- 🎨 **Major UI Overhaul**: Complete redesign with a minimalist, professional aesthetic.
- 🏗️ **Codebase Restructuring**: Professional architecture with separated Services and Models.
- 📝 **Documentation**: Comprehensive XML documentation and professional README/CHANGELOG.
- 🛡️ **Security**: Added Security Policy and Code of Conduct.
- 🐛 **Bug Fixes**: Fixed single-instance enforcement and startup reliability.
- ⚙️ **Release Pipeline**: Automated GitHub Actions workflow for reliable releases.

## Previous Versions

### v1.1.0
**Release Date:** August 19, 2025

#### What's New in v1.1.0
- 🖱️ **Auto Hide Cursor**: Cursor automatically hides after user inactivity (timeout configurable in UI)
- 🛠️ **Bugfix:** "Start with Windows" now works reliably on all Windows 10/11 systems (improved registry handling, robust path detection, and admin support)
- 🟢 **Auto Hide Toggle**: New toggle in the UI to enable/disable auto-hide feature
- ⏱️ **Custom Timeout**: Set the number of seconds before the cursor auto-hides
- ✅ All previous enhancements and fixes

## Previous Versions

### v1.0.2
- 🔧 **Enhanced CI/CD Pipeline** - Improved build automation and reliability
- 💻 **Better InnoSetup Handling** - More robust installer creation process
- 🛡️ **Improved Error Handling** - Enhanced logging and fallback mechanisms
- ✨ **Version Management** - Centralized version control across all components
- 📦 **Professional Release Process** - Automated GitHub release creation
- 🎯 **System Tray Improvements** - Better tray icon resource management
- 📋 **Registry Management** - Enhanced startup settings persistence
- ⚡ **Memory Optimization** - Better resource cleanup and leak prevention

### v1.0.1
**Release Date:** August 7, 2025
... (older notes)
- CursorCloak_Setup_v1.0.2.exe - Framework-dependent installer (requires .NET 9.0) - ~2MB
- CursorCloak_Setup_v1.0.2_SelfContained.exe - Self-contained installer (no .NET required) - ~66MB
- CursorCloak-v1.0.2-win-x64.zip - Framework-dependent portable (requires .NET 9.0) - ~0.3MB
- CursorCloak-v1.0.2-win-x64-selfcontained.zip - Self-contained portable (no .NET required) - ~66MB

#### Requirements
- Windows 10/11 (x64)
- Administrator privileges
- .NET 9.0 Runtime (for framework-dependent versions only)

#### Usage
- **Alt + H**: Hide cursor anywhere in Windows
- **Alt + S**: Show cursor again
- Close window to run in background mode

#### SmartScreen Notice
Windows may show a security warning. Click "More info" then "Run anyway". This is normal for unsigned applications. Always run as administrator for proper functionality.

---

## Previous Versions

### Version 1.0.1 (Enhanced Patch Release)
**Release Date:** August 7, 2025

#### What's New in v1.0.1
- 🔧 **Fixed "Start with Windows" functionality** - Now properly saves and applies settings when toggled
- 🗑️ **Professional Uninstaller** - Complete cleanup including registry entries and user data
- ✅ **Enhanced Registry Management** - Improved startup entry handling with validation
- 🛡️ **Better Error Handling** - More robust registry access and file operations
- 📋 **Comprehensive Cleanup** - Uninstaller removes all traces of the application

#### Bug Fixes
- Fixed startup checkbox not persisting settings properly
- Added proper registry cleanup on uninstall
- Enhanced uninstaller to remove user configuration data
- Improved error handling for registry operations
- Added startup verification functionality with user feedback

#### What the Enhanced Uninstaller Removes
- All application files and folders
- Windows startup registry entries
- User settings and configuration files
- Desktop and Start Menu shortcuts
- Stops any running instances
- Complete registry cleanup

#### Download Options
- **Framework-dependent installer** (requires .NET 9.0) - ~2MB
- **Self-contained installer** (no .NET required) - ~66MB
- **Framework-dependent portable** (requires .NET 9.0) - ~0.3MB
- **Self-contained portable** (no .NET required) - ~66MB

### Version 1.0.0 (Professional Edition)
**Release Date:** August 6, 2025

#### What's New in v1.0.0
- 🎯 **Professional Cursor Management** - Hide and show mouse cursor with global hotkeys
- 🔄 **Background Mode** - Continues running when window is closed
- ⌨️ **Global Hotkeys** - Alt+H to hide, Alt+S to show cursor anywhere in Windows
- 💾 **Persistent Settings** - Automatically saves and restores user preferences
- 🛡️ **Smart Administrator Handling** - Automatic privilege management with user guidance
- 🎨 **Modern Interface** - Professional WPF design with dark theme
- 📦 **Multiple Deployment Options** - Framework-dependent and self-contained builds

#### Core Features
- **Global Hotkey Support** - Works system-wide in any application
- **No Tray Clutter** - Clean operation without system tray icons
- **Smart Privilege Management** - Handles administrator requirements automatically
- **Persistent Configuration** - Settings survive restarts and updates
- **Windows Integration** - Seamless Windows 10/11 compatibility
- **Professional Interface** - Modern WPF with intuitive controls
- **Memory Efficient** - Minimal system resource usage
- **Comprehensive Error Handling** - User-friendly error management and feedback
- **Professional Installer** - Enhanced InnoSetup installer with SmartScreen guidance
- **Multiple Formats** - Both framework-dependent and self-contained versions

#### System Requirements
- Windows 10 or later (Windows 11 recommended)
- Administrator privileges required for cursor manipulation
- .NET 9.0 Runtime (framework-dependent version) or included (self-contained)
- Approximately 50MB disk space (framework-dependent) or 60MB (self-contained)

#### Download Options (4 Formats Available)
1. **Framework-dependent installer** (1.8MB) - requires .NET 9.0 Runtime
2. **Self-contained installer** (54.9MB) - no .NET required, works on any Windows 10/11 PC
3. **Framework-dependent portable** (0.1MB) - requires .NET 9.0 Runtime
4. **Self-contained portable** (54.6MB) - no .NET required

#### Security Notice (SmartScreen)
Windows may show a security warning because this open-source app isn't commercially code-signed. This is completely normal and safe.

**If you see a SmartScreen warning:**
1. Click **"More info"**
2. Click **"Run anyway"**

This software is 100% open source, requires no network access, and collects no data.

#### Quick Start Guide
1. Download your preferred version below
2. Run installer as administrator OR extract portable version
3. Launch CursorCloak as administrator
4. Use **Alt+H** to hide cursor, **Alt+S** to show cursor
5. Close window to run in background mode

#### Future Roadmap
- [ ] Custom hotkey configuration
- [ ] System tray integration
- [ ] Multiple cursor hiding modes
- [ ] Game-specific profiles
- [ ] Multi-monitor support enhancements

#### Technical Details
- Built with .NET 9.0 and WPF
- Uses Windows API for cursor manipulation
- Persistent JSON-based configuration
- Thread-safe operations
- Comprehensive resource management

---

For installation and usage instructions, see [README.md](../README.md).
For troubleshooting, see [TROUBLESHOOTING.md](TROUBLESHOOTING.md).
