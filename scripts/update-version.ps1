# CursorCloak Version Update Script
# Usage: .\update-version.ps1 -NewVersion "1.0.3" -ReleaseType "Patch Release"

param(
    [Parameter(Mandatory=$true)]
    [string]$NewVersion,  # Format: "1.0.3"
    
    [Parameter(Mandatory=$true)]
    [string]$ReleaseType  # Format: "Enhanced Patch Release"
)

Write-Host "Updating CursorCloak to version $NewVersion ($ReleaseType)" -ForegroundColor Cyan
Write-Host ""

# Validate version format
if ($NewVersion -notmatch '^\d+\.\d+\.\d+$') {
    Write-Error "Invalid version format. Use X.Y.Z"
    exit 1
}

# Function to safely replace content in files
function Update-FileContent {
    param($FilePath, $SearchPattern, $ReplaceWith, $Description)
    
    if (Test-Path $FilePath) {
        Write-Host "  Updating $Description..." -ForegroundColor Gray
        try {
            (Get-Content $FilePath -Raw) -replace $SearchPattern, $ReplaceWith | Set-Content $FilePath -NoNewline
            Write-Host "    $FilePath updated" -ForegroundColor Green
        } catch {
            Write-Host "    Failed to update $FilePath : $_" -ForegroundColor Red
        }
    } else {
        Write-Host "    File not found: $FilePath" -ForegroundColor Yellow
    }
}

Write-Host "1. Updating Core Application Files..." -ForegroundColor Yellow

# Update CursorCloak.UI.csproj
Update-FileContent `
    "src/CursorCloak.UI/CursorCloak.UI.csproj" `
    '<AssemblyVersion>[\d\.]+</AssemblyVersion>' `
    "<AssemblyVersion>$NewVersion.0</AssemblyVersion>" `
    "AssemblyVersion"

Update-FileContent `
    "src/CursorCloak.UI/CursorCloak.UI.csproj" `
    '<FileVersion>[\d\.]+</FileVersion>' `
    "<FileVersion>$NewVersion.0</FileVersion>" `
    "FileVersion"

# Update app.manifest
Update-FileContent `
    "src/CursorCloak.UI/app.manifest" `
    'version="[\d\.]+"' `
    "version=`"$NewVersion.0`"" `
    "App Manifest Version"

Write-Host ""
Write-Host "2. Updating CI/CD Workflow..." -ForegroundColor Yellow

# Update GitHub workflow
Update-FileContent `
    ".github/workflows/build-release.yml" `
    "PROJECT_VERSION: '[\d\.]+'" `
    "PROJECT_VERSION: '$NewVersion'" `
    "CI/CD Project Version"

Write-Host ""
Write-Host "3. Updating Build Scripts..." -ForegroundColor Yellow

# Update build.ps1
Update-FileContent `
    "scripts/build.ps1" `
    'CursorCloak Build Script v[\d\.]+' `
    "CursorCloak Build Script v$NewVersion" `
    "Build Script Title"

Update-FileContent `
    "scripts/build.ps1" `
    'CursorCloak-v[\d\.]+-' `
    "CursorCloak-v$NewVersion-" `
    "Build Script File Names"

Update-FileContent `
    "scripts/build.ps1" `
    'CursorCloak_Setup_v[\d\.]+' `
    "CursorCloak_Setup_v$NewVersion" `
    "Build Script Installer Names"

Write-Host ""
Write-Host "4. Updating Installer Scripts..." -ForegroundColor Yellow

# Update setup.iss
Update-FileContent `
    "scripts/setup.iss" `
    'CursorCloak v[\d\.]+ - Enhanced Release' `
    "CursorCloak v$NewVersion - $ReleaseType" `
    "Main Installer Script"

Update-FileContent `
    "scripts/setup.iss" `
    'AppVersion=[\d\.]+' `
    "AppVersion=$NewVersion" `
    "Main Installer AppVersion"

Update-FileContent `
    "scripts/setup.iss" `
    'AppVerName=CursorCloak v[\d\.]+ - Enhanced Release' `
    "AppVerName=CursorCloak v$NewVersion - $ReleaseType" `
    "Main Installer AppVerName"

Update-FileContent `
    "scripts/setup.iss" `
    'OutputBaseFilename=CursorCloak_Setup_v[\d\.]+' `
    "OutputBaseFilename=CursorCloak_Setup_v$NewVersion" `
    "Main Installer Output Name"

Update-FileContent `
    "scripts/setup.iss" `
    'VersionInfoVersion=[\d\.]+\.0' `
    "VersionInfoVersion=$NewVersion.0" `
    "Main Installer Version Info"

Update-FileContent `
    "scripts/setup.iss" `
    'VersionInfoProductVersion=[\d\.]+\.0' `
    "VersionInfoProductVersion=$NewVersion.0" `
    "Main Installer Product Version"

# Update setup-selfcontained.iss
Update-FileContent `
    "scripts/setup-selfcontained.iss" `
    'CursorCloak v[\d\.]+ - Self-Contained Release' `
    "CursorCloak v$NewVersion - Self-Contained Edition" `
    "Self-Contained Installer Script"

Update-FileContent `
    "scripts/setup-selfcontained.iss" `
    'AppVersion=[\d\.]+' `
    "AppVersion=$NewVersion" `
    "Self-Contained Installer AppVersion"

Update-FileContent `
    "scripts/setup-selfcontained.iss" `
    'OutputBaseFilename=CursorCloak_Setup_v[\d\.]+_SelfContained' `
    "OutputBaseFilename=CursorCloak_Setup_v$NewVersion_SelfContained" `
    "Self-Contained Installer Output Name"

Write-Host ""
Write-Host "5. Updating Documentation..." -ForegroundColor Yellow

# Update README.md
Update-FileContent `
    "README.md" `
    'Latest Release v[\d\.]+ - Enhanced Release' `
    "Latest Release v$NewVersion - $ReleaseType" `
    "README Latest Release Header"

Update-FileContent `
    "README.md" `
    'What''s New in v[\d\.]+:' `
    "What's New in v$NewVersion:" `
    "README What's New Section"

Update-FileContent `
    "README.md" `
    '/releases/download/v[\d\.]+/' `
    "/releases/download/v$NewVersion/" `
    "README Download Links"

Update-FileContent `
    "README.md" `
    'CursorCloak_Setup_v[\d\.]+\.exe' `
    "CursorCloak_Setup_v$NewVersion.exe" `
    "README Installer File Names"

Update-FileContent `
    "README.md" `
    'CursorCloak-v[\d\.]+-win-x64' `
    "CursorCloak-v$NewVersion-win-x64" `
    "README ZIP File Names"

# Update SMARTSCREEN-INFO.md
Update-FileContent `
    "docs/SMARTSCREEN-INFO.md" `
    'CursorCloak_Setup_v[\d\.]+\.exe' `
    "CursorCloak_Setup_v$NewVersion.exe" `
    "SmartScreen Installer References"

Update-FileContent `
    "docs/SMARTSCREEN-INFO.md" `
    'CursorCloak-v[\d\.]+-win-x64' `
    "CursorCloak-v$NewVersion-win-x64" `
    "SmartScreen ZIP References"

Write-Host ""
Write-Host "Version update completed!" -ForegroundColor Green
Write-Host ""
Write-Host "📋 Next Steps:" -ForegroundColor Cyan
Write-Host "1. Review all changed files for accuracy" -ForegroundColor White
Write-Host "2. Update docs/VERSION.md manually (add new version, move current to previous)" -ForegroundColor White
Write-Host "3. Test build: .\scripts\build.ps1 -CreateAllPackages" -ForegroundColor White
Write-Host "4. Commit changes: git add . && git commit -m 'v$NewVersion - $ReleaseType'" -ForegroundColor White
Write-Host "5. Push changes: git push origin main" -ForegroundColor White
Write-Host "6. Create GitHub release: gh release create v$NewVersion" -ForegroundColor White
Write-Host ""
Write-Host "⚠️  Manual Updates Required:" -ForegroundColor Yellow
Write-Host "   • docs/VERSION.md - Follow versioning rule to add new version" -ForegroundColor Gray
Write-Host "   • Any installer UI text that mentions specific features" -ForegroundColor Gray
Write-Host "   • Release notes and changelog entries" -ForegroundColor Gray
