# CursorCloak Version Information

This file tracks version information and release notes for CursorCloak.

## Current Version: 1.2.0 - Self-Contained Release

### Release Date: August 7, 2025

### What's New in v1.2.0:
- ✅ **Self-Contained Application**: No .NET runtime required - runs on any Windows PC!
- ✅ **Custom App Icon**: Professional mouse cursor icon with hide indicator
- ✅ **Single File Deployment**: Everything bundled into one executable
- ✅ **Optimized Size**: Streamlined package without unnecessary files
- ✅ **Clean Project Structure**: Removed redundant documentation and old releases

### Core Features (Enhanced):
- ✅ **No .NET Dependency**: Self-contained - works on Windows 10/11 without any prerequisites
- ✅ **Professional Icon**: Custom-designed mouse cursor icon
- ✅ **Single EXE**: All dependencies included in one file
- ✅ **Background Running**: App continues running when closed, keeping hotkeys active
- ✅ **No Tray Icon**: Clean background operation without system tray clutter
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
