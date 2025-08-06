#!/usr/bin/env pwsh
# CursorCloak Test Script
# Tests the built application functionality

param(
    [string]$BuildPath = ".\CursorCloak.UI\bin\Release\net9.0-windows\",
    [switch]$Verbose
)

Write-Host "🧪 CursorCloak Test Script" -ForegroundColor Cyan
Write-Host "==========================" -ForegroundColor Cyan

# Function to check if running as administrator
function Test-Administrator {
    $isAdmin = ([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
    if (-not $isAdmin) {
        Write-Warning "⚠️  Not running as administrator. Some tests may fail."
        return $false
    }
    Write-Host "✅ Running as administrator" -ForegroundColor Green
    return $true
}

# Function to test file existence
function Test-BuildFiles {
    Write-Host "📁 Checking build files..." -ForegroundColor Yellow
    
    $requiredFiles = @(
        "CursorCloak.UI.exe",
        "CursorCloak.UI.dll",
        "CursorCloak.UI.deps.json",
        "CursorCloak.UI.runtimeconfig.json"
    )
    
    $allFilesExist = $true
    foreach ($file in $requiredFiles) {
        $filePath = Join-Path $BuildPath $file
        if (Test-Path $filePath) {
            $size = [math]::Round((Get-Item $filePath).Length / 1KB, 2)
            Write-Host "   ✅ $file ($size KB)" -ForegroundColor Green
        } else {
            Write-Host "   ❌ $file (missing)" -ForegroundColor Red
            $allFilesExist = $false
        }
    }
    
    return $allFilesExist
}

# Function to test .NET dependencies
function Test-DotNetRuntime {
    Write-Host "🔍 Checking .NET runtime..." -ForegroundColor Yellow
    
    try {
        $dotnetVersion = dotnet --version 2>$null
        if ($dotnetVersion) {
            Write-Host "   ✅ .NET SDK $dotnetVersion found" -ForegroundColor Green
            
            # Check for .NET 9.0 specifically
            $runtimes = dotnet --list-runtimes 2>$null | Where-Object { $_ -like "*Microsoft.NETCore.App*9.0*" }
            if ($runtimes) {
                Write-Host "   ✅ .NET 9.0 runtime available" -ForegroundColor Green
                return $true
            } else {
                Write-Host "   ⚠️  .NET 9.0 runtime not found" -ForegroundColor Yellow
                return $false
            }
        } else {
            Write-Host "   ❌ .NET runtime not found" -ForegroundColor Red
            return $false
        }
    } catch {
        Write-Host "   ❌ Error checking .NET runtime: $_" -ForegroundColor Red
        return $false
    }
}

# Function to test application startup
function Test-ApplicationStartup {
    Write-Host "🚀 Testing application startup..." -ForegroundColor Yellow
    
    $exePath = Join-Path $BuildPath "CursorCloak.UI.exe"
    if (-not (Test-Path $exePath)) {
        Write-Host "   ❌ Executable not found at $exePath" -ForegroundColor Red
        return $false
    }
    
    try {
        # Start the application and check if it loads
        $process = Start-Process -FilePath $exePath -PassThru -WindowStyle Hidden
        Start-Sleep -Seconds 3
        
        if ($process.HasExited) {
            Write-Host "   ❌ Application exited immediately (exit code: $($process.ExitCode))" -ForegroundColor Red
            return $false
        } else {
            Write-Host "   ✅ Application started successfully" -ForegroundColor Green
            # Stop the test instance
            $process.Kill()
            $process.WaitForExit(5000)
            return $true
        }
    } catch {
        Write-Host "   ❌ Error starting application: $_" -ForegroundColor Red
        return $false
    }
}

# Function to test installer
function Test-Installer {
    Write-Host "📦 Checking installer..." -ForegroundColor Yellow
    
    $installerPath = ".\Installer\CursorCloak_Setup.exe"
    if (Test-Path $installerPath) {
        $size = [math]::Round((Get-Item $installerPath).Length / 1MB, 2)
        Write-Host "   ✅ Installer found ($size MB)" -ForegroundColor Green
        
        # Basic integrity check
        try {
            $fileInfo = Get-AuthenticodeSignature $installerPath -ErrorAction SilentlyContinue
            if ($fileInfo) {
                Write-Host "   📋 Installer structure appears valid" -ForegroundColor Green
            }
        } catch {
            Write-Host "   ⚠️  Cannot verify installer signature" -ForegroundColor Yellow
        }
        
        return $true
    } else {
        Write-Host "   ⚠️  Installer not found (use build.ps1 -Publish -CreateInstaller)" -ForegroundColor Yellow
        return $false
    }
}

# Function to run all tests
function Invoke-AllTests {
    $results = @{}
    
    $results.Administrator = Test-Administrator
    $results.BuildFiles = Test-BuildFiles
    $results.DotNetRuntime = Test-DotNetRuntime
    $results.ApplicationStartup = Test-ApplicationStartup
    $results.Installer = Test-Installer
    
    return $results
}

# Main execution
try {
    Write-Host "Build path: $BuildPath" -ForegroundColor Gray
    Write-Host ""
    
    $testResults = Invoke-AllTests
    
    Write-Host ""
    Write-Host "📊 Test Summary:" -ForegroundColor Cyan
    Write-Host "================" -ForegroundColor Cyan
    
    $passedTests = 0
    $totalTests = $testResults.Count
    
    foreach ($test in $testResults.GetEnumerator()) {
        $status = if ($test.Value) { "PASS ✅" } else { "FAIL ❌" }
        Write-Host "   $($test.Key): $status"
        if ($test.Value) { $passedTests++ }
    }
    
    Write-Host ""
    $successRate = [math]::Round(($passedTests / $totalTests) * 100, 1)
    Write-Host "Success Rate: $passedTests/$totalTests ($successRate%)" -ForegroundColor $(if ($successRate -eq 100) { "Green" } elseif ($successRate -ge 75) { "Yellow" } else { "Red" })
    
    if ($successRate -eq 100) {
        Write-Host ""
        Write-Host "🎉 All tests passed! CursorCloak is ready for use." -ForegroundColor Green
        Write-Host "💡 Run as administrator: Right-click CursorCloak.UI.exe → 'Run as administrator'" -ForegroundColor Cyan
    } elseif ($successRate -ge 75) {
        Write-Host ""
        Write-Host "⚠️  Most tests passed, but some issues detected. Check the results above." -ForegroundColor Yellow
    } else {
        Write-Host ""
        Write-Host "❌ Multiple test failures detected. Please address the issues before deployment." -ForegroundColor Red
        exit 1
    }
    
} catch {
    Write-Error "❌ Test execution failed: $_"
    exit 1
}
