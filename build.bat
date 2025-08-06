@echo off
REM Build script for CursorCloak
echo Building CursorCloak solution...

REM Clean previous builds
echo Cleaning previous builds...
dotnet clean --configuration Release

REM Restore NuGet packages
echo Restoring NuGet packages...
dotnet restore

REM Build the solution
echo Building solution...
dotnet build --configuration Release --no-restore

if %ERRORLEVEL% neq 0 (
    echo Build failed!
    pause
    exit /b %ERRORLEVEL%
)

REM Publish the UI application for deployment
echo Publishing UI application...
dotnet publish CursorCloak.UI\CursorCloak.UI.csproj --configuration Release --runtime win-x64 --self-contained true --output .\Publish\

if %ERRORLEVEL% neq 0 (
    echo Publish failed!
    pause
    exit /b %ERRORLEVEL%
)

echo Build completed successfully!
echo.
echo Binaries location:
echo - Debug: bin\Debug\net9.0-windows\
echo - Release: bin\Release\net9.0-windows\
echo - Published: Publish\
echo.
pause
