# CursorCloak

A professional Windows utility that allows you to hide and show the system mouse cursor using hotkeys. Features **background running mode** and **seamless Windows integration**. **Requires administrator privileges.**

*🤖 Enhanced with AI assistance using GitHub Copilot for optimal code quality and performance.*

## 📥 Download

### Latest Release v1.0.0 - Professional Edition
[![Download Latest](https://img.shields.io/badge/Download-Latest%20Release-blue?style=for-the-badge)](https://github.com/JAMPANIKOMAL/CursorCloak/releases/latest)

**Choose Your Installation Method (4 Options Available):**

**🔧 Windows Installers:**
1. [**CursorCloak_Setup_v1.0.0.exe**](https://github.com/JAMPANIKOMAL/CursorCloak/releases/download/v1.0.0/CursorCloak_Setup_v1.0.0.exe) - Framework-dependent installer (requires .NET 9.0)
2. [**CursorCloak_Setup_v1.0.0_SelfContained.exe**](https://github.com/JAMPANIKOMAL/CursorCloak/releases/download/v1.0.0/CursorCloak_Setup_v1.0.0_SelfContained.exe) - Self-contained installer (no .NET required)

**📦 Portable Versions:**
3. [**CursorCloak-v1.0.0-win-x64.zip**](https://github.com/JAMPANIKOMAL/CursorCloak/releases/download/v1.0.0/CursorCloak-v1.0.0-win-x64.zip) - Framework-dependent portable (~5MB, requires .NET 9.0)
4. [**CursorCloak-v1.0.0-win-x64-selfcontained.zip**](https://github.com/JAMPANIKOMAL/CursorCloak/releases/download/v1.0.0/CursorCloak-v1.0.0-win-x64-selfcontained.zip) - Self-contained portable (~60MB, no .NET required)

**🛡️ SmartScreen Notice:** Windows may show a security warning because this app isn't commercially signed. Click "More info" → "Run anyway". See [SMARTSCREEN-INFO.md](SMARTSCREEN-INFO.md) for details.

### Alternative Downloads
- [View All Releases](https://github.com/JAMPANIKOMAL/CursorCloak/releases)
- [Source Code (ZIP)](https://github.com/JAMPANIKOMAL/CursorCloak/archive/refs/heads/main.zip)

## ✨ Features

### 🎮 Core Features
- **⌨️ Global Hotkeys**: Hide/show system cursor with Alt+H/Alt+S hotkeys  
- **� Background Mode**: Continues running when window is closed
- **💾 Persistent Settings**: Remembers all preferences between sessions
- **🎯 No Tray Clutter**: Clean operation without system tray icons
- **🎨 Modern UI**: Clean, dark-themed WPF interface with professional design
- **🔐 Administrator Protection**: Automatic privilege checking and comprehensive error handling
- **⚡ Lightweight**: Minimal system resource usage with efficient background operation

## 🚀 Quick Start

### Option 1: Windows Installer (Recommended)
1. Download the appropriate installer for your needs:
   - **Framework-dependent**: `CursorCloak_Setup_v1.0.0.exe` (smaller, requires .NET 9.0)
   - **Self-contained**: `CursorCloak_Setup_v1.0.0_SelfContained.exe` (larger, no .NET required)
2. Run the installer as administrator
3. Follow the setup wizard (choose installation directory, shortcuts)
4. Launch automatically after installation
5. **Complete installation with shortcuts and uninstaller!**

### Option 2: Portable ZIP
1. Download the appropriate ZIP for your needs:
   - **Framework-dependent**: `CursorCloak-v1.0.0-win-x64.zip` (~5MB, requires .NET 9.0)
   - **Self-contained**: `CursorCloak-v1.0.0-win-x64-selfcontained.zip` (~60MB, no .NET required)
2. Extract the ZIP file to your preferred location
3. Right-click `CursorCloak.UI.exe` and select "Run as administrator"
4. Start using immediately - Alt+H to hide, Alt+S to show cursor!

**🛡️ SmartScreen Warning?** This is normal for unsigned open-source apps. Click "More info" → "Run anyway"

### Option 3: Building from Source
1. Clone this repository
2. Build with enhanced build script:
   ```powershell
   .\build.ps1 -Clean -Publish
   ```
3. Executable will be in build output directory
4. Run as administrator and enjoy!

## 📋 Requirements

- **Windows 10 or later** (Windows 11 recommended)
- **.NET 9.0 Runtime** (framework-dependent versions only)
- **Administrator privileges** (required for global hotkeys and cursor manipulation)

## ⌨️ Hotkeys

- **Alt + H**: Hide cursor
- **Alt + S**: Show cursor

## 🏗️ Architecture

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

## 🛠️ Building from Source

### Prerequisites
- .NET 9.0 SDK
- Windows 10/11
- Administrator privileges
- InnoSetup (optional, for creating installers)

### Build Commands
```bash
# Using the enhanced build script (recommended)
.\build.ps1                                      # Basic build
.\build.ps1 -Clean                              # Clean and build
.\build.ps1 -Clean -Publish -SelfContained      # Self-contained deployment
.\build.ps1 -Clean -Publish -CreateInstaller    # Build with installer
.\build.ps1 -Clean -Publish -Test               # Build with tests

# Manual build commands
dotnet clean --configuration Release
dotnet restore
dotnet build --configuration Release

# Create self-contained deployment
dotnet publish CursorCloak.UI\CursorCloak.UI.csproj --configuration Release --runtime win-x64 --self-contained true --output .\publish\ui\

# Test the built application
.\test.ps1

# Create installer (requires InnoSetup)
# Compile setup.iss with InnoSetup Compiler
```

## ⚙️ Installation and Setup

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

## 🔧 Configuration

Settings are stored in:
```
%APPDATA%\CursorCloak\settings.json
```

Configuration options:
- **IsHidingEnabled**: Whether cursor hiding is active
- **StartWithWindows**: Launch automatically on Windows startup

## 🐛 Troubleshooting

### Common Issues

**Error 0xc000041d (Fatal Exception)**
- Ensure running as administrator
- Verify .NET 9.0 runtime is installed
- Check Windows Event Viewer for detailed errors

**Hotkeys Not Working**
- Check for conflicts with other applications
- Ensure application has proper focus
- Verify administrator privileges

**Cursor Won't Hide/Show**
- Some applications override cursor visibility
- Try moving mouse to refresh state
- Restart application if cursor gets stuck

For more help, check our [Issues page](https://github.com/JAMPANIKOMAL/CursorCloak/issues) or create a new issue.

## 👨‍💻 Development

### Project Structure
```
CursorCloak/
├── .github/                 # GitHub workflows and templates
│   ├── workflows/          # Automated CI/CD pipelines
│   └── ISSUE_TEMPLATE/     # Bug report and feature request templates
├── CursorCloak.UI/          # WPF application
├── CursorCloak.Engine/      # Console engine
├── build.ps1               # Enhanced build script with multiple options
├── test.ps1                # Application testing script
├── setup.iss               # InnoSetup installer script
├── LICENSE                 # MIT License
├── VERSION.md              # Version tracking and release notes
├── CONTRIBUTING.md         # Contribution guidelines
└── README.md               # This file
```

### Code Quality
- Comprehensive error handling throughout
- Resource management for GDI objects
- Thread-safe operations
- Memory leak prevention
- Administrator privilege checking

### Contributing
1. Fork the repository
2. Create a feature branch
3. Ensure builds pass without warnings
4. Test on clean Windows installation
5. Verify administrator privilege handling
6. Test installer creation and deployment
7. Update documentation for any changes
8. Submit a pull request
## 🔐 Security Considerations

- **Administrator Privileges**: Required for system cursor manipulation
- **Code Signing**: Consider signing executables for production distribution
- **Antivirus**: May flag cursor manipulation as suspicious behavior
- **System Integration**: Modifies global cursor state

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 📊 Version History

### v1.0.0 - Initial Release
- ✅ Fixed fatal crash issues (error 0xc000041d)
- ✅ Added administrator privilege checking  
- ✅ Improved resource management and memory leak prevention
- ✅ Enhanced build and deployment process
- ✅ Comprehensive error handling throughout
- ✅ Thread-safe operations and proper cleanup

### Recent Improvements
- ✅ Enhanced dispatcher unhandled exception handling
- ✅ Fixed memory leaks in cursor handling
- ✅ Added proper cleanup for GDI objects and icons
- ✅ Implemented thread-safe initialization with locks
- ✅ Added comprehensive error checking for Windows API calls
- ✅ Fixed nullable reference warnings and null checks
- ✅ Protected against race conditions during startup

## 🤝 Support

- **Bug Reports**: [Create an issue](https://github.com/JAMPANIKOMAL/CursorCloak/issues/new?assignees=&labels=bug&template=bug_report.md)
- **Feature Requests**: [Request a feature](https://github.com/JAMPANIKOMAL/CursorCloak/issues/new?assignees=&labels=enhancement&template=feature_request.md)
- **Questions**: [Start a discussion](https://github.com/JAMPANIKOMAL/CursorCloak/discussions)

## 📈 Project Stats

![GitHub release (latest by date)](https://img.shields.io/github/v/release/JAMPANIKOMAL/CursorCloak?style=flat-square)
![GitHub downloads](https://img.shields.io/github/downloads/JAMPANIKOMAL/CursorCloak/total?style=flat-square)
![GitHub issues](https://img.shields.io/github/issues/JAMPANIKOMAL/CursorCloak?style=flat-square)
![GitHub license](https://img.shields.io/github/license/JAMPANIKOMAL/CursorCloak?style=flat-square)
![GitHub stars](https://img.shields.io/github/stars/JAMPANIKOMAL/CursorCloak?style=flat-square)
![GitHub forks](https://img.shields.io/github/forks/JAMPANIKOMAL/CursorCloak?style=flat-square)

### 🤖 AI-Enhanced Development
This project has been enhanced with **GitHub Copilot** assistance to ensure:
- Optimal code quality and performance
- Best practices implementation
- Comprehensive error handling
- Memory management and resource cleanup
- Modern C# and WPF patterns

---

*Made with ❤️ and 🤖 AI assistance for Windows users who need cursor control*
