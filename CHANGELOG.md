# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [2.0.1] - 2025-11-19
### Fixed
- **Start with Windows**: Fixed a bug where the "Start with Windows" checkbox would not correctly reflect the registry state due to path quoting issues.
- **Installer**: Corrected version numbers and text in the installer and uninstaller to accurately reflect the current version.

## [2.0.0] - 2025-11-19

### Added
- **Minimalist UI**: Completely redesigned user interface with a dark theme, custom window chrome, and iOS-style toggle switches.
- **Single Instance Enforcement**: Implemented a Mutex-based check to prevent multiple instances of the application from running simultaneously.
- **Services Architecture**: Refactored core logic into dedicated services (`CursorEngine`, `HotKeyManager`, `StartupManager`, `SettingsManager`).
- **Models Architecture**: Extracted data models into a dedicated `Models` namespace.

### Changed
- **Project Structure**: Reorganized source code into `Services` and `Models` directories for better separation of concerns.
- **Documentation**: Updated README and contributing guidelines to meet professional standards.

### Fixed
- **Multiple Instances**: Fixed an issue where multiple instances of the application could be launched, causing hotkey conflicts.

## [1.1.0] - 2025-08-19

### Added
- **Auto-Hide Cursor**: Added functionality to automatically hide the cursor after a configurable period of inactivity.
- **Uninstaller**: Included a professional uninstaller that cleans up registry entries and user settings.

### Fixed
- **Startup Persistence**: Fixed an issue where the "Start with Windows" setting was not saving correctly.
- **Registry Management**: Improved robustness of registry operations for startup entries.

## [1.0.2] - 2025-05-10

### Added
- Initial release with basic cursor hiding functionality.
- Global hotkeys (Alt+H, Alt+S).
- System tray integration.
