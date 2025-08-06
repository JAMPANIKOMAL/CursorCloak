@echo off
REM Build script for CursorCloak
echo Building CursorCloak solution...

REM Check if running as administrator
net session >nul 2>&1
if %errorLevel% neq 0 (
    echo This script requires administrator privileges to build properly.
    echo Please run as administrator.
    pause
    exit /b 1
)

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

REM Build the UI project specifically
echo Building UI project for Release...
dotnet build CursorCloak.UI\CursorCloak.UI.csproj --configuration Release --no-restore

if %ERRORLEVEL% neq 0 (
    echo UI build failed!
    pause
    exit /b %ERRORLEVEL%
)

REM Publish the UI application for deployment (optional)
echo.
echo Do you want to create a self-contained deployment? (y/n)
set /p choice=
if /i "%choice%"=="y" (
    echo Publishing UI application...
    dotnet publish CursorCloak.UI\CursorCloak.UI.csproj --configuration Release --runtime win-x64 --self-contained true --output .\Publish\
    
    if %ERRORLEVEL% neq 0 (
        echo Publish failed!
        pause
        exit /b %ERRORLEVEL%
    )
)

echo Build completed successfully!
echo.
echo Binaries location:
echo - Release: CursorCloak.UI\bin\Release\net9.0-windows\
if /i "%choice%"=="y" (
    echo - Published: Publish\
)
echo.
echo To create installer:
echo 1. Ensure InnoSetup is installed
echo 2. Right-click on setup.iss and select "Compile"
echo 3. The installer will be created in the Installer\ directory
echo.
echo Note: The application requires administrator privileges to run.
echo.
pause
