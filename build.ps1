param(
    [switch]$Clean,
    [switch]$CreateAllPackages
)

Write-Host "CursorCloak Build Script v1.0.0" -ForegroundColor Cyan

function Clear-BuildArtifacts {
    Write-Host "Cleaning build artifacts..." -ForegroundColor Yellow
    
    $cleanPaths = @(
        ".\CursorCloak.UI\bin",
        ".\CursorCloak.UI\obj", 
        ".\CursorCloak.Engine\bin",
        ".\CursorCloak.Engine\obj",
        ".\publish",
        ".\Installer",
        ".\CursorCloak-v1.0.0-*.zip"
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
    
    dotnet publish "CursorCloak.UI\CursorCloak.UI.csproj" --configuration Release --runtime win-x64 --self-contained false --output ".\publish\framework\ui\"
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Framework publish failed"
        exit 1
    }
    Write-Host "   Framework version published" -ForegroundColor Green
}

function Publish-SelfContained {
    Write-Host "Publishing self-contained version..." -ForegroundColor Yellow
    
    New-Item -ItemType Directory -Path ".\publish\self-contained\ui" -Force | Out-Null
    
    dotnet publish "CursorCloak.UI\CursorCloak.UI.csproj" --configuration Release --runtime win-x64 --self-contained true -p:PublishSingleFile=true -p:EnableCompressionInSingleFile=true --output ".\publish\self-contained\ui\"
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Self-contained publish failed"
        exit 1
    }
    Write-Host "   Self-contained version published" -ForegroundColor Green
}

function Create-ZipPackages {
    Write-Host "Creating ZIP packages..." -ForegroundColor Yellow
    
    # Framework-dependent ZIP
    Compress-Archive -Path ".\publish\framework\ui\*" -DestinationPath "CursorCloak-v1.0.0-win-x64.zip" -CompressionLevel Optimal -Force
    $frameworkSize = [math]::Round((Get-Item "CursorCloak-v1.0.0-win-x64.zip").Length / 1MB, 1)
    Write-Host "   Created CursorCloak-v1.0.0-win-x64.zip ($frameworkSize MB)" -ForegroundColor Green
    
    # Self-contained ZIP
    Compress-Archive -Path ".\publish\self-contained\ui\*" -DestinationPath "CursorCloak-v1.0.0-win-x64-selfcontained.zip" -CompressionLevel Optimal -Force
    $selfContainedSize = [math]::Round((Get-Item "CursorCloak-v1.0.0-win-x64-selfcontained.zip").Length / 1MB, 1)
    Write-Host "   Created CursorCloak-v1.0.0-win-x64-selfcontained.zip ($selfContainedSize MB)" -ForegroundColor Green
}

function Create-Installers {
    Write-Host "Creating installers..." -ForegroundColor Yellow
    
    $innoSetupPath = "C:\Program Files (x86)\Inno Setup 6\ISCC.exe"
    if (-not (Test-Path $innoSetupPath)) {
        Write-Warning "InnoSetup not found. Skipping installer creation."
        return
    }
    
    New-Item -ItemType Directory -Path ".\Installer" -Force | Out-Null
    
    # Framework-dependent installer
    if (Test-Path "setup.iss") {
        & $innoSetupPath "setup.iss"
        if ($LASTEXITCODE -eq 0) {
            Write-Host "   Created framework-dependent installer" -ForegroundColor Green
        }
    }
    
    # Self-contained installer
    if (Test-Path "setup-selfcontained.iss") {
        & $innoSetupPath "setup-selfcontained.iss"
        if ($LASTEXITCODE -eq 0) {
            Write-Host "   Created self-contained installer" -ForegroundColor Green
        }
    }
}

# Main execution
try {
    if ($Clean) {
        Clear-BuildArtifacts
    }
    
    if ($CreateAllPackages) {
        Build-Solution
        Publish-Framework
        Publish-SelfContained
        Create-ZipPackages
        Create-Installers
        
        Write-Host ""
        Write-Host "All packages created successfully!" -ForegroundColor Green
        Write-Host "Files created:" -ForegroundColor Cyan
        Write-Host "1. CursorCloak_Setup_v1.0.0.exe" -ForegroundColor Gray
        Write-Host "2. CursorCloak-v1.0.0-win-x64.zip" -ForegroundColor Gray
        Write-Host "3. CursorCloak_Setup_v1.0.0_SelfContained.exe" -ForegroundColor Gray
        Write-Host "4. CursorCloak-v1.0.0-win-x64-selfcontained.zip" -ForegroundColor Gray
    } else {
        Build-Solution
    }

} catch {
    Write-Error "Build failed: $($_.Exception.Message)"
    exit 1
}