#!/usr/bin/env pwsh
# CursorCloak Build Script
# Builds the project in Release configuration

param(
    [switch]$Clean,
    [switch]$Publish,
    [switch]$SelfContained
)

Write-Host "🎯 CursorCloak Build Script" -ForegroundColor Cyan
Write-Host "===========================" -ForegroundColor Cyan

# Check if running as administrator
$isAdmin = ([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
if (-not $isAdmin) {
    Write-Warning "⚠️  Not running as administrator. The built application will require admin privileges to run."
}

# Clean if requested
if ($Clean) {
    Write-Host "🧹 Cleaning previous builds..." -ForegroundColor Yellow
    dotnet clean --configuration Release
    if (Test-Path ".\bin") { Remove-Item ".\bin" -Recurse -Force }
    if (Test-Path ".\publish") { Remove-Item ".\publish" -Recurse -Force }
}

# Restore packages
Write-Host "📦 Restoring NuGet packages..." -ForegroundColor Yellow
dotnet restore
if ($LASTEXITCODE -ne 0) {
    Write-Error "❌ Package restore failed"
    exit 1
}

# Build solution
Write-Host "🔨 Building solution..." -ForegroundColor Yellow
dotnet build --configuration Release --no-restore
if ($LASTEXITCODE -ne 0) {
    Write-Error "❌ Build failed"
    exit 1
}

# Publish if requested
if ($Publish) {
    Write-Host "📤 Publishing applications..." -ForegroundColor Yellow
    
    $publishArgs = @(
        "--configuration", "Release"
        "--runtime", "win-x64"
        "--output", ".\publish\ui\"
    )
    
    if ($SelfContained) {
        $publishArgs += "--self-contained", "true"
        Write-Host "   📄 Creating self-contained deployment" -ForegroundColor Gray
    } else {
        $publishArgs += "--self-contained", "false"
        Write-Host "   📄 Creating framework-dependent deployment" -ForegroundColor Gray
    }
    
    # Publish UI
    dotnet publish "CursorCloak.UI\CursorCloak.UI.csproj" @publishArgs
    if ($LASTEXITCODE -ne 0) {
        Write-Error "❌ UI publish failed"
        exit 1
    }
    
    # Publish Engine
    $publishArgs[5] = ".\publish\engine\"
    dotnet publish "CursorCloak.Engine\CursorCloak.Engine.csproj" @publishArgs
    if ($LASTEXITCODE -ne 0) {
        Write-Error "❌ Engine publish failed"
        exit 1
    }
    
    Write-Host "✅ Published to .\publish\" -ForegroundColor Green
}

Write-Host "✅ Build completed successfully!" -ForegroundColor Green
Write-Host ""
Write-Host "📁 Output locations:" -ForegroundColor Cyan
Write-Host "   UI: .\CursorCloak.UI\bin\Release\net9.0-windows\" -ForegroundColor Gray
Write-Host "   Engine: .\CursorCloak.Engine\bin\Release\net9.0\" -ForegroundColor Gray
if ($Publish) {
    Write-Host "   Published: .\publish\" -ForegroundColor Gray
}
Write-Host ""
Write-Host "⚡ To run: Right-click CursorCloak.UI.exe → 'Run as administrator'" -ForegroundColor Yellow
