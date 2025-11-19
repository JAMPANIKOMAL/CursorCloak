# CursorCloak

CursorCloak is a Windows utility designed to provide system-wide control over mouse cursor visibility. It allows users to hide and show the cursor using global hotkeys, making it suitable for screen recording, presentations, and distraction-free environments.

## Features

- **Global Hotkeys**: Toggle cursor visibility system-wide using `Alt+H` (Hide) and `Alt+S` (Show).
- **Auto-Hide**: Automatically hides the cursor after a user-configurable period of inactivity.
- **Background Operation**: Runs silently in the background without cluttering the taskbar.
- **Persistent Settings**: Saves user preferences and startup configuration between sessions.
- **Single Instance**: Ensures only one instance of the application runs to prevent conflicts.

## Installation

### Prerequisites
- Windows 10 or Windows 11 (64-bit)
- .NET 9.0 Desktop Runtime

### Installation Methods

#### Installer (Recommended)
1. Download the latest `CursorCloak_Setup_v2.0.1.exe` from the [Releases](https://github.com/JAMPANIKOMAL/CursorCloak/releases) page.
2. Run the installer and follow the on-screen instructions.
3. The application will launch automatically upon completion.

#### Portable Version
1. Download `CursorCloak-v2.0.1-win-x64.zip`.
2. Extract the contents to a preferred location.
3. Run `CursorCloak.UI.exe`.

## Usage

1. **Launch**: Run CursorCloak from the Start Menu or installation directory.
2. **Hide Cursor**: Press `Alt+H` to hide the cursor.
3. **Show Cursor**: Press `Alt+S` to restore the cursor.
4. **Auto-Hide**: Enable the "Auto-Hide" toggle in the main window to hide the cursor after inactivity.
5. **Configuration**: Adjust the inactivity timeout in the UI.

## Architecture

The project follows a clean architecture pattern with a separation of concerns:

- **CursorCloak.UI**: The main WPF application handling user interaction and visual presentation.
  - **Services**: Contains business logic (`CursorEngine`, `HotKeyManager`, `StartupManager`).
  - **Models**: Defines data structures (`Settings`, `UserConfig`).
- **CursorCloak.Engine**: A low-level library for Windows API interactions (P/Invoke).

## Building from Source

### Requirements
- Visual Studio 2022 or JetBrains Rider
- .NET 9.0 SDK

### Build Steps
1. Clone the repository:
   ```bash
   git clone https://github.com/JAMPANIKOMAL/CursorCloak.git
   ```
2. Navigate to the solution directory:
   ```bash
   cd CursorCloak
   ```
3. Build the solution:
   ```bash
   dotnet build --configuration Release
   ```

## Contributing

We welcome contributions to CursorCloak. Please refer to [CONTRIBUTING.md](docs/CONTRIBUTING.md) for detailed guidelines on coding standards, pull requests, and the development process.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
