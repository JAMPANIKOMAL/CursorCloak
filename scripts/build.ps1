param(
    [switch]$Clean,
    [switch]$CreateAllPackages,
    [switch]$AllPackages
)

Write-Host "CursorCloak Build Script v1.0.2" -ForegroundColor Cyan

function Clear-BuildArtifacts {
    Write-Host "Cleaning build artifacts..." -ForegroundColor Yellow
    
    $cleanPaths = @(
        ".\src\CursorCloak.UI\bin",
        ".\src\CursorCloak.UI\obj", 
        ".\src\CursorCloak.Engine\bin",
        ".\src\CursorCloak.Engine\obj",
        ".\publish",
        ".\Installer",
    ".\releases\CursorCloak-v2.0.1-*.zip"
    )
    
    foreach ($path in $cleanPaths) {
        if (Test-Path $path) {
            Remove-Item $path -Recurse -Force
            Write-Host "   Removed $path" -ForegroundColor Gray
        }
    }
    Write-Host "   Build artifacts cleaned" -ForegroundColor Green
}

function Build-Solution {
    Write-Host "Building solution..." -ForegroundColor Yellow
    
    dotnet restore
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Package restore failed"
        exit 1
    }
    
    dotnet build --configuration Release --no-restore
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Build failed"
        exit 1
    }
    Write-Host "   Build completed successfully" -ForegroundColor Green
}

function Publish-Framework {
    Write-Host "Publishing framework-dependent version..." -ForegroundColor Yellow
    
    New-Item -ItemType Directory -Path ".\publish\framework\ui" -Force | Out-Null
    
    dotnet publish "src\CursorCloak.UI\CursorCloak.UI.csproj" --configuration Release --runtime win-x64 --self-contained false --output ".\publish\framework\ui\"
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Framework publish failed"
        exit 1
    }
    Write-Host "   Framework version published" -ForegroundColor Green
}

function Publish-SelfContained {
    Write-Host "Publishing self-contained version..." -ForegroundColor Yellow
    
    New-Item -ItemType Directory -Path ".\publish\self-contained\ui" -Force | Out-Null
    
    dotnet publish "src\CursorCloak.UI\CursorCloak.UI.csproj" --configuration Release --runtime win-x64 --self-contained true -p:PublishSingleFile=true -p:EnableCompressionInSingleFile=true --output ".\publish\self-contained\ui\"
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Self-contained publish failed"
        exit 1
    }
    Write-Host "   Self-contained version published" -ForegroundColor Green
}

function Create-ZipPackages {
    Write-Host "Creating ZIP packages..." -ForegroundColor Yellow
    
    # Ensure releases directory exists
    New-Item -ItemType Directory -Path ".\releases" -Force | Out-Null
    
    # Framework-dependent ZIP
    Compress-Archive -Path ".\publish\framework\ui\*" -DestinationPath ".\releases\CursorCloak-v2.0.1-win-x64.zip" -CompressionLevel Optimal -Force
    $frameworkSize = [math]::Round((Get-Item ".\releases\CursorCloak-v2.0.1-win-x64.zip").Length / 1MB, 1)
    Write-Host "   Created releases\CursorCloak-v2.0.1-win-x64.zip ($frameworkSize MB)" -ForegroundColor Green
    
    # Self-contained ZIP
    Compress-Archive -Path ".\publish\self-contained\ui\*" -DestinationPath ".\releases\CursorCloak-v2.0.1-win-x64-selfcontained.zip" -CompressionLevel Optimal -Force
    $selfContainedSize = [math]::Round((Get-Item ".\releases\CursorCloak-v2.0.1-win-x64-selfcontained.zip").Length / 1MB, 1)
    Write-Host "   Created releases\CursorCloak-v2.0.1-win-x64-selfcontained.zip ($selfContainedSize MB)" -ForegroundColor Green
}

function Create-Installers {
    Write-Host "Creating installers with enhanced SmartScreen mitigation..." -ForegroundColor Yellow
    
    $innoSetupPath = "C:\Program Files (x86)\Inno Setup 6\ISCC.exe"
    if (-not (Test-Path $innoSetupPath)) {
        Write-Warning "InnoSetup not found. Skipping installer creation."
        return
    }
    
    # Framework-dependent installer
    if (Test-Path ".\scripts\setup.iss") {
        & $innoSetupPath ".\scripts\setup.iss"
        if ($LASTEXITCODE -eq 0) {
            Write-Host "   Created framework-dependent installer with enhanced metadata" -ForegroundColor Green
        }
    }
    
    # Self-contained installer
    if (Test-Path ".\scripts\setup-selfcontained.iss") {
        & $innoSetupPath ".\scripts\setup-selfcontained.iss"
        if ($LASTEXITCODE -eq 0) {
            Write-Host "   Created self-contained installer with enhanced metadata" -ForegroundColor Green
        }
    }
}

function Show-Summary {
    Write-Host ""
    Write-Host "All packages created successfully with enhanced SmartScreen mitigation!" -ForegroundColor Green
    Write-Host "Files created:" -ForegroundColor Cyan
    Write-Host "1. releases\CursorCloak_Setup_v2.0.1.exe" -ForegroundColor Gray
    Write-Host "2. releases\CursorCloak-v2.0.1-win-x64.zip" -ForegroundColor Gray
    Write-Host "3. releases\CursorCloak_Setup_v2.0.1_SelfContained.exe" -ForegroundColor Gray
    Write-Host "4. releases\CursorCloak-v2.0.1-win-x64-selfcontained.zip" -ForegroundColor Gray
    
    Write-Host ""
    Write-Host "File sizes:" -ForegroundColor Yellow
    if (Test-Path ".\releases\CursorCloak-v2.0.1-win-x64.zip") {
    $size = [math]::Round((Get-Item ".\releases\CursorCloak-v2.0.1-win-x64.zip").Length / 1MB, 1)
        Write-Host "Framework ZIP: $size MB" -ForegroundColor White
    }
    if (Test-Path ".\releases\CursorCloak-v2.0.1-win-x64-selfcontained.zip") {
    $size = [math]::Round((Get-Item ".\releases\CursorCloak-v2.0.1-win-x64-selfcontained.zip").Length / 1MB, 1)
        Write-Host "Self-contained ZIP: $size MB" -ForegroundColor White
    }
    if (Test-Path ".\releases\CursorCloak_Setup_v2.0.1.exe") {
    $size = [math]::Round((Get-Item ".\releases\CursorCloak_Setup_v2.0.1.exe").Length / 1MB, 1)
        Write-Host "Framework Installer: $size MB" -ForegroundColor White
    }
    if (Test-Path ".\releases\CursorCloak_Setup_v2.0.1_SelfContained.exe") {
    $size = [math]::Round((Get-Item ".\releases\CursorCloak_Setup_v2.0.1_SelfContained.exe").Length / 1MB, 1)
        Write-Host "Self-contained Installer: $size MB" -ForegroundColor White
    }
}

# Main execution
try {
    if ($Clean) {
        Clear-BuildArtifacts
        Write-Host "Clean completed. Use -CreateAllPackages or -AllPackages to build." -ForegroundColor Cyan
        return
    }
    
    if ($CreateAllPackages -or $AllPackages) {
        Build-Solution
        Publish-Framework
        Publish-SelfContained
        Create-ZipPackages
        Create-Installers
        Show-Summary
    } else {
        Build-Solution
        Write-Host "Build completed. Use -CreateAllPackages or -AllPackages to create deployment packages." -ForegroundColor Cyan
    }

} catch {
    Write-Error "Build failed: $($_.Exception.Message)"
    exit 1
}
