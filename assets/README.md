# CursorCloak Assets

This folder contains all static assets used by the CursorCloak application.

## Folder Structure

### `/icons/`
Contains application icons used throughout the project:

- **`app-icon.ico`** - Main application icon (Windows ICO format)
  - Used for: Window title bar, taskbar, installers, shortcuts
  - Resolution: Multiple sizes (16x16, 32x32, 48x48, 256x256)
  
- **`app-icon.png`** - Application icon (PNG format) 
  - Used for: Documentation, GitHub, web representations
  - Resolution: High-resolution for clear display
  
- **`cursor-icon.png`** - Cursor-related icon
  - Used for: UI elements related to cursor functionality

## Usage in Code

### Project File References
Icons are referenced in `CursorCloak.UI.csproj` as embedded resources:
```xml
<ApplicationIcon>..\..\assets\icons\app-icon.ico</ApplicationIcon>
<Resource Include="..\..\assets\icons\app-icon.ico" />
```

### Pack URI Access
Icons are accessed in code using pack URIs:
```csharp
var iconUri = new Uri("pack://application:,,,/app-icon.ico");
```

### Installer References
Installers reference icons from this folder:
```ini
SetupIconFile=..\assets\icons\app-icon.ico
```

## Adding New Assets

When adding new assets:
1. Place files in appropriate subfolder (`icons/`, `images/`, etc.)
2. Update project file to include as embedded resource if needed
3. Update documentation to reflect new assets
4. Ensure consistent naming conventions

## Design Guidelines

### Icons
- Use consistent color scheme (dark theme friendly)
- Maintain clear visibility at small sizes (16x16)
- Follow Windows icon design guidelines
- Include multiple resolutions in ICO files
