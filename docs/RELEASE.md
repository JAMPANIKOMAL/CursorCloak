# Release Process

This document outlines the steps to create a new release for CursorCloak.

## Prerequisites
- Access to the GitHub repository
- Git installed locally
- `dotnet` CLI installed (for version updates)

## Steps

1.  **Update Version Number**
    - Open `src/CursorCloak.UI/CursorCloak.UI.csproj`.
    - Update `<AssemblyVersion>` and `<FileVersion>` to the new version number (e.g., `2.0.1.0`).
    - Save the file.

2.  **Update Documentation**
    - Update `README.md` to reflect any new features or changes.
    - Update `docs/VERSION.md` (if it exists) or the Version History section in `README.md`.

3.  **Commit Changes**
    ```bash
    git add .
    git commit -m "chore: Release v2.0.1"
    git push
    ```

4.  **Create Tag**
    The release workflow is triggered automatically when a tag starting with `v` is pushed.
    ```bash
    git tag v2.0.1
    git push origin v2.0.1
    ```

5.  **Verify Release**
    - Go to the [GitHub Actions](https://github.com/JAMPANIKOMAL/CursorCloak/actions) tab.
    - Verify that the "Build and Release" workflow is running.
    - Once completed, check the [Releases](https://github.com/JAMPANIKOMAL/CursorCloak/releases) page for the new release.

## Troubleshooting
- If the release workflow fails, check the Action logs for details.
- Ensure the tag format is correct (e.g., `v1.0.0`, `v2.1.0`).
