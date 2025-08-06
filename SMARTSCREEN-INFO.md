# Windows SmartScreen Information

## What is Windows SmartScreen?

Windows SmartScreen is a security feature that helps protect your PC by checking files and apps downloaded from the internet. When you download and run CursorCloak, you might see a SmartScreen warning because:

1. **The application is not digitally signed** - Digital code signing certificates cost hundreds of dollars annually, which is not feasible for free open-source projects
2. **Low download frequency** - New applications need to build reputation over time
3. **Not from Microsoft Store** - Sideloaded applications trigger additional scrutiny

## How to Install CursorCloak Safely

### Method 1: Installer (Recommended)
1. Download `CursorCloak_Setup_v1.0.0.exe` from [GitHub Releases](https://github.com/JAMPANIKOMAL/CursorCloak/releases)
2. Right-click the installer → **Properties** → **Digital Signatures** tab (to verify it's from the correct source)
3. Run the installer as administrator
4. If SmartScreen appears:
   - Click **"More info"**
   - Click **"Run anyway"**
5. Follow the installation wizard

### Method 2: Portable Version (Framework-Dependent)
1. Download `CursorCloak-v1.0.0-win-x64.zip` from [GitHub Releases](https://github.com/JAMPANIKOMAL/CursorCloak/releases)
2. Extract to any folder
3. Requires .NET 9.0 Runtime: `winget install Microsoft.DotNet.Runtime.9`
4. Right-click `CursorCloak.UI.exe` → **"Run as administrator"**

### Method 3: Portable Self-Contained Version
1. Download `CursorCloak-v1.0.0-win-x64-selfcontained.zip` from [GitHub Releases](https://github.com/JAMPANIKOMAL/CursorCloak/releases)
2. Extract to any folder (no .NET required)
3. Right-click `CursorCloak.UI.exe` → **"Run as administrator"**

## Why CursorCloak is Safe

✅ **Open Source**: Full source code available on [GitHub](https://github.com/JAMPANIKOMAL/CursorCloak)
✅ **No Network Access**: Application works completely offline
✅ **No Data Collection**: No telemetry, analytics, or personal data collection
✅ **Minimal Permissions**: Only requires administrator access for cursor manipulation
✅ **Transparent**: All build scripts and processes are publicly available

## Alternative Solutions

### If you prefer not to bypass SmartScreen:

1. **Compile from Source**: Build the application yourself using Visual Studio 2022
2. **Wait for Reputation**: As more users download and use CursorCloak, SmartScreen warnings will decrease
3. **Use Windows Subsystem for Linux**: Run a Linux cursor utility instead

## Building from Source (Advanced)

To build a signed version yourself:

1. Clone the repository: `git clone https://github.com/JAMPANIKOMAL/CursorCloak.git`
2. Open in Visual Studio 2022
3. Build in Release configuration
4. Optionally sign with your own certificate

## Reporting Issues

If you experience any security concerns or issues:
- [Report on GitHub Issues](https://github.com/JAMPANIKOMAL/CursorCloak/issues)
- Check existing discussions in the repository

---

**Remember**: SmartScreen warnings are common for legitimate open-source software. Always download from the official GitHub repository to ensure authenticity.
