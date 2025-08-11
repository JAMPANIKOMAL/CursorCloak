# CursorCloak Version Management Guide

## Version Consistency Status for v1.0.2

### ✅ **Files with CORRECT v1.0.2 References**

#### Core Application Files
- `src/CursorCloak.UI/CursorCloak.UI.csproj` (AssemblyVersion & FileVersion: 1.0.2.0) ✅
- `src/CursorCloak.UI/app.manifest` (assemblyIdentity version: 1.0.2.0) ✅

#### Build & Installer Scripts
- `scripts/setup.iss` (All version references: 1.0.2) ✅
- `scripts/setup-selfcontained.iss` (All version references: 1.0.2) ✅  
- `scripts/build.ps1` (All build script references: 1.0.2) ✅

#### CI/CD & Automation
- `.github/workflows/build-release.yml` (PROJECT_VERSION: 1.0.2) ✅

#### Documentation
- `docs/VERSION.md` (Current version: 1.0.2) ✅
- `README.md` (Latest release & download links: v1.0.2) ✅

### ✅ **All Files Updated to v1.0.2** 

#### Documentation Files
- `docs/SMARTSCREEN-INFO.md` - Updated to v1.0.2 download links ✅
- `README.md` - Updated Quick Start section to v1.0.2 references ✅

---

## 🔧 **Version Bump Checklist**

### **CRITICAL: Files That MUST Be Updated for Every Version**

When bumping from version X.Y.Z to A.B.C, update these files in this order:

#### 1. **Core Application Files** (HIGHEST PRIORITY)
```
File: src/CursorCloak.UI/CursorCloak.UI.csproj
Lines: <AssemblyVersion>A.B.C.0</AssemblyVersion>
       <FileVersion>A.B.C.0</FileVersion>
```

```
File: src/CursorCloak.UI/app.manifest  
Line: <assemblyIdentity version="A.B.C.0" name="CursorCloak.UI"/>
```

#### 2. **CI/CD Workflow** (HIGH PRIORITY)
```
File: .github/workflows/build-release.yml
Line: PROJECT_VERSION: 'A.B.C'
```

#### 3. **Installer Scripts** (HIGH PRIORITY) 
```
File: scripts/setup.iss
Lines: AppVersion=A.B.C
       AppVerName=CursorCloak vA.B.C - [Release Type]
       OutputBaseFilename=CursorCloak_Setup_vA.B.C
       VersionInfoVersion=A.B.C.0
       VersionInfoProductVersion=A.B.C.0
       WelcomeLabel2=...CursorCloak vA.B.C...
       InfoBeforeLabel=CursorCloak vA.B.C...
       FinishedLabel=CursorCloak vA.B.C...
       InfoPage title=CursorCloak vA.B.C...
       All display text containing version
```

```
File: scripts/setup-selfcontained.iss
Lines: AppVersion=A.B.C
       AppVerName=CursorCloak vA.B.C - Self-Contained Edition
       OutputBaseFilename=CursorCloak_Setup_vA.B.C_SelfContained
       VersionInfoVersion=A.B.C.0
       VersionInfoProductVersion=A.B.C.0
       All UI text containing version
```

#### 4. **Build Scripts** (HIGH PRIORITY)
```
File: scripts/build.ps1
Lines: Write-Host "CursorCloak Build Script vA.B.C"
       All file names: CursorCloak-vA.B.C-win-x64.zip
       All file names: CursorCloak_Setup_vA.B.C.exe
       All file names: CursorCloak_Setup_vA.B.C_SelfContained.exe
       All file paths in script
```

#### 5. **Documentation Files** (MEDIUM PRIORITY)
```
File: README.md
Lines: ### Latest Release vA.B.C - [Release Type]
       **🆕 What's New in vA.B.C:**
       All download links: /releases/download/vA.B.C/
       All file names in download links
```

```
File: docs/VERSION.md
Update according to versioning rule:
- Move current version to "Previous Versions" section
- Add new version as "Current Version" at top
- Update release date
- Add new features/changes
```

```
File: docs/SMARTSCREEN-INFO.md
Lines: All download links and file names
       Example text mentioning version numbers
```

#### 6. **Other Documentation** (LOW PRIORITY)
```
Files: docs/RELEASE-PROCESS.md, docs/DEPLOYMENT-STATUS.md, etc.
Update: Any examples or references to specific versions
```

---

## 🤖 **Automated Version Bump Script**

### **PowerShell Script to Update All Versions**

```powershell
# Version Update Script for CursorCloak
param(
    [Parameter(Mandatory=$true)]
    [string]$NewVersion,  # Format: "1.0.3"
    
    [Parameter(Mandatory=$true)]
    [string]$ReleaseType  # Format: "Enhanced Patch Release"
)

Write-Host "Updating CursorCloak to version $NewVersion" -ForegroundColor Cyan

# 1. Update Core Application Files
(Get-Content "src/CursorCloak.UI/CursorCloak.UI.csproj") -replace 
    '<AssemblyVersion>[\d\.]+</AssemblyVersion>', 
    "<AssemblyVersion>$NewVersion.0</AssemblyVersion>" | 
    Set-Content "src/CursorCloak.UI/CursorCloak.UI.csproj"

(Get-Content "src/CursorCloak.UI/CursorCloak.UI.csproj") -replace 
    '<FileVersion>[\d\.]+</FileVersion>', 
    "<FileVersion>$NewVersion.0</FileVersion>" | 
    Set-Content "src/CursorCloak.UI/CursorCloak.UI.csproj"

(Get-Content "src/CursorCloak.UI/app.manifest") -replace 
    'version="[\d\.]+"', 
    "version=`"$NewVersion.0`"" | 
    Set-Content "src/CursorCloak.UI/app.manifest"

# 2. Update CI/CD Workflow
(Get-Content ".github/workflows/build-release.yml") -replace 
    "PROJECT_VERSION: '[\d\.]+'" , 
    "PROJECT_VERSION: '$NewVersion'" | 
    Set-Content ".github/workflows/build-release.yml"

# 3. Update Build Script
(Get-Content "scripts/build.ps1") -replace 
    'CursorCloak Build Script v[\d\.]+', 
    "CursorCloak Build Script v$NewVersion" | 
    Set-Content "scripts/build.ps1"

# More replacements for build.ps1...
(Get-Content "scripts/build.ps1") -replace 
    'CursorCloak-v[\d\.]+-', 
    "CursorCloak-v$NewVersion-" | 
    Set-Content "scripts/build.ps1"

# 4. Update Installer Scripts  
# (Add regex replacements for setup.iss and setup-selfcontained.iss)

# 5. Update Documentation
# (Add regex replacements for README.md, VERSION.md, etc.)

Write-Host "Version update completed! Please review changes before committing." -ForegroundColor Green
Write-Host "Next steps:" -ForegroundColor Yellow
Write-Host "1. Review all changed files" -ForegroundColor Gray
Write-Host "2. Test build: .\scripts\build.ps1 -CreateAllPackages" -ForegroundColor Gray
Write-Host "3. Commit changes: git add . && git commit -m 'v$NewVersion - $ReleaseType'" -ForegroundColor Gray
Write-Host "4. Push and create release" -ForegroundColor Gray
```

---

## 📋 **Manual Verification Checklist**

After running version update, verify these locations:

### **Core Files**
- [ ] `src/CursorCloak.UI/CursorCloak.UI.csproj` - AssemblyVersion & FileVersion
- [ ] `src/CursorCloak.UI/app.manifest` - assemblyIdentity version  

### **Build System**
- [ ] `.github/workflows/build-release.yml` - PROJECT_VERSION
- [ ] `scripts/build.ps1` - Script title and all file names
- [ ] `scripts/setup.iss` - All version fields and UI text
- [ ] `scripts/setup-selfcontained.iss` - All version fields and UI text

### **Documentation** 
- [ ] `README.md` - Latest release section and download links
- [ ] `docs/VERSION.md` - Current version moved to previous, new version added
- [ ] `docs/SMARTSCREEN-INFO.md` - Download links updated

### **Build Test**
- [ ] Run `.\scripts\build.ps1 -CreateAllPackages`
- [ ] Verify 4 files created with correct version numbers
- [ ] Check file sizes are reasonable

---

## 🎯 **Version Bump Rules**

### **Semantic Versioning**
- **Patch (X.Y.Z+1)**: Bug fixes, small improvements
- **Minor (X.Y+1.0)**: New features, non-breaking changes  
- **Major (X+1.0.0)**: Breaking changes, major redesign

### **Release Types**
- **Patch Release**: "Patch Release" or "Enhanced Patch Release"
- **Minor Release**: "Feature Release" or "Enhanced Release"  
- **Major Release**: "Major Release" or "Professional Edition"

### **File Naming Convention**
- Installers: `CursorCloak_Setup_vX.Y.Z.exe` and `CursorCloak_Setup_vX.Y.Z_SelfContained.exe`
- Portable: `CursorCloak-vX.Y.Z-win-x64.zip` and `CursorCloak-vX.Y.Z-win-x64-selfcontained.zip`
- GitHub Release Tag: `vX.Y.Z`

---

## ⚡ **Quick Command Reference**

```bash
# Search for version references
grep -r "1\.0\.2" --exclude-dir=.git .

# Update version in specific file
sed -i 's/1\.0\.2/1\.0\.3/g' filename

# Build and test
.\scripts\build.ps1 -CreateAllPackages

# Create release
gh release create v1.0.3 --title "CursorCloak v1.0.3 - Release Type" --notes "Release notes here" files...
```

---

*This guide ensures consistent version management across the entire CursorCloak project.*
