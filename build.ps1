#!/usr/bin/env pwsh
# CursorCloak Build Script - Enhanced Version
# Builds the project in Release configuration with multiple options

param(
    [switch]$Clean,
    [switch]$Publish,
    [switch]$SelfContained,
    [switch]$CreateInstaller,
    [switch]$Test
)

Write-Host "🎯 CursorCloak Enhanced Build Script" -ForegroundColor Cyan
Write-Host "====================================" -ForegroundColor Cyan

# Function to check prerequisites
function Test-Prerequisites {
    Write-Host "🔍 Checking prerequisites..." -ForegroundColor Yellow
    
    # Check .NET SDK
    $dotnetVersion = dotnet --version 2>$null
    if (-not $dotnetVersion) {
        Write-Error "❌ .NET SDK not found. Please install .NET 9.0 SDK."
        exit 1
    }
    Write-Host "   ✅ .NET SDK $dotnetVersion found" -ForegroundColor Green
    
    # Check if running as administrator
    $isAdmin = ([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
    if (-not $isAdmin) {
        Write-Warning "⚠️  Not running as administrator. The built application will require admin privileges to run."
    } else {
        Write-Host "   ✅ Running as administrator" -ForegroundColor Green
    }
    
    # Check for Inno Setup if creating installer
    if ($CreateInstaller) {
        $innoSetupPath = "C:\Program Files (x86)\Inno Setup 6\ISCC.exe"
        if (-not (Test-Path $innoSetupPath)) {
            Write-Warning "⚠️  Inno Setup not found at $innoSetupPath. Installer creation will be skipped."
            $script:CreateInstaller = $false
        } else {
            Write-Host "   ✅ Inno Setup found" -ForegroundColor Green
        }
    }
}

# Function to clean build artifacts
function Clear-BuildArtifacts {
    Write-Host "🧹 Cleaning previous builds..." -ForegroundColor Yellow
    
    $pathsToClean = @(
        ".\bin", ".\publish", ".\Installer", 
        ".\CursorCloak.UI\bin", ".\CursorCloak.UI\obj",
        ".\CursorCloak.Engine\bin", ".\CursorCloak.Engine\obj"
    )
    
    foreach ($path in $pathsToClean) {
        if (Test-Path $path) {
            Remove-Item $path -Recurse -Force -ErrorAction SilentlyContinue
            Write-Host "   🗑️  Removed $path" -ForegroundColor Gray
        }
    }
    
    dotnet clean --configuration Release --verbosity quiet
    Write-Host "   ✅ Clean completed" -ForegroundColor Green
}

# Function to restore packages
function Restore-Packages {
    Write-Host "📦 Restoring NuGet packages..." -ForegroundColor Yellow
    dotnet restore --verbosity quiet
    if ($LASTEXITCODE -ne 0) {
        Write-Error "❌ Package restore failed"
        exit 1
    }
    Write-Host "   ✅ Packages restored" -ForegroundColor Green
}

# Function to build solution
function Build-Solution {
    Write-Host "🔨 Building solution..." -ForegroundColor Yellow
    dotnet build --configuration Release --no-restore --verbosity quiet
    if ($LASTEXITCODE -ne 0) {
        Write-Error "❌ Build failed"
        exit 1
    }
    Write-Host "   ✅ Build completed" -ForegroundColor Green
}

# Function to run tests
function Invoke-Tests {
    Write-Host "🧪 Running tests..." -ForegroundColor Yellow
    dotnet test --configuration Release --no-build --verbosity quiet
    if ($LASTEXITCODE -eq 0) {
        Write-Host "   ✅ All tests passed" -ForegroundColor Green
    } else {
        Write-Warning "⚠️  Some tests failed or no tests found"
    }
}

# Function to publish applications
function Publish-Applications {
    Write-Host "📤 Publishing applications..." -ForegroundColor Yellow
    
    $publishArgs = @(
        "--configuration", "Release"
        "--runtime", "win-x64"
        "--verbosity", "quiet"
    )
    
    if ($SelfContained) {
        $publishArgs += "--self-contained", "true"
        Write-Host "   📄 Creating self-contained deployment" -ForegroundColor Gray
    } else {
        $publishArgs += "--self-contained", "false"
        Write-Host "   📄 Creating framework-dependent deployment" -ForegroundColor Gray
    }
    
    # Publish UI
    Write-Host "   🖥️  Publishing UI application..." -ForegroundColor Gray
    $publishArgs += "--output", ".\publish\ui\"
    dotnet publish "CursorCloak.UI\CursorCloak.UI.csproj" @publishArgs
    if ($LASTEXITCODE -ne 0) {
        Write-Error "❌ UI publish failed"
        exit 1
    }
    
    # Publish Engine
    Write-Host "   ⚙️  Publishing Engine application..." -ForegroundColor Gray
    $publishArgs[-1] = ".\publish\engine\"
    dotnet publish "CursorCloak.Engine\CursorCloak.Engine.csproj" @publishArgs
    if ($LASTEXITCODE -ne 0) {
        Write-Error "❌ Engine publish failed"
        exit 1
    }
    
    Write-Host "   ✅ Published to .\publish\" -ForegroundColor Green
}

# Function to create installer
function New-Installer {
    Write-Host "📦 Creating installer..." -ForegroundColor Yellow
    
    $innoSetupPath = "C:\Program Files (x86)\Inno Setup 6\ISCC.exe"
    if (Test-Path $innoSetupPath) {
        & $innoSetupPath "setup.iss"
        if ($LASTEXITCODE -eq 0) {
            Write-Host "   ✅ Installer created successfully" -ForegroundColor Green
            if (Test-Path ".\Installer\CursorCloak_Setup.exe") {
                $size = [math]::Round((Get-Item ".\Installer\CursorCloak_Setup.exe").Length / 1MB, 2)
                Write-Host "   📁 Installer size: $size MB" -ForegroundColor Gray
            }
        } else {
            Write-Error "❌ Installer creation failed"
        }
    } else {
        Write-Warning "⚠️  Inno Setup not found, skipping installer creation"
    }
}

# Main execution
try {
    Test-Prerequisites
    
    if ($Clean) {
        Clear-BuildArtifacts
    }
    
    Restore-Packages
    Build-Solution
    
    if ($Test) {
        Invoke-Tests
    }
    
    if ($Publish) {
        Publish-Applications
    }
    
    if ($CreateInstaller -and $Publish) {
        New-Installer
    } elseif ($CreateInstaller) {
        Write-Warning "⚠️  Cannot create installer without publishing. Use -Publish -CreateInstaller"
    }
    
    Write-Host ""
    Write-Host "✅ Build process completed successfully!" -ForegroundColor Green
    Write-Host ""
    Write-Host "📁 Output locations:" -ForegroundColor Cyan
    Write-Host "   UI: .\CursorCloak.UI\bin\Release\net9.0-windows\" -ForegroundColor Gray
    Write-Host "   Engine: .\CursorCloak.Engine\bin\Release\net9.0\" -ForegroundColor Gray
    if ($Publish) {
        Write-Host "   Published: .\publish\" -ForegroundColor Gray
    }
    if ($CreateInstaller -and (Test-Path ".\Installer\CursorCloak_Setup.exe")) {
        Write-Host "   Installer: .\Installer\CursorCloak_Setup.exe" -ForegroundColor Gray
    }
    Write-Host ""
    Write-Host "⚡ To run: Right-click CursorCloak.UI.exe → 'Run as administrator'" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "💡 Build options:" -ForegroundColor Cyan
    Write-Host "   .\build.ps1 -Clean -Publish -SelfContained -CreateInstaller" -ForegroundColor Gray
    
} catch {
    Write-Error "❌ Build process failed: $_"
    exit 1
}
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
