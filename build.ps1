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

Write-Host "CursorCloak Enhanced Build Script" -ForegroundColor Cyan
Write-Host "====================================" -ForegroundColor Cyan

# Function to check prerequisites
function Test-Prerequisites {
    Write-Host "Checking prerequisites..." -ForegroundColor Yellow
    
    # Check .NET SDK
    $dotnetVersion = dotnet --version 2>$null
    if (-not $dotnetVersion) {
        Write-Error ".NET SDK not found. Please install .NET 9.0 SDK."
        exit 1
    }
    Write-Host "   .NET SDK $dotnetVersion found" -ForegroundColor Green
    
    # Check solution file
    if (-not (Test-Path "CursorCloak.sln")) {
        Write-Error "Solution file not found. Run from project root."
        exit 1
    }
    Write-Host "   Solution file found" -ForegroundColor Green
    
    # Check InnoSetup if creating installer
    if ($CreateInstaller) {
        $innoSetupPath = "C:\Program Files (x86)\Inno Setup 6\ISCC.exe"
        if (-not (Test-Path $innoSetupPath)) {
            Write-Warning "InnoSetup not found at $innoSetupPath"
            Write-Warning "Please install InnoSetup from https://jrsoftware.org/isdl.php"
            return $false
        }
        Write-Host "   InnoSetup found at $innoSetupPath" -ForegroundColor Green
    }
    
    return $true
}

# Function to clean build artifacts
function Clear-BuildArtifacts {
    Write-Host "Cleaning build artifacts..." -ForegroundColor Yellow
    
    $cleanPaths = @(
        ".\CursorCloak.UI\bin",
        ".\CursorCloak.UI\obj", 
        ".\CursorCloak.Engine\bin",
        ".\CursorCloak.Engine\obj",
        ".\publish",
        ".\Installer\CursorCloak_Setup.exe"
    )
    
    foreach ($path in $cleanPaths) {
        if (Test-Path $path) {
            Remove-Item $path -Recurse -Force
            Write-Host "   Removed $path" -ForegroundColor Gray
        }
    }
    Write-Host "   Build artifacts cleaned" -ForegroundColor Green
}

# Function to restore NuGet packages
function Restore-Packages {
    Write-Host "Restoring NuGet packages..." -ForegroundColor Yellow
    
    dotnet restore
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Package restore failed"
        exit 1
    }
    Write-Host "   Packages restored successfully" -ForegroundColor Green
}

# Function to build solution
function Build-Solution {
    Write-Host "Building solution in Release configuration..." -ForegroundColor Yellow
    
    dotnet build --configuration Release --no-restore
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Build failed"
        exit 1
    }
    Write-Host "   Build completed successfully" -ForegroundColor Green
}

# Function to run tests
function Invoke-Tests {
    Write-Host "Running tests..." -ForegroundColor Yellow
    
    if (Test-Path ".\test.ps1") {
        & ".\test.ps1"
        if ($LASTEXITCODE -ne 0) {
            Write-Error "Tests failed"
            exit 1
        }
    } else {
        Write-Host "   No test script found, skipping tests" -ForegroundColor Yellow
    }
}

# Function to publish applications
function Publish-Applications {
    Write-Host "Publishing applications..." -ForegroundColor Yellow
    
    # Create publish directory
    New-Item -ItemType Directory -Path ".\publish" -Force | Out-Null
    New-Item -ItemType Directory -Path ".\publish\ui" -Force | Out-Null
    New-Item -ItemType Directory -Path ".\publish\engine" -Force | Out-Null
    
    # Base publish arguments
    $publishArgs = @(
        "--configuration", "Release",
        "--verbosity", "quiet",
        "--runtime", "win-x64",
        "--output", ".\publish\ui\"
    )
    
    if ($SelfContained) {
        $publishArgs += "--self-contained", "true"
        $publishArgs += "-p:PublishSingleFile=true"
        $publishArgs += "-p:EnableCompressionInSingleFile=true"
        Write-Host "   Creating self-contained deployment" -ForegroundColor Gray
    } else {
        $publishArgs += "--self-contained", "false"
        $publishArgs += "-p:PublishSingleFile=false"
        Write-Host "   Creating framework-dependent deployment" -ForegroundColor Gray
    }
    
    # Publish UI
    dotnet publish "CursorCloak.UI\CursorCloak.UI.csproj" @publishArgs
    if ($LASTEXITCODE -ne 0) {
        Write-Error "UI publish failed"
        exit 1
    }
    
    # Publish Engine
    $publishArgs[7] = ".\publish\engine\"
    dotnet publish "CursorCloak.Engine\CursorCloak.Engine.csproj" @publishArgs
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Engine publish failed"
        exit 1
    }
    
    Write-Host "   Published to .\publish\" -ForegroundColor Green
}

# Function to create installer
function New-Installer {
    Write-Host "Creating installer..." -ForegroundColor Yellow
    
    if (-not (Test-Path "setup.iss")) {
        Write-Error "InnoSetup script not found"
        exit 1
    }
    
    # Create Installer directory
    New-Item -ItemType Directory -Path ".\Installer" -Force | Out-Null
    
    # Run InnoSetup
    $innoSetupPath = "C:\Program Files (x86)\Inno Setup 6\ISCC.exe"
    if (Test-Path $innoSetupPath) {
        & $innoSetupPath "setup.iss"
        if ($LASTEXITCODE -eq 0) {
            Write-Host "   Installer created successfully" -ForegroundColor Green
        } else {
            Write-Error "Installer creation failed"
            exit 1
        }
    } else {
        Write-Warning "InnoSetup not found at $innoSetupPath. Please install InnoSetup to create installer."
    }
}

# Main execution
try {
    # Test prerequisites
    $canCreateInstaller = Test-Prerequisites
    
    # Clean if requested
    if ($Clean) {
        Clear-BuildArtifacts
    }
    
    # Restore packages
    Restore-Packages
    
    # Build solution
    Build-Solution
    
    # Run tests if requested
    if ($Test) {
        Invoke-Tests
    }
    
    # Publish if requested
    if ($Publish) {
        Publish-Applications
    }
    
    # Create installer if requested and possible
    if ($CreateInstaller -and $canCreateInstaller) {
        New-Installer
    }
    
    Write-Host ""
    Write-Host "Build process completed successfully!" -ForegroundColor Green
    Write-Host ""
    Write-Host "Output locations:" -ForegroundColor Cyan
    Write-Host "   UI: .\CursorCloak.UI\bin\Release\net9.0-windows\" -ForegroundColor Gray
    Write-Host "   Engine: .\CursorCloak.Engine\bin\Release\net9.0\" -ForegroundColor Gray
    if ($Publish) {
        Write-Host "   Published: .\publish\" -ForegroundColor Gray
    }
    if ($CreateInstaller -and (Test-Path ".\Installer\CursorCloak_Setup.exe")) {
        Write-Host "   Installer: .\Installer\CursorCloak_Setup.exe" -ForegroundColor Gray
    }
    Write-Host ""
    Write-Host "To run: Right-click CursorCloak.UI.exe -> 'Run as administrator'" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Build options:" -ForegroundColor Cyan
    Write-Host "   .\build.ps1 -Clean -Publish -SelfContained -CreateInstaller" -ForegroundColor Gray
    
} catch {
    Write-Error "Build process failed: $_"
    exit 1
}
