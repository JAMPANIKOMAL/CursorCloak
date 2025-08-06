# CursorCloak Troubleshooting Guide

## Error: 0xc000041d (Fatal Exception)

This error typically occurs when the application crashes during startup. Here are the common causes and solutions:

### 1. Administrator Privileges Required
**Problem**: CursorCloak needs admin rights to modify system cursors
**Solution**: 
- Always run as administrator
- The installer should request admin privileges automatically
- If running from development, right-click executable → "Run as administrator"

### 2. Missing .NET Runtime
**Problem**: .NET 9.0 runtime not installed
**Solution**:
- Download and install .NET 9.0 Runtime from Microsoft
- Or use self-contained deployment (see build instructions)

### 3. Windows Compatibility
**Problem**: Older Windows versions may have compatibility issues
**Solution**:
- Ensure Windows 10 or later (recommended)
- Windows 7/8.1 may work but are not officially supported

### 4. Hotkey Conflicts
**Problem**: Alt+H or Alt+S already in use by another application
**Solution**:
- Close other applications that might use these hotkeys
- Check for accessibility software or gaming software

### 5. Antivirus Interference
**Problem**: Antivirus blocking cursor manipulation
**Solution**:
- Add CursorCloak to antivirus exceptions
- Temporarily disable real-time protection for testing

## Building and Installation Issues

### Build Failures
1. Ensure .NET 9.0 SDK is installed
2. Run build scripts as administrator
3. Check for path length limitations
4. Verify all NuGet packages restore correctly

### Installer Issues
1. Ensure InnoSetup is installed
2. Compile setup.iss as administrator
3. Check that release binaries exist before creating installer
4. Verify Windows Defender doesn't quarantine the installer

### Runtime Issues
1. **Application won't start**:
   - Check Windows Event Viewer for detailed error messages
   - Verify .NET runtime is installed
   - Try running from command line to see error messages

2. **Hotkeys not working**:
   - Ensure application has focus initially
   - Check for hotkey conflicts with other software
   - Verify administrator privileges

3. **Cursor not hiding/showing**:
   - Some applications override cursor visibility
   - Try moving mouse to refresh cursor state
   - Restart application if cursor gets stuck

## Development Issues

### VS Code Linting Errors
- The linter may show false positives for generated code
- If the project builds successfully, these can be ignored
- Clean and rebuild to refresh IntelliSense

### Resource Management
- The application properly cleans up GDI resources
- Memory leaks should not occur with proper error handling
- Monitor with Task Manager if concerned about resource usage

## Performance Optimization

### Memory Usage
- Application uses minimal memory (~10-20MB)
- Most memory usage is from .NET runtime
- No memory leaks should occur with current implementation

### CPU Usage
- Minimal CPU usage when idle
- Brief spikes during cursor show/hide operations
- Hotkey detection is efficient and low-impact

## Deployment Best Practices

1. **Always build in Release mode** for deployment
2. **Test on clean machine** without development tools
3. **Include all dependencies** in installer
4. **Request admin privileges** in manifest and installer
5. **Provide clear error messages** for end users

## Support Information

If you continue to experience issues:

1. Check Windows Event Viewer (Windows Logs → Application)
2. Look for CursorCloak-related error entries
3. Note exact error codes and timestamps
4. Test with minimal system (safe mode if necessary)
5. Verify compatibility with your Windows version

## Version Information

- Application Version: 1.0.0.0
- Target Framework: .NET 9.0
- Minimum OS: Windows 10 (recommended)
- Required Privileges: Administrator

## Known Limitations

1. Some full-screen applications may override cursor visibility
2. Remote desktop sessions may have limited functionality
3. Multiple monitor setups work but cursor state is global
4. Gaming applications may conflict with global hotkeys
