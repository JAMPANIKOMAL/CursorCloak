# PowerShell build script for CursorCloak
Write-Host "Building CursorCloak solution..." -ForegroundColor Green

# Clean previous builds
Write-Host "Cleaning previous builds..." -ForegroundColor Yellow
dotnet clean --configuration Release

# Restore NuGet packages
Write-Host "Restoring NuGet packages..." -ForegroundColor Yellow
dotnet restore

# Build the solution
Write-Host "Building solution..." -ForegroundColor Yellow
dotnet build --configuration Release --no-restore

if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed!" -ForegroundColor Red
    Read-Host "Press Enter to exit"
    exit $LASTEXITCODE
}

# Publish the UI application for deployment
Write-Host "Publishing UI application..." -ForegroundColor Yellow
dotnet publish CursorCloak.UI\CursorCloak.UI.csproj --configuration Release --runtime win-x64 --self-contained true --output .\Publish\

if ($LASTEXITCODE -ne 0) {
    Write-Host "Publish failed!" -ForegroundColor Red
    Read-Host "Press Enter to exit"
    exit $LASTEXITCODE
}

Write-Host "Build completed successfully!" -ForegroundColor Green
Write-Host ""
Write-Host "Binaries location:" -ForegroundColor Cyan
Write-Host "- Debug: bin\Debug\net9.0-windows\" -ForegroundColor White
Write-Host "- Release: bin\Release\net9.0-windows\" -ForegroundColor White
Write-Host "- Published: Publish\" -ForegroundColor White
Write-Host ""

Read-Host "Press Enter to exit"
