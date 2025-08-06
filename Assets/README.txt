# Icon Placeholder
# This directory should contain the application icon files:
# - app-icon.ico (for Windows executable)
# - app-icon.png (for InnoSetup installer)

To add an actual icon:
1. Create or obtain a 32x32 pixel icon file
2. Save it as "app-icon.ico" in this Assets directory
3. Update the CursorCloak.UI.csproj to reference it
4. Update setup.iss to use it for the installer

For now, the application will use the default Windows icon.
