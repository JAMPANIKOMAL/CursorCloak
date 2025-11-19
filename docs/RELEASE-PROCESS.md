# 🚀 CursorCloak Release Process Documentation

This document provides a complete guide for creating new releases of CursorCloak.

## 📋 **Pre-Release Checklist**

### **1. Code Quality Checks**
- [ ] ✅ All code changes committed and pushed
- [ ] ✅ Local build succeeds: `dotnet build --configuration Release`
- [ ] ✅ No compiler warnings or errors
- [ ] ✅ Manual testing completed on Windows 10/11
- [ ] ✅ Administrator privileges tested
- [ ] ✅ Global hotkeys (Alt+H/Alt+S) working
- [ ] ✅ Settings persistence verified
- [ ] ✅ Background mode functioning

### **2. Version Consistency Check**
- [ ] ✅ All version numbers match across files
- [ ] ✅ Assembly versions updated
- [ ] ✅ Installer scripts updated
- [ ] ✅ Documentation updated
- [ ] ✅ CI/CD workflow environment variables updated

## 🔢 **Version Update Process**

## 🤖 **Automated Version Update (Recommended)**

We have created a PowerShell script to automate updating version numbers across all files.

### **Step 1: Run the Update Script**
```powershell
# Usage: .\scripts\update-version.ps1 -NewVersion "X.Y.Z" -ReleaseType "Release Name"
.\scripts\update-version.ps1 -NewVersion "2.0.1" -ReleaseType "Patch Release"
```

**This script automatically updates:**
- `src/CursorCloak.UI/CursorCloak.UI.csproj`
- `src/CursorCloak.UI/app.manifest`
- `.github/workflows/build-release.yml`
- `scripts/build.ps1`
- `scripts/setup.iss`
- `scripts/setup-selfcontained.iss`
- `README.md`
- `docs/SMARTSCREEN-INFO.md`

### **Step 2: Manual Updates (Required)**
The script will remind you to manually update these files:
1.  **`docs/VERSION.md`**: Move current version to history and add new version details.
2.  **`CHANGELOG.md`**: Add the new version entry at the top.

### **Step 3: Verify & Commit**
```bash
# Verify changes
git diff

# Commit
git add .
git commit -m "chore: Release v2.0.1"
```

### **Step 4: Tag & Push**
```bash
# Push commit first
git push origin main

# Create and push tag
git tag v2.0.1
git push origin v2.0.1
```

## 🔍 **Quality Assurance Checklist**

### **Pre-Push Verification**
- [ ] ✅ Search for old version numbers: `grep -r "1\.0\.X" --exclude-dir=.git`
- [ ] ✅ Local build succeeds without warnings
- [ ] ✅ Version numbers consistent across all files
- [ ] ✅ Git status shows only intended changes
- [ ] ✅ Documentation updated with new features

### **Post-Push Verification**
- [ ] ✅ GitHub Actions build succeeds
- [ ] ✅ All 4 packages created (2 installers + 2 ZIP files)
- [ ] ✅ InnoSetup installers build successfully
- [ ] ✅ Release created automatically on GitHub
- [ ] ✅ Download links work correctly
- [ ] ✅ File sizes reasonable (~2MB framework, ~66MB self-contained)

## 🎯 **Release Types**

### **Patch Release (1.0.X → 1.0.X+1)**
- Bug fixes
- Small improvements
- No breaking changes
- Example: `v1.0.1` → `v1.0.2`

### **Minor Release (1.X.0 → 1.X+1.0)**
- New features
- Non-breaking changes
- Enhanced functionality
- Example: `v1.0.2` → `v1.1.0`

### **Major Release (X.0.0 → X+1.0.0)**
- Breaking changes
- Major new features
- Architecture changes
- Example: `v1.1.0` → `v2.0.0`

## 📦 **Expected Build Outputs**

### **GitHub Actions Artifacts**
```
releases/
├── CursorCloak_Setup_v1.0.Y.exe                    (~2MB)
├── CursorCloak_Setup_v1.0.Y_SelfContained.exe     (~66MB)
├── CursorCloak-v1.0.Y-win-x64.zip                 (~0.3MB)
└── CursorCloak-v1.0.Y-win-x64-selfcontained.zip   (~66MB)
```

### **GitHub Release**
- Automatic release creation with tag `v1.0.Y`
- Release notes from commit messages
- All 4 files attached as downloadable assets
- Professional release description with feature highlights

## 🚨 **Troubleshooting Common Issues**

### **InnoSetup Build Fails**
- **Problem**: "Process completed with exit code 1"
- **Solution**: Check installer scripts for syntax errors
- **Verify**: InnoSetup paths and file references are correct

### **Version Mismatch**
- **Problem**: Some files still show old version
- **Solution**: Use global search/replace across entire project
- **Check**: Assembly versions, installer scripts, documentation

### **GitHub Actions Timeout**
- **Problem**: Build takes too long
- **Solution**: Check for network issues or dependency problems
- **Monitor**: Build logs for specific step failures

### **Release Not Created**
- **Problem**: Tag pushed but no release appears
- **Solution**: Check if build job completed successfully
- **Verify**: Release job only runs on successful build

## 📝 **Version History Template**

Add to `docs/VERSION.md`:

```markdown
## Current Version: 1.0.Y - [Release Type]

### Release Date: [Date]

### What's New in v1.0.Y:
- 🔧 **[Feature/Fix Name]**: [Description]
- ✅ **[Feature/Fix Name]**: [Description]
- 🛡️ **[Feature/Fix Name]**: [Description]

### Technical Changes:
- ✅ [Technical improvement]
- ✅ [Technical improvement]

### Bug Fixes:
- ✅ Fixed [issue description]
- ✅ Resolved [issue description]
```

## 🎉 **Release Validation**

After release creation:

1. **Download and Test All Packages**
   - [ ] Framework installer works
   - [ ] Self-contained installer works
   - [ ] Framework ZIP extracts and runs
   - [ ] Self-contained ZIP extracts and runs

2. **Verify Functionality**
   - [ ] Alt+H hides cursor
   - [ ] Alt+S shows cursor
   - [ ] Settings persist between sessions
   - [ ] "Start with Windows" works
   - [ ] Background mode functions
   - [ ] Uninstaller removes all traces

3. **Check Release Quality**
   - [ ] Download links work
   - [ ] File sizes correct
   - [ ] Version numbers displayed correctly
   - [ ] Release notes accurate

---

## 🔧 **Quick Reference Commands**

```bash
# Complete release workflow
cd c:\Users\[User]\Documents\CursorCloak
git pull origin main
# [Update all version numbers]
dotnet build --configuration Release
git add .
git commit -m "v1.0.Y - [Release]: [Description]"
git push origin main
git tag -a v1.0.Y -m "CursorCloak v1.0.Y - [Description]"
git push origin v1.0.Y
```

**Remember**: Always test locally before releasing! 🚀
