# Release Upload Instructions

## Current Status
✅ Tags created: v1.0.0, v1.1.0, v1.2.0
✅ GitHub releases created automatically from tags
❌ Release assets (ZIP files) need to be uploaded manually

## Available Release Files
- `CursorCloak-v1.2.0-win-x64.zip` (56.8 MB) - Standard build
- `CursorCloak-v1.2.0-win-x64-optimized.zip` (57.2 MB) - Optimized build

## How to Upload Release Assets

### Method 1: GitHub Web Interface (Recommended)
1. Go to https://github.com/JAMPANIKOMAL/CursorCloak/releases
2. Click on the v1.2.0 release (should be automatically created from the tag)
3. Click "Edit release" 
4. Drag and drop or click to upload the ZIP files:
   - `CursorCloak-v1.2.0-win-x64.zip`
   - `CursorCloak-v1.2.0-win-x64-optimized.zip`
5. Add release notes from `RELEASE-NOTES-v1.2.0.md`
6. Click "Update release"

### Method 2: GitHub CLI (if installed)
```powershell
# Install GitHub CLI if not installed: winget install GitHub.cli

# Upload release assets
gh release upload v1.2.0 CursorCloak-v1.2.0-win-x64.zip CursorCloak-v1.2.0-win-x64-optimized.zip

# Update release notes
gh release edit v1.2.0 --notes-file RELEASE-NOTES-v1.2.0.md
```

### Method 3: Create Release via GitHub CLI
```powershell
# Create release with assets in one command
gh release create v1.2.0 CursorCloak-v1.2.0-win-x64.zip CursorCloak-v1.2.0-win-x64-optimized.zip --title "CursorCloak v1.2.0 - Self-Contained Edition" --notes-file RELEASE-NOTES-v1.2.0.md
```

## After Upload
Once the release assets are uploaded:
1. The download links in README.md will work correctly
2. Users can download the ZIP files directly from the releases page
3. The "Latest Release" badge will point to v1.2.0

## Files to Include in Release
- ✅ `CursorCloak-v1.2.0-win-x64.zip` - Main self-contained build
- ✅ `CursorCloak-v1.2.0-win-x64-optimized.zip` - Optimized version
- ✅ Release notes from `RELEASE-NOTES-v1.2.0.md`
