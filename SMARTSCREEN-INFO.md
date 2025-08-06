# Windows SmartScreen Information

## What is Windows SmartScreen?

Windows SmartScreen is a security feature that helps protect your PC by checking downloaded files and applications against a database of known malicious software.

## Why does CursorCloak trigger SmartScreen?

CursorCloak may trigger SmartScreen warnings because:

1. **Not Digitally Signed**: The executable is not digitally signed with an Extended Validation (EV) certificate
2. **New Application**: SmartScreen uses reputation-based protection - new applications without widespread usage may be flagged
3. **Open Source**: Most open-source applications don't have expensive code signing certificates

## How to safely install CursorCloak:

### Method 1: Through SmartScreen Warning
1. Run the installer (`CursorCloak_Setup_v1.2.0.exe`)
2. If SmartScreen appears, click **"More info"**
3. Click **"Run anyway"** button
4. Proceed with installation

### Method 2: Temporary Disable SmartScreen (Advanced Users)
1. Open Windows Security (Windows Defender)
2. Go to App & Browser Control
3. Turn off SmartScreen for apps and files temporarily
4. Install CursorCloak
5. **Remember to turn SmartScreen back on**

### Method 3: Portable Version
If you prefer to avoid the installer:
1. Download the ZIP version from GitHub releases
2. Extract `CursorCloak.UI.exe`
3. Right-click and "Run as administrator"

## Is CursorCloak Safe?

✅ **YES** - CursorCloak is completely safe:

- **Open Source**: Full source code available at https://github.com/JAMPANIKOMAL/CursorCloak
- **No Malware**: Uses only standard Windows APIs for cursor management
- **No Network Access**: Doesn't connect to the internet or send data
- **No Data Collection**: Doesn't collect or transmit personal information
- **Local Settings Only**: Stores preferences locally in your user folder

## For Developers

To build a signed version yourself:
1. Obtain a code signing certificate
2. Sign the executable using `signtool.exe`
3. Create the installer with signed files

## Need Help?

- **Documentation**: Check README.md in the installation folder
- **Issues**: https://github.com/JAMPANIKOMAL/CursorCloak/issues
- **Source Code**: https://github.com/JAMPANIKOMAL/CursorCloak

---

**Remember**: SmartScreen warnings are normal for new, unsigned applications. The source code is publicly available for security review.
