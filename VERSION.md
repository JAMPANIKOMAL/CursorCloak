# CursorCloak Version Information

This file tracks version information and release notes for CursorCloak.

## Current Version: 1.0.0 - Background Mode Release

### Release Date: August 6, 2025

### What's New in v1.0.0:
- ✅ **Background Running**: App continues running when closed, keeping hotkeys active
- ✅ **No Tray Icon**: Clean background operation without system tray clutter
- ✅ **Welcome Dialog**: First-run experience for user configuration
- ✅ **User Preferences**: Customizable name, organization, and settings
- ✅ Hide/show cursor with Alt+H/Alt+S hotkeys
- ✅ Persistent settings storage with user configuration
- ✅ Windows startup integration with background mode
- ✅ Modern WPF dark-themed interface
- ✅ Administrator privilege handling
- ✅ Comprehensive error handling and user feedback
- ✅ Memory leak prevention
- ✅ Professional installer with InnoSetup
- ✅ Enhanced build system with multiple deployment options
- ✅ Automated GitHub Actions CI/CD

### System Requirements:
- Windows 10 or later (Windows 11 recommended)
- .NET 9.0 Runtime (or use self-contained build)
- Administrator privileges required

### Installation Options:
1. **Installer (Recommended)**: Full installer with shortcuts and uninstaller
2. **Portable Framework-Dependent**: Requires .NET 9.0 runtime
3. **Portable Self-Contained**: Includes .NET runtime (~100MB)

### Hotkeys:
- **Alt + H**: Hide cursor
- **Alt + S**: Show cursor

### Known Issues:
- Some applications may override cursor visibility
- Cursor state may not persist in certain full-screen games
- Requires administrator privileges for system cursor manipulation

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
