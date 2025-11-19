# CursorCloak Version Verification Script
# Usage: .\verify-version.ps1 -ExpectedVersion "2.0.1"

param(
    [Parameter(Mandatory=$true)]
    [string]$ExpectedVersion
)

$ErrorCount = 0

function Check-FileContent {
    param($FilePath, $Pattern, $Description)
    
    if (Test-Path $FilePath) {
        $content = Get-Content $FilePath -Raw
        if ($content -match $Pattern) {
            Write-Host "✅ $Description matches $ExpectedVersion" -ForegroundColor Green
        } else {
            Write-Host "❌ $Description matches MISMATCH in $FilePath" -ForegroundColor Red
            $global:ErrorCount++
        }
    } else {
        Write-Host "⚠️  File not found: $FilePath" -ForegroundColor Yellow
        $global:ErrorCount++
    }
}

Write-Host "Verifying CursorCloak version is $ExpectedVersion..." -ForegroundColor Cyan
Write-Host ""

# 1. CSPROJ
Check-FileContent "src/CursorCloak.UI/CursorCloak.UI.csproj" "<AssemblyVersion>$ExpectedVersion.0</AssemblyVersion>" "AssemblyVersion"
Check-FileContent "src/CursorCloak.UI/CursorCloak.UI.csproj" "<FileVersion>$ExpectedVersion.0</FileVersion>" "FileVersion"

# 2. App Manifest
Check-FileContent "src/CursorCloak.UI/app.manifest" "version=`"$ExpectedVersion.0`"" "Manifest Version"

# 3. Setup Scripts
Check-FileContent "scripts/setup.iss" "AppVersion=$ExpectedVersion" "Setup.iss AppVersion"
Check-FileContent "scripts/setup.iss" "OutputBaseFilename=CursorCloak_Setup_v$ExpectedVersion" "Setup.iss OutputFilename"
Check-FileContent "scripts/setup-selfcontained.iss" "AppVersion=$ExpectedVersion" "Setup-selfcontained.iss AppVersion"

# 4. Build Script
Check-FileContent "scripts/build.ps1" "CursorCloak-v$ExpectedVersion-" "Build Script Filenames"

# 5. Documentation
Check-FileContent "README.md" "CursorCloak_Setup_v$ExpectedVersion.exe" "README Installer Link"
Check-FileContent "docs/VERSION.md" "## Current Version: v$ExpectedVersion" "VERSION.md Header"
Check-FileContent ".github/workflows/build-release.yml" "PROJECT_VERSION: '$ExpectedVersion'" "GitHub Workflow Version"

Write-Host ""
if ($ErrorCount -eq 0) {
    Write-Host "All version checks passed!" -ForegroundColor Green
} else {
    Write-Host "Found $ErrorCount version mismatches." -ForegroundColor Red
    exit 1
}
