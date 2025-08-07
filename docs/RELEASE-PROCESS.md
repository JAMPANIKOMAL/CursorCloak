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

### **Files to Update for New Version (e.g., 1.0.X → 1.0.Y)**

#### **1. Core Application Files**
```xml
<!-- src/CursorCloak.UI/CursorCloak.UI.csproj -->
<AssemblyVersion>1.0.Y.0</AssemblyVersion>
<FileVersion>1.0.Y.0</FileVersion>
```

```xml
<!-- src/CursorCloak.UI/app.manifest -->
<assemblyIdentity version="1.0.Y.0" name="CursorCloak.UI"/>
```

#### **2. CI/CD Workflow**
```yaml
# .github/workflows/build-release.yml
env:
  DOTNET_VERSION: '9.0.x'
  PROJECT_VERSION: '1.0.Y'
```

#### **3. Installer Scripts**
```iss
; scripts/setup.iss
AppVersion=1.0.Y
AppVerName=CursorCloak v1.0.Y - [Release Type]
OutputBaseFilename=CursorCloak_Setup_v1.0.Y
VersionInfoVersion=1.0.Y.0
VersionInfoProductVersion=1.0.Y.0

; Update all UI text references from v1.0.X to v1.0.Y
```

```iss
; scripts/setup-selfcontained.iss
AppVersion=1.0.Y
AppVerName=CursorCloak v1.0.Y - Self-Contained Edition
OutputBaseFilename=CursorCloak_Setup_v1.0.Y_SelfContained
VersionInfoVersion=1.0.Y.0
VersionInfoProductVersion=1.0.Y.0

; Update all UI text references from v1.0.X to v1.0.Y
```

#### **4. Build Scripts**
```powershell
# scripts/build.ps1
Write-Host "CursorCloak Build Script v1.0.Y" -ForegroundColor Cyan

# Update all file paths:
".\releases\CursorCloak-v1.0.Y-*.zip"
".\releases\CursorCloak-v1.0.Y-win-x64.zip"
".\releases\CursorCloak-v1.0.Y-win-x64-selfcontained.zip"
".\releases\CursorCloak_Setup_v1.0.Y.exe"
".\releases\CursorCloak_Setup_v1.0.Y_SelfContained.exe"
```

#### **5. Documentation**
```markdown
<!-- README.md -->
### Latest Release v1.0.Y - [Release Type]

**🆕 What's New in v1.0.Y:**
[Update with new features/fixes]

Download links:
- CursorCloak_Setup_v1.0.Y.exe
- CursorCloak_Setup_v1.0.Y_SelfContained.exe
- CursorCloak-v1.0.Y-win-x64.zip
- CursorCloak-v1.0.Y-win-x64-selfcontained.zip
```

## 🛠️ **Release Commands**

### **Step 1: Local Verification**
```bash
# Navigate to project directory
cd c:\Users\[YourName]\Documents\CursorCloak

# Clean and test build
dotnet clean
dotnet restore
dotnet build --configuration Release

# Test the application
.\src\CursorCloak.UI\bin\Release\net9.0-windows\CursorCloak.UI.exe
```

### **Step 2: Version Update**
```bash
# Create new version files using search and replace
# Use your IDE or PowerShell to update all version references

# Verify all files updated
git status
git diff
```

### **Step 3: Commit Changes**
```bash
# Stage all changes
git add .

# Commit with descriptive message
git commit -m "v1.0.Y - [Release Type]: [Brief description of changes]"

# Push to main branch
git push origin main
```

### **Step 4: Create Release Tag**
```bash
# Create annotated tag
git tag -a v1.0.Y -m "CursorCloak v1.0.Y - [Release Type]: [Detailed description]"

# Push tag to trigger release workflow
git push origin v1.0.Y
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
