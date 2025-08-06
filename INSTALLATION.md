# 🚀 CursorCloak Installation Guide

## 📥 Download Options

### 🔥 **Option 1: Windows Installer (Recommended)**
1. Go to [CursorCloak Releases](https://github.com/JAMPANIKOMAL/CursorCloak/releases/tag/v1.0.0)
2. Download `CursorCloak_Setup_v1.0.0.exe` (1.85 MB)
3. Follow installation steps below

### 📦 **Option 2: ZIP Package (Advanced Users)**
1. Download `CursorCloak-v1.0.0-win-x64.zip` (97.6 KB)
2. Extract to desired location
3. Right-click `CursorCloak.UI.exe` → "Run as administrator"

---

## 🛡️ Windows SmartScreen Warning

**⚠️ IMPORTANT**: Windows may show a SmartScreen warning because this is a new application without a digital signature.

### If you see this message:
```
Windows protected your PC
Microsoft Defender SmartScreen prevented an unrecognized app from starting.
```

### How to bypass safely:
1. **Click "More info"** (small text link)
2. **Click "Run anyway"** button that appears
3. The installer will start normally

### Why this happens:
- New applications without expensive code signing certificates trigger this
- This is normal and safe - the app is clean and built from open source
- Once enough users install it, Windows will recognize it as safe

---

## 📋 Installation Steps

### Using the Installer:
1. **Run** `CursorCloak_Setup_v1.0.0.exe`
2. **Handle SmartScreen** (see above if prompted)
3. **Follow installer** - it will:
   - Install to `C:\Program Files\CursorCloak`
   - Create desktop shortcut (optional)
   - Add to Start Menu
4. **Launch** the application
5. **Complete welcome setup** with your preferences
6. **Test background mode** - close with X button, hotkeys still work!

### Using ZIP Package:
1. **Extract** to folder like `C:\CursorCloak`
2. **Right-click** `CursorCloak.UI.exe` → **"Run as administrator"**
3. **Complete welcome setup**
4. **Enjoy background mode**

---

## 🔧 System Requirements

- **Windows 10/11** (x64 recommended)
- **Administrator privileges** (required for global hotkeys)
- **.NET 9.0 Runtime** (included in installer)

---

## ⌨️ Usage

### Global Hotkeys (work even when app is hidden):
- **Alt + H**: Hide system cursor
- **Alt + S**: Show system cursor

### Background Mode:
- Close window with **X button** - app continues running
- Hotkeys remain active in background
- No system tray icon (clean background operation)

---

## 🔒 Security Notes

- **Open Source**: Full code available on GitHub
- **No Network Access**: App works completely offline
- **Minimal Permissions**: Only needs cursor control and hotkey registration
- **Clean Background**: No data collection or telemetry

---

## 🐛 Troubleshooting

### App won't start:
- Ensure you're running as administrator
- Check Windows Event Viewer for .NET errors
- Try compatibility mode (Windows 10)

### Hotkeys not working:
- Verify administrator privileges
- Check for conflicts with other apps
- Restart Windows if issues persist

### SmartScreen keeps blocking:
- This is expected for new apps
- Always click "More info" → "Run anyway"
- Consider using ZIP package instead

---

## 📞 Support

- **Issues**: [GitHub Issues](https://github.com/JAMPANIKOMAL/CursorCloak/issues)
- **Documentation**: [Project README](https://github.com/JAMPANIKOMAL/CursorCloak)
- **Source Code**: [GitHub Repository](https://github.com/JAMPANIKOMAL/CursorCloak)

---

**🎉 Enjoy your professional cursor control experience!**
