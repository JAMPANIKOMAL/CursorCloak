#!/usr/bin/env pwsh
# CursorCloak Test Script
# Tests the built application functionality

param(
    [string]$BuildPath = ".\src\CursorCloak.UI\bin\Release\net9.0-windows\",
    [switch]$Verbose
)

Write-Host "CursorCloak Test Script" -ForegroundColor Cyan
Write-Host "=======================" -ForegroundColor Cyan

# Function to check if running as administrator
function Test-Administrator {
    $isAdmin = ([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
    if (-not $isAdmin) {
        Write-Warning "Not running as administrator. Some tests may fail."
        return $false
    }
    Write-Host "Running as administrator" -ForegroundColor Green
    return $true
}

# Function to test build files
function Test-BuildFiles {
    Write-Host "Testing build outputs..." -ForegroundColor Yellow
    
    $requiredFiles = @(
        "CursorCloak.UI.exe",
        "CursorCloak.UI.dll",
        "CursorCloak.UI.runtimeconfig.json"
    )
    
    $allFilesExist = $true
    
    foreach ($file in $requiredFiles) {
        $filePath = Join-Path $BuildPath $file
        if (Test-Path $filePath) {
            $size = [math]::Round((Get-Item $filePath).Length / 1KB, 2)
            Write-Host "   $file ($size KB)" -ForegroundColor Green
        } else {
            Write-Host "   $file (missing)" -ForegroundColor Red
            $allFilesExist = $false
        }
    }
    
    if ($allFilesExist) {
        Write-Host "All required files found" -ForegroundColor Green
    } else {
        Write-Host "Some required files are missing" -ForegroundColor Red
    }
    
    return $allFilesExist
}

# Function to test dependencies
function Test-Dependencies {
    Write-Host "Testing dependencies..." -ForegroundColor Yellow
    
    # Check for critical dependencies
    $depsPath = Join-Path $BuildPath "CursorCloak.UI.deps.json"
    if (Test-Path $depsPath) {
        Write-Host "   Dependencies file found" -ForegroundColor Green
        
        # Read and parse dependencies
        try {
            $deps = Get-Content $depsPath | ConvertFrom-Json
            $runtimeTarget = $deps.runtimeTarget.name
            Write-Host "   Runtime target: $runtimeTarget" -ForegroundColor Gray
            
            $libraries = $deps.libraries
            if ($libraries) {
                $libCount = $libraries.PSObject.Properties.Count
                Write-Host "   Libraries: $libCount dependencies" -ForegroundColor Gray
            }
        } catch {
            Write-Warning "Could not parse dependencies file"
        }
    } else {
        Write-Host "   Dependencies file missing" -ForegroundColor Red
        return $false
    }
    
    return $true
}

# Function to test executable
function Test-Executable {
    Write-Host "Testing executable..." -ForegroundColor Yellow
    
    $exePath = Join-Path $BuildPath "CursorCloak.UI.exe"
    if (-not (Test-Path $exePath)) {
        Write-Host "   Executable not found" -ForegroundColor Red
        return $false
    }
    
    # Test file properties
    try {
        $fileInfo = Get-Item $exePath
        $size = [math]::Round($fileInfo.Length / 1MB, 2)
        Write-Host "   Executable found ($size MB)" -ForegroundColor Green
        Write-Host "   Created: $($fileInfo.CreationTime)" -ForegroundColor Gray
        Write-Host "   Modified: $($fileInfo.LastWriteTime)" -ForegroundColor Gray
        
        # Check if it's a valid PE file
        $bytes = [System.IO.File]::ReadAllBytes($exePath)
        if ($bytes.Length -gt 2 -and $bytes[0] -eq 0x4D -and $bytes[1] -eq 0x5A) {
            Write-Host "   Valid PE executable" -ForegroundColor Green
        } else {
            Write-Host "   Invalid executable format" -ForegroundColor Red
            return $false
        }
        
    } catch {
        Write-Host "   Error reading executable: $_" -ForegroundColor Red
        return $false
    }
    
    return $true
}

# Function to test installer
function Test-Installer {
    Write-Host "Testing installer..." -ForegroundColor Yellow
    
    $installerPath = ".\Installer\CursorCloak_Setup.exe"
    if (Test-Path $installerPath) {
        $size = [math]::Round((Get-Item $installerPath).Length / 1MB, 2)
        Write-Host "   Installer found ($size MB)" -ForegroundColor Green
        return $true
    } else {
        Write-Host "   Installer not found (run with -CreateInstaller)" -ForegroundColor Yellow
        return $false
    }
}

# Function to test setup.iss configuration
function Test-SetupConfig {
    Write-Host "Testing setup configuration..." -ForegroundColor Yellow
    
    if (-not (Test-Path "scripts\setup.iss")) {
        Write-Host "   Setup script not found" -ForegroundColor Red
        return $false
    }
    
    $setupContent = Get-Content "scripts\setup.iss" -Raw
    
    # Check for duplicate sections
    $tasksSections = ($setupContent -split '\n' | Where-Object { $_ -match '^\[Tasks\]' }).Count
    if ($tasksSections -gt 1) {
        Write-Host "   WARNING: Multiple [Tasks] sections found" -ForegroundColor Red
        return $false
    } elseif ($tasksSections -eq 1) {
        Write-Host "   Tasks section configured correctly" -ForegroundColor Green
    }
    
    # Check for required sections
    $requiredSections = @('\[Setup\]', '\[Files\]', '\[Icons\]')
    $allSectionsFound = $true
    
    foreach ($section in $requiredSections) {
        if ($setupContent -match $section) {
            Write-Host "   $($section -replace '\\', '') section found" -ForegroundColor Green
        } else {
            Write-Host "   $($section -replace '\\', '') section missing" -ForegroundColor Red
            $allSectionsFound = $false
        }
    }
    
    return $allSectionsFound
}

# Function to test project structure
function Test-ProjectStructure {
    Write-Host "Testing project structure..." -ForegroundColor Yellow
    
    $requiredDirs = @(
        "src\CursorCloak.UI",
        "src\CursorCloak.Engine"
    )
    
    $requiredFiles = @(
        "CursorCloak.sln",
        "scripts\build.ps1",
        "scripts\setup.iss"
    )
    
    $allItemsExist = $true
    
    foreach ($dir in $requiredDirs) {
        if (Test-Path $dir -PathType Container) {
            Write-Host "   $dir/ directory found" -ForegroundColor Green
        } else {
            Write-Host "   $dir/ directory missing" -ForegroundColor Red
            $allItemsExist = $false
        }
    }
    
    foreach ($file in $requiredFiles) {
        if (Test-Path $file -PathType Leaf) {
            Write-Host "   $file found" -ForegroundColor Green
        } else {
            Write-Host "   $file missing" -ForegroundColor Red
            $allItemsExist = $false
        }
    }
    
    return $allItemsExist
}

# Main test execution
try {
    Write-Host ""
    
    # Run all tests
    $adminTest = Test-Administrator
    $structureTest = Test-ProjectStructure
    $buildTest = Test-BuildFiles
    $depsTest = Test-Dependencies
    $exeTest = Test-Executable
    $setupTest = Test-SetupConfig
    $installerTest = Test-Installer
    
    Write-Host ""
    Write-Host "Test Results Summary:" -ForegroundColor Cyan
    Write-Host "====================" -ForegroundColor Cyan
    
    $results = @{
        "Administrator Rights" = $adminTest
        "Project Structure" = $structureTest
        "Build Files" = $buildTest
        "Dependencies" = $depsTest
        "Executable" = $exeTest
        "Setup Config" = $setupTest
        "Installer" = $installerTest
    }
    
    $passedTests = 0
    $totalTests = $results.Count
    
    foreach ($test in $results.GetEnumerator()) {
        $status = if ($test.Value) { "PASS"; $passedTests++ } else { "FAIL" }
        $color = if ($test.Value) { "Green" } else { "Red" }
        Write-Host "   $($test.Key): $status" -ForegroundColor $color
    }
    
    Write-Host ""
    if ($passedTests -eq $totalTests) {
        Write-Host "All tests passed! ($passedTests/$totalTests)" -ForegroundColor Green
        exit 0
    } else {
        Write-Host "Some tests failed. ($passedTests/$totalTests passed)" -ForegroundColor Red
        exit 1
    }
    
} catch {
    Write-Error "Test execution failed: $_"
    exit 1
}
